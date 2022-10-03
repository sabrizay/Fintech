using AspectCore.DynamicProxy;
using FluentValidation;
using Fintech.Library.Core.CrossCuttingCorserns.Validation;

namespace Fintech.Library.Core.Aspects.AspectCore;

public class ValidationAspect : AbstractInterceptorAttribute
{
    private readonly Type _validatorType;

    public ValidationAspect(Type validatorType)
    {
        if (!typeof(IValidator).IsAssignableFrom(validatorType))
        {
            throw new Exception("Wrong validation type");
        }
        _validatorType = validatorType;
    }

    public override async Task Invoke(AspectContext context, AspectDelegate next)
    {

        bool IsAsync = context.IsAsync();

        IValidator validator = (IValidator)Activator.CreateInstance(_validatorType);

        var entityType = _validatorType.BaseType.GetGenericArguments().First();


        var entities = context.Parameters.Where(t => t.GetType() == entityType);


        foreach (var entity in entities)
        {
            ValidationTool.Validate(validator, entity);
        }

        await next(context);
    }
}
