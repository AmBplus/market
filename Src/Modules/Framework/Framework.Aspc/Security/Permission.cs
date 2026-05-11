using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Infrastructure.Security;

namespace Framework.Aspc.Security
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class PermissionAttribute : AuthorizeAttribute, IAsyncAuthorizationFilter
    {
        private long? PermissionId { get; }
        public PermissionAttribute( long permissionNameId)
        {
            PermissionId = permissionNameId;
        }  
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            // دریافت سرویس‌های مورد نیاز
            var serviceProvider = context.HttpContext.RequestServices;
            var logger = serviceProvider.GetService<ILogger<PermissionAttribute>>();
            var userInfo = serviceProvider.GetService<UserInfo>();
            var accessValidator = serviceProvider.GetService<IAccessValidator>();
            if (userInfo == null) {
                throw new ArgumentNullException(nameof(userInfo));
            }
            // بررسی وجود احراز هویت
            var user = context.HttpContext.User;
            if (!userInfo.IsAuthenticated)
            {
                logger?.LogWarning("Unauthorized access attempt");
                context.Result = new UnauthorizedResult();
                return;
            }
            if(accessValidator == null)
            {
                throw new ArgumentNullException(nameof(accessValidator));
            }
            var hasPermission =  await accessValidator.HasPermission(PermissionId);
            if (!hasPermission) {
                logger?.LogWarning("Unauthorized access attempt");
                context.Result = new ForbidResult();
                return;
            }
             await Task.CompletedTask;
        }
    }
}
