using AspectCore.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;
using Fintech.Library.Core.CrossCuttingCorserns.Caching;
using Fintech.Library.Core.Utilities.IoC;
using static Fintech.Library.Utilities.Extensions.DependencyInjectionExtensions;

namespace Fintech.Library.Core.Aspects.AspectCore;

public class CacheRemoveAspect : AbstractInterceptorAttribute
{
    private readonly ICacheManager _cacheManager;
    private readonly string _pattern;

    public CacheRemoveAspect(string pattern)
    {
        _cacheManager = ServiceTool.ServiceProvider.GetService<ICacheManager>();
        _pattern = pattern;
    }

    public async override Task Invoke(AspectContext context, AspectDelegate next)
    {

        await next(context);

        _cacheManager.RemoveByPattern(_pattern);

    }
}
