using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using System.Text;
using System.Linq;
using Infrastructure.Security;

namespace Framework.Aspc.Security;



public class UserInfo 
{
    #region Constructor
    public UserInfo
        (IHttpContextAccessor httpContextAccessor)
    {
        User = httpContextAccessor.HttpContext?.User;
        Identity = httpContextAccessor.HttpContext?.User?.Identity;
    }
    #endregion /Constructor

    #region Properties

    private ClaimsPrincipal? User { get; init; }
    private System.Security.Principal.IIdentity? Identity { get; init; }
    // تابع برای دیکد کردن از Base64
    //string DecodeFromBase64(string? encoded)
    //{
    //    if (string.IsNullOrEmpty(encoded)) return "";
    //    try
    //    {
    //        return Encoding.UTF8.GetString(Convert.FromBase64String(encoded));
    //    }
    //    catch
    //    {
    //        return ""; // در صورت خطا (مثلاً مقدار نامعتبر)
    //    }
    //}
    #endregion /Properties

    #region Read Only Properties

    #region public bool IsAuthenticated { get; }
    public bool IsAuthenticated
    {
        get
        {
            if (Identity is null)
            {
                return false;
            }

            return Identity.IsAuthenticated;
        }
    }
    #endregion /public bool IsAuthenticated { get; }
    #region public bool IsMale { get; }
    private string _fullNameInfo;
    public string FullNameInfo
    {
        get
        {
            if (string.IsNullOrEmpty(_fullNameInfo))
            {

                StringBuilder sb = new StringBuilder();
                if (!string.IsNullOrEmpty(Name) || !string.IsNullOrEmpty(LastName))
                {
                    if (!string.IsNullOrEmpty(Name))
                    {
                        sb.Append(Name);
                        sb.Append(" ");
                    }
                    if (!string.IsNullOrEmpty(LastName))
                    {
                        sb.Append(LastName);

                    }
                }
                else if (!string.IsNullOrEmpty(CellPhoneNumber))
                {
                    sb.Append(CellPhoneNumber);
                }
                else if (!string.IsNullOrEmpty(Username))
                {
                    sb.Append(CellPhoneNumber);
                }
                else
                {
                    sb.Append("----");
                }


                _fullNameInfo = sb.ToString();
            }
            return _fullNameInfo;

        }
    }
    #endregion /public bool FullNameInfo { get; }


    #region public string? Name { get; }
    public string? Name
    {
        get
        {
            var value = GetClaimValue(keyName: SecurityConstant.ClaimKeyName.Name);

            return value;
        }
    }
    #endregion /public string? Name { get; }

    #region public string? Role { get; }
    public List<string>? Role
    {
        get
        {
            var value = GetClaimsValue
                (keyName: SecurityConstant.ClaimKeyName.Role);

            return value;
        }
    }
    #endregion /public string? Role { get; }
    #region public string? RoleTitle { get; }
    public string? RoleTitle
    {
        get
        {
            var value = GetClaimValue
                (keyName: SecurityConstant.ClaimKeyName.RoleTitle);

            return value;
        }
    }
    #endregion /public string? RoleTitle { get; }
    #region public string? RoleDomainTitle { get; }
    public string? RoleDomainTitle
    {
        get
        {
            var value = GetClaimValue
                (keyName: SecurityConstant.ClaimKeyName.DomainRoleTitle);

            return value;
        }
    }
    #endregion /public string? RoleDomainTitle { get; }


    #region public string? LastName { get; }
    public string? LastName
    {
        get
        {
            var value = GetClaimValue(keyName: SecurityConstant.ClaimKeyName.LastName);

            return value;
        }
    }
    #endregion /public string? LastName { get; }

    #region public string? FirstName { get; }
    public string? FirstName
    {
        get
        {
            var value = GetClaimValue(keyName: SecurityConstant.ClaimKeyName.FirstName);

            return value;
        }
    }
    #endregion /public string? FirstName { get; }

    #region public string? UserIP { get; }
    public string? UserIP
    {
        get
        {
            var value = GetClaimValue
                (keyName: SecurityConstant.ClaimKeyName.UserIP);

            return value;
        }
    }
    #endregion /public string? UserIP { get; }

    #region public string? Username { get; }
    public string? Username
    {
        get
        {
            var value = GetClaimValue
                (keyName: SecurityConstant.ClaimKeyName.Username);

            return value;
        }
    }
    #endregion /public string? Username { get; }

    #region public string? EmailAddress { get; }
    public string? EmailAddress
    {
        get
        {
            var value = GetClaimValue
                (keyName: SecurityConstant.ClaimKeyName.EmailAddress);

            return value;
        }
    }
    #endregion /public string? EmailAddress { get; }

    #region public string? CellPhoneNumber { get; }
    public string? CellPhoneNumber
    {
        get
        {
            var value = GetClaimValue
                (keyName: SecurityConstant.ClaimKeyName.CellPhoneNumber);

            return value;
        }
    }
    #endregion /public string? CellPhoneNumber { get; }

    #region public string? NactionalCode { get; }
    public string? NationalCode
    {
        get
        {
            var value = GetClaimValue
                (keyName: SecurityConstant.ClaimKeyName.NactionalCode);

            return value;
        }
    }
    #endregion /public string? NactionalCode { get; }  

    #region public string? ImageProfile { get; }
    public string? ImageProfile
    {
        get
        {
            var value = GetClaimValue
                (keyName: SecurityConstant.ClaimKeyName.ImageProfile);

            return value;
        }
    }

    #endregion /public string? ImageProfile { get; }


    #region public System.Guid? UserId { get; }
    public System.Guid? UserId
    {
        get
        {
            var value = GetClaimValue
                (keyName: SecurityConstant.ClaimKeyName.UserId);

            if (value is null)
            {
                return null;
            }

            try
            {
                var result =
                    new System.Guid(g: value);

                return result;
            }
            catch
            {
                return null;
            }
        }
    }
    #endregion /public System.Guid? UserId { get; }

    #region public System.Guid? SessionId { get; }
    public System.Guid? SessionId
    {
        get
        {
            var value = GetClaimValue
                (keyName: SecurityConstant.ClaimKeyName.SessionId);

            if (value is null)
            {
                return null;
            }

            try
            {
                var result =
                    new System.Guid(g: value);

                return result;
            }
            catch
            {
                return null;
            }
        }
    }
    #endregion /public System.Guid? SessionId { get; }


    #region public int? UserId { get; }
    public long? Id
    {
        get
        {
            var value = GetClaimValue
                (keyName: SecurityConstant.ClaimKeyName.Id);

            if (value is null)
            {
                return null;
            }

            try
            {
                var result =
                    Convert.ToInt64(value);

                return result;
            }
            catch
            {
                return null;
            }
        }
    }
    #endregion /public long UserId { get; }
    #region public long RoleDomainId { get; }
    public long RoleDomainId
    {
        get
        {
            var value = GetClaimValue
                (keyName: SecurityConstant.ClaimKeyName.RoleDomainId);

            if (value is null)
            {
                return 0;
            }

            try
            {
                var result =
                    Convert.ToInt64(value);

                return result;
            }
            catch
            {
                return 0;
            }
        }
    }
    #endregion /public long RoleDomainId { get; } 

    #region public long DomainId { get; }
    public long DomainId
    {
        get
        {
            var value = GetClaimValue
                (keyName: SecurityConstant.ClaimKeyName.DomainId);

            if (value is null)
            {
                return 0;
            }

            try
            {
                var result =
                    Convert.ToInt64(value);

                return result;
            }
            catch
            {
                return 0;
            }
        }
    }
    #endregion /public long DomainId { get; }

    #region public long RoleId { get; }
    public long RoleId
    {
        get
        {
            var value = GetClaimValue
                (keyName: SecurityConstant.ClaimKeyName.RoleId);

            if (value is null)
            {
                return 0;
            }

            try
            {
                var result =
                    Convert.ToInt64(value);

                return result;
            }
            catch
            {
                return 0;
            }
        }
    }
    #endregion /public long RoleId { get; }
    #region public bool? ConfirmedDataForCertificate { get; }
    public bool? ConfirmedDataForCertificate
    {
        get
        {
            var value = GetClaimValue
                (keyName: SecurityConstant.ClaimKeyName.ConfirmedDataForCertificate);

            if (value is null)
            {
                return null;
            }

            try
            {
                var result =
                    Convert.ToBoolean(value);

                return result;
            }
            catch
            {
                return null;
            }
        }
    }
    #endregion /public System.bool? ConfirmedDataForCertificate { get; }

    #endregion /Read Only Properties

    #region Methods

    #region GetClaimValue()
    private string? GetClaimValue(string? keyName)
    {
        if (string.IsNullOrWhiteSpace(value: keyName))
        {
            return null;
        }

        if (User is null)
        {
            return null;
        }

        // نیازی به نوشتن دستور ذیل نیست

        var claim =
            User.Claims
            .Where(current => current.Type.ToLower() == keyName.ToLower())
            .FirstOrDefault();

        if (claim is null)
        {
            return null;
        }

        var value =
            claim.Value;

        if (string.IsNullOrWhiteSpace(value: value))
        {
            return null;
        }

        var result =
            value.Trim();

        return result;
    }
    #endregion /GetClaimValue()


    #region GetClaimsValue()
    private List<string>? GetClaimsValue(string? keyName)
    {
        if (string.IsNullOrWhiteSpace(value: keyName))
        {
            return null;
        }

        if (User is null)
        {
            return null;
        }

        // نیازی به نوشتن دستور ذیل نیست

        var claim =
            User.Claims
            .Where(current => current.Type.ToLower() == keyName.ToLower())
            .ToList();

        if (claim is null)
        {
            return null;
        }

        var value =
            claim.Select(x => x.Value.Trim());


        return value.ToList();
    }
    #endregion /GetClaimsValue()
    #endregion /Methods
}
