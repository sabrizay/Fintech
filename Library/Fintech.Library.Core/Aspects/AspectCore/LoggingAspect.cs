using AspectCore.DynamicProxy;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace Fintech.Library.Core.Aspects.AspectCore;

public class LoggingAspect : AbstractInterceptorAttribute
{
    private ILogger _logger;
    private IHttpContextAccessor _httpContextAccessor;
    private readonly bool _isOnlyLogError;
    public LoggingAspect(bool IsOnlyLogError)
    {
        _isOnlyLogError = IsOnlyLogError;
    }
    public override async Task Invoke(AspectContext context, AspectDelegate next)
    {
        _logger = context.ServiceProvider.GetService<ILoggerFactory>().CreateLogger<ILogger>();
        _httpContextAccessor = context.ServiceProvider.GetService<IHttpContextAccessor>();

        string userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        string userIP = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();


        string methodName = $"{context.ImplementationMethod.DeclaringType.FullName}.{context.ImplementationMethod.Name}";

        string param = string.Join(',', context.Parameters);
        string message = $"Method:{methodName} Params:{param} userId:{userId} userIp:{userIP}";

        try
        {
            if (_isOnlyLogError == false)
                _logger.LogInformation("{message}", message);

            await next(context);

            //if (_isOnlyLogError == false)
            //    _logger.LogInformation("Method:{method} Params:{param} status:Ended", methodName, param);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{message}", message);
            throw;
        }
    }
}
