using AspectCore.DynamicProxy;
using System.Transactions;

namespace Fintech.Library.Core.Aspects.AspectCore;

public class TransactionAspect : AbstractInterceptorAttribute
{
    public async override Task Invoke(AspectContext context, AspectDelegate next)
    {

        using TransactionScope scope = new(TransactionScopeAsyncFlowOption.Enabled);
        try
        {
            await next(context);
            scope.Complete();
        }
        catch
        {
            scope.Dispose();
            throw;
        }
    }
}
