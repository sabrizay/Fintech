using AspectCore.DynamicProxy;
using AspectCore.DynamicProxy.Parameters;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Fintech.ExternalService.CloudService.Caching;
using Fintech.Library.Utilities.Extensions;

namespace Fintech.Library.Core.Aspects.AspectCore;

public class CacheAspect
    : AbstractInterceptorAttribute
{
    private readonly int _duration = 60;
    //private string key = "";
    //private readonly ICacheManager _cacheManager;
    private readonly ICacheService _cacheService;
    //private readonly AsyncLock asyncLock;

    public CacheAspect(int Duration)
    {
        _cacheService = DependencyInjectionExtensions.ServiceTool.ServiceProvider.GetService<ICacheService>();
        _duration = Duration;
    }

    //public CacheAspect(int Duration, string Key)
    //{
    //    //_cacheManager = ServiceTool.ServiceProvider.GetService<ICacheManager>();
    //    _cacheService = DependencyInjectionExtensions.ServiceTool.ServiceProvider.GetService<ICacheService>();
    //    _duration = Duration;
    //    key = Key;
    //}

    public async override Task Invoke(AspectContext context, AspectDelegate next)
    {
        bool IsAsync = context.IsAsync();

        var methodReturnType = context.GetReturnParameter().Type;

        //Method dönüş verisi içermiyorsa caching yapma
        if (methodReturnType == typeof(void) || methodReturnType == typeof(Task) || methodReturnType == typeof(ValueTask))
        {
            await next(context);
            return;
        }
        var returnType = methodReturnType;


        if (IsAsync)
        {
            //Gets the type of the asynchronous return
            returnType = returnType.GenericTypeArguments.FirstOrDefault();
        }

        //if (string.IsNullOrEmpty(key))
        string key = $"{context.ImplementationMethod.DeclaringType.FullName}.{context.ImplementationMethod.Name} Param:{string.Join(',', context.Parameters)} ";

        //If the cache has a value, the cache value is returned directly
        if (_cacheService.IsAdd(key))
        {
            //Reflection gets the cache value, which is equivalent to cache.HashGet <>(key,param)
            var value = _cacheService.GetData(key);
            if (IsAsync)
            {
                //Determine whether it is a task or a valuetask
                if (methodReturnType == typeof(Task<>).MakeGenericType(returnType))
                {
                    try
                    {
                        var exmpleData = JsonConvert.DeserializeObject(value.ToString(), returnType);

                        //Reflection gets the return value of task < > type, which is equivalent to Task.FromResult (value)
                        context.ReturnValue = typeof(Task).GetMethod(nameof(Task.FromResult)).MakeGenericMethod(returnType).Invoke(null, new[] { exmpleData });
                    }
                    catch{}
                }
                else if (methodReturnType == typeof(ValueTask<>).MakeGenericType(returnType))
                {
                    //Reflection builds the return value of valuetask < > type, which is equivalent to new valuetask (value)
                    context.ReturnValue = Activator.CreateInstance(typeof(ValueTask<>).MakeGenericType(returnType), value);
                }
            }
            else
            {
                context.ReturnValue = value;
            }
            return;
        }

        await next(context);
        object returnValue;
        if (IsAsync)
        {
            returnValue = await context.UnwrapAsyncReturnValue();
            //Reflection gets the value of the asynchronous result, equivalent to（ context.ReturnValue  as Task<>).Result
            //returnValue = typeof(Task<>).MakeGenericType(returnType).GetProperty(nameof(Task.Result)).GetValue(context.ReturnValue);

        }
        else
        {
            returnValue = context.ReturnValue;
        }

        await _cacheService.SetDataAsync(key, returnValue, _duration);
    }
}
