

using Framework.ResultHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using System.Linq;
using System.Text.RegularExpressions;

namespace Framework.Validator;

public static class ValidationHelper
{
    static ValidationHelper()
    {
    }

    public static ResultOperation GetFailedResultWithError_s(this IList<ValidationResult> validationResults)
    {
        var errors = validationResults.Select(x => x.ErrorMessage).ToList();
        return ResultOperation.ToFailedResult(errors!);
    }

    public static bool IsValid<T>(this T entity) where T : class
    {
        var validationContext =
            new ValidationContext(instance: entity);

        var validationResults =
            new List<ValidationResult>();

        var isValid =
            System.ComponentModel.DataAnnotations.Validator
                .TryValidateObject(instance: entity, validationContext: validationContext,
                    validationResults: validationResults, validateAllProperties: true);
        
        return isValid;
    }


    public static ResultOperation
        GetValidationResults<T>(this T entity) where T : class
    {
        var validationContext =
            new ValidationContext(instance: entity);

        var validationResults =
            new List<ValidationResult>();

        //var isValid =
        System.ComponentModel.DataAnnotations.Validator
            .TryValidateObject(instance: entity, validationContext: validationContext,
                validationResults: validationResults, validateAllProperties: true);
       
        if (validationResults.Count > 0)
        {
            var resultError = validationResults.GetFailedResultWithError_s();
            return ResultOperation.ToFailedResult(resultError.Message!);
        }
       
        return ResultOperation.ToSuccessResult();
    }

    public static bool IsValidIranianNationalCode(string input)
    {
        if (!Regex.IsMatch(input, @"^\d{10}$"))
            return false;

        var check = Convert.ToInt32(input.Substring(9, 1));
        var sum = Enumerable.Range(0, 9)
            .Select(x => Convert.ToInt32(input.Substring(x, 1)) * (10 - x))
            .Sum() % 11;

        return (sum < 2 && check == sum) || (sum >= 2 && check + sum == 11);
    }

}
