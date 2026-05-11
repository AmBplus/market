using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Framework.Services.Orm;
using System.Text;
using System.Text.Json;

namespace Web.Pages.Auth.Login
{
    public class IndexModel : PageModel
    {
        private readonly IDapperHelper _dapper;

        public IndexModel(IDapperHelper dapper)
        {
            _dapper = dapper;
        }

        [BindProperty]
        public LoginInputModel Input { get; set; } = new();

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var param = new Dictionary<string, object> { ["@id"] = Input.Id?.Trim() ?? string.Empty };
            var user = await _dapper.QueryFirstOrDefaultAsync<UserInfoDto>("exec [dbo].[sp_GetUserInfo] @id", param);

            if (user == null || user.isSuccess == false)
            {
                ModelState.AddModelError(string.Empty, "کاربر یافت نشد");
                return Page();
            }
            if (string.IsNullOrEmpty(user.hashedPassword) || string.IsNullOrEmpty(Input.Password) || user.hashedPassword != Input.Password)
            {
                ModelState.AddModelError(string.Empty, "نام کاربری یا رمز عبور نادرست است");
                return Page();
            }

            var claims = new List<Claim>();
            static string ToBase64(string? value) => string.IsNullOrEmpty(value) ? string.Empty : Convert.ToBase64String(Encoding.UTF8.GetBytes(value));

            claims.Add(new Claim(ClaimTypes.Name, ToBase64(user.name)));
            claims.Add(new Claim(Web.Services.Authenticate.ClaimConstants.FirstName, ToBase64(user.name)));
            claims.Add(new Claim(Web.Services.Authenticate.ClaimConstants.LastName, ToBase64(user.fname)));
            if (!string.IsNullOrEmpty(user.username))
                claims.Add(new Claim(Web.Services.Authenticate.ClaimConstants.Username, user.username));
            if (!string.IsNullOrEmpty(user.phoneNumber))
                claims.Add(new Claim(Web.Services.Authenticate.ClaimConstants.CellPhoneNumber, user.phoneNumber));
            if (!string.IsNullOrEmpty(user.email))
                claims.Add(new Claim(Web.Services.Authenticate.ClaimConstants.EmailAddress, user.email));

            try
            {
                if (!string.IsNullOrWhiteSpace(user.roles))
                {
                    var roleList = JsonSerializer.Deserialize<List<RoleDto>>(user.roles) ?? new();
                    foreach (var r in roleList)
                    {
                        if (!string.IsNullOrEmpty(r.name))
                            claims.Add(new Claim(Web.Services.Authenticate.ClaimConstants.Role, r.name));
                    }
                }
            }
            catch { }

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(claimsIdentity);
            var authProps = new AuthenticationProperties { IsPersistent = true };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProps);
            return RedirectToPage("/_Client/Home/Index");
        }

        public class LoginInputModel
        {
            [BindProperty]
            public string Id { get; set; } = string.Empty;
            [BindProperty]
            public string Password { get; set; } = string.Empty;
        }

        private class UserInfoDto
        {
            public string? name { get; set; }
            public string? fname { get; set; }
            public string? username { get; set; }
            public string? hashedPassword { get; set; }
            public string? roles { get; set; }
            public bool isEnableTwoFactor { get; set; }
            public string? phoneNumber { get; set; }
            public string? email { get; set; }
            public bool isActive { get; set; }
            public bool isDelete { get; set; }
            public bool isSuccess { get; set; }
            public string? message { get; set; }
        }
        private class RoleDto { public long roleId { get; set; } public string? name { get; set; } }
    }
}
