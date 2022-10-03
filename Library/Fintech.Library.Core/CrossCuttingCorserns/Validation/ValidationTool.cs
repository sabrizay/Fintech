﻿using FluentValidation;


namespace Fintech.Library.Core.CrossCuttingCorserns.Validation;

public static class ValidationTool
{
    public static void Validate(IValidator validator, object entity)
    {
        var result = validator.Validate(new ValidationContext<object>(entity));

        if (!result.IsValid)
            throw new ValidationException(result.Errors);
    }
}
