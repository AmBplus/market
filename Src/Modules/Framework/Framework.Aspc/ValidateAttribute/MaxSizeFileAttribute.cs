using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Framework.Aspc.ValidateAttribute;

public class MaxSizeFileAttribute : ValidationAttribute, IClientModelValidator
{
    public MaxSizeFileAttribute(int maxSize)
    {
        MaxSize = maxSize;
    }

    private int MaxSize { get; }

    public void AddValidation(ClientModelValidationContext context)
    {
        context.Attributes.Add("data-val", "true");
        var maxSizeOnKiloByte = MaxSize / 1024;
        var message = string.Empty;
        if(maxSizeOnKiloByte > 1000) {
            message = string.Format("حداکثر فایل آپلودی باید {0} {1} باشد", maxSizeOnKiloByte, "مگابایت");
        }
        else
        {
            message = string.Format("حداکثر فایل آپلودی باید {0} {1} باشد", maxSizeOnKiloByte, "کیلوبایت");
        }
        context.Attributes.Add("data-val-MaxFileSize", message);
    }

    public override bool IsValid(object? value)
    {
        if (value == null) return true;
        if (value is not IFormFile file) return true;
        if (file.Length < MaxSize) return true;
        return false;
    }
}
