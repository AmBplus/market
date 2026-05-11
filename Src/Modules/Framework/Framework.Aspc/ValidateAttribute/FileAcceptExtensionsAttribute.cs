using System.ComponentModel.DataAnnotations;
using Framework.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Framework.Aspc.ValidateAttribute;

public class FileAcceptExtensionsAttribute : ValidationAttribute, IClientModelValidator
{
    public FileAcceptExtensionsAttribute(string[] validExtensions)
    {
        ValidExtensions = validExtensions;
    }
    public FileAcceptExtensionsAttribute(string validExtension , bool isSingle )
    {
        ValidExtension = validExtension;
        IsSingle = isSingle;
    }

    private string[] ValidExtensions { get; }
    private string ValidExtension { get; }
    private bool IsSingle { get; }

    public void AddValidation(ClientModelValidationContext context)
    {
        var validEx = string.Empty;
        if (IsSingle)
        {
            validEx = ValidExtension;
        }
        else
        {
         
            for (var i = 0; i < ValidExtensions.Length; i++)
                if (i == ValidExtensions.Length - 1)
                    validEx += ValidExtensions[i];
                else
                    validEx += $"{ValidExtensions[i]},";
        }
   
        if (ErrorMessageResourceName != null)
            context.Attributes.Add("data-val-ValidFileExtension", ErrorMessageResourceName);
        else
            context.Attributes.Add("data-val-ValidFileExtension", $" {Messages.ValidFormatis}{validEx}");
        context.Attributes.Add("accept", validEx);
    }

    public override bool IsValid(object? value)
    {
        if (value == null) return true;
        if (value is IFormFile file)
            return IsValidFile(file);
        if (value is ICollection<IFormFile> files) return IsValidFiles(files);
        return false;
    }

    public bool IsValidFiles(ICollection<IFormFile> formFiles)
    {
        foreach (var file in formFiles)
            if (!IsValidFile(file))
                return false;
        return true;
    }

    public bool IsValidFile(IFormFile file)
    {
        var fileExtension = Path.GetExtension(file.FileName);
        if (ValidExtensions.Contains(fileExtension)) return true;
        return false;
    }
}
