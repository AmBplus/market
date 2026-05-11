using Framework.Resources;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Infrastructure;



[Controller]
    public class AppBaseController : Controller
    {
   [NonAction]
        public bool AddPageError(string? message)
        {
            return AddMessage
              (type: Messages.MessageType.PageError, message: HandleErrorMessage(message));
        }
    [NonAction]
    public bool AddPageWarning(string? message)
        {
            return AddMessage
              (type: Messages.MessageType.PageWarning, message: message);
        }
    [NonAction]
    public bool AddPageSuccess(string? message)
        {
            return AddMessage
              (type: Messages.MessageType.PageSuccess, message: HandleSuccessMessage(message));
        }
    [NonAction]
    public bool AddToastError(string? message)
        {
            return AddMessage
              (type: Messages.MessageType.ToastError, message: HandleErrorMessage(message));
        }
    [NonAction]
    public bool AddToastWarning(string? message)
        {
            return AddMessage
              (type: Messages.MessageType.ToastWarning, message: message);
        }
    [NonAction]
    public bool AddToastSuccess(string? message)
        {
            return AddMessage
              (type: Messages.MessageType.ToastSuccess, message: HandleSuccessMessage(message));
        }
    [NonAction]
    public bool AddToastInfo(string? message)
        {
            return AddMessage
              (type: Messages.MessageType.ToastInfo, message: message);
        }
    [NonAction]
    public bool AddMessage(Messages.MessageType type, string? message)
        {
            return Messages.Utility.AddMessage
              (tempData: TempData, type: type, message: message);
        }

    [NonAction]
    protected string SetReturnUrl(string? returnUrl)
        {
            if (string.IsNullOrWhiteSpace(value: returnUrl))
            {
                returnUrl = "./Index";
            }

            return returnUrl;
        }
    [NonAction]
    public bool AddAlertSuccess(string? message)
        {
            return AddMessage
            (type: Messages.MessageType.AlertSuccess, message: HandleSuccessMessage(message));
        }
    [NonAction]
    public bool AddAlertWarning(string? message)
        {
            return AddMessage
            (type: Messages.MessageType.AlertWarning, message: message);
        }
    [NonAction]
    public bool AddAlertError(string? message)
        {
            return AddMessage
           (type: Messages.MessageType.AlertError, message: HandleErrorMessage(message));
        }
    [NonAction]
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
    [NonAction]
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



