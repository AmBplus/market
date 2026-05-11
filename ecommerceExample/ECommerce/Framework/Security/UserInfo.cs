using Microsoft.AspNetCore.Http;

namespace Framework.Security;

/// <summary>
/// اطلاعات کاربر جاری - از HttpContext استخراج می‌شود
/// </summary>
public class UserInfo
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserInfo(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    /// <summary>
    /// شناسه کاربر جاری - از Claims استخراج می‌شود
    /// </summary>
    public long UserId
    {
        get
        {
            var claim = _httpContextAccessor.HttpContext?.User?.FindFirst("UserId");
            return claim != null && long.TryParse(claim.Value, out var id) ? id : 0;
        }
    }

    /// <summary>
    /// نام کاربری جاری
    /// </summary>
    public string UserName =>
        _httpContextAccessor.HttpContext?.User?.FindFirst("UserName")?.Value ?? string.Empty;

    /// <summary>
    /// آیا کاربر وارد شده
    /// </summary>
    public bool IsAuthenticated =>
        _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
}
