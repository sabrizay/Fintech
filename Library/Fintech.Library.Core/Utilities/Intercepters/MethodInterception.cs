using Castle.DynamicProxy;

namespace Fintech.Library.Core.Utilities.Intercepters;

public abstract class MethodInterception : MethodInterceptionBaseAttribute
{
    virtual protected void OnBefore(IInvocation invocation) { }
    virtual protected void OnAfter(IInvocation invocation) { }
    virtual protected void OnException(IInvocation invocation) { }
    virtual protected void OnSuccess(IInvocation invocation) { }
    public override void Intercept(IInvocation invocation)
    {
        var isSuccess = true;
        OnBefore(invocation);
        try
        {
            invocation.Proceed();
        }
        catch (Exception ex)
        {
            isSuccess = false;
            OnException(invocation);
            throw new Exception(ex.Message);
        }
        finally
        {
            if (isSuccess)
                OnSuccess(invocation);

        }

        OnAfter(invocation);
    }
}
