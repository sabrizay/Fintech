using AspectCore.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Fintech.Library.Core.Aspects.AspectCore;

public class PerformanceAspect : AbstractInterceptorAttribute
{
    private readonly int _perfonmanceMesure = 0;
    public PerformanceAspect(int PerformanceMesure)
    {
        _perfonmanceMesure = PerformanceMesure;
    }
    public override async Task Invoke(AspectContext context, AspectDelegate next)
    {

        Stopwatch _stopwatch = context.ServiceProvider.GetService<Stopwatch>();

        _stopwatch.Start();
        await next(context);

        _stopwatch.Stop();

        if (_stopwatch.Elapsed.TotalSeconds > _perfonmanceMesure)
        {
            var logger = context.ServiceProvider.GetService<ILoggerFactory>().CreateLogger<ILogger>();
            logger.LogWarning("Performance Issue Mesure:{mesure} Execute:{execute}", _perfonmanceMesure, _stopwatch.Elapsed.TotalSeconds);
        }

        _stopwatch.Reset();
    }
}
