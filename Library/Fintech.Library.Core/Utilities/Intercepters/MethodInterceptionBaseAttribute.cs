using Castle.DynamicProxy;

namespace Fintech.Library.Core.Utilities.Intercepters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public abstract class MethodInterceptionBaseAttribute : Attribute, IInterceptor
{
    public int Prority { get; set; }

    public virtual void Intercept(IInvocation invocation)
    {

    }
}
