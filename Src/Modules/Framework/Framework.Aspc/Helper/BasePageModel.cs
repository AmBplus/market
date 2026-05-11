using Framework.Resources;


namespace Infrastructure;

/// <summary>
/// Version 3.0
/// </summary>
public abstract class BasePageModel :
  Microsoft.AspNetCore.Mvc.RazorPages.PageModel, Messages.IMessageHandler
{



    public bool AddPageError(string? message)
    {
        return AddMessage
          (type: Messages.MessageType.PageError, message: HandleErrorMessage(message));
    }
    
    public bool AddPageWarning(string? message)
    {
        return AddMessage
          (type: Messages.MessageType.PageWarning, message: message);
    }

    public bool AddPageSuccess(string? message)
    {
        return AddMessage
          (type: Messages.MessageType.PageSuccess, message: HandleSuccessMessage(message));
    }

    public bool AddToastError(string? message)
    {
        return AddMessage
          (type: Messages.MessageType.ToastError, message: HandleErrorMessage(message));
    }

    public bool AddToastWarning(string? message)
    {
        return AddMessage
          (type: Messages.MessageType.ToastWarning, message: message);
    }

    public bool AddToastSuccess(string? message)
    {
        return AddMessage
          (type: Messages.MessageType.ToastSuccess, message: HandleSuccessMessage(message));
    }
    public bool AddToastInfo(string? message)
    {
        return AddMessage
          (type: Messages.MessageType.ToastInfo, message: message );
    }
    public bool AddMessage(Messages.MessageType type, string? message)
    {
        return Messages.Utility.AddMessage
          (tempData: TempData, type: type, message: message);
    }

    protected string SetReturnUrl(string? returnUrl)
    {
        if (string.IsNullOrWhiteSpace(value: returnUrl))
        {
            returnUrl = "./Index";
        }

        return returnUrl;
    }

    public bool AddAlertSuccess(string? message)
    {
        return AddMessage
        (type: Messages.MessageType.AlertSuccess, message: HandleSuccessMessage(message));
    }

    public bool AddAlertWarning(string? message)
    {
        return AddMessage
        (type: Messages.MessageType.AlertWarning, message: message);
    }

    public bool AddAlertError(string? message)
    {
        return AddMessage
       (type: Messages.MessageType.AlertError, message: HandleErrorMessage(message));
    }

    public bool AddToPageErrorFromModeStateErros()
    {
        foreach (var modelStateKey in ModelState.Keys)
        {
            var modelErrors = ModelState[modelStateKey]?.Errors;
            if (modelErrors == null) break;
            foreach (var modelError in modelErrors)
            {
                AddPageError(modelError.ErrorMessage);
            }
        }
        return true;
    }
    public bool AddToAlertErrorFromModeStateErros(string? message)
    {
        foreach (var modelStateKey in ModelState.Keys)
        {
            var modelErrors = ModelState[modelStateKey]?.Errors;
            if (modelErrors == null) break;
            foreach (var modelError in modelErrors)
            {
                AddAlertError(modelError.ErrorMessage);
            }
        }
        return true;
    }
    private string HandleErrorMessage(string? message)
    {
        if (string.IsNullOrEmpty(message))
        {
            return Framework.Resources.ErrorMessages.ErrorOccurred;
        }
        return message;
    }
    private string HandleSuccessMessage(string? message)
    {
        if (string.IsNullOrEmpty(message))
        {
            return Framework.Resources.Messages.SuccessOperation;
        }
        return message;
    }
}



