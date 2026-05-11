using Infrastructure.Helper;
namespace Infrastructure.Security;

public static class SecurityConstant
{
    static SecurityConstant()
    {
    }

    public static class Scheme
    {
        public const string Default =
            AuthenticateScheme.Current;

        static Scheme()
        {
        }
    }

    public static class ClaimKeyName
    {
        public const string Name =
            System.Security.Claims.ClaimTypes.Name;

        public const string Role =
            System.Security.Claims.ClaimTypes.Role;

        public const string UserId = "UserId";
        public const string RoleDomainId = "RoleDomainId";
        public const string RoleTitle = "RoleTitle";
        public const string RoleId = "RoleId";
        public const string DomainRoleTitle = "DomainRoleTitle";
        public const string DomainId = "DomainId";

        //public const string UserId =
        //	System.Security.Claims.ClaimTypes.NameIdentifier;

        public const string UserIP = "UserIP";
        public const string ConfirmedDataForCertificate = "ConfirmedDataForCertificate";
        public const string LastName = "LastName";
        public const string RoleCode = "RoleCode";
        public const string Username = "Username";
        public const string FirstName = "FirstName";
        public const string SessionId = "SessionId";
        public const string EmailAddress = "EmailAddress";
        public const string CellPhoneNumber = "CellPhoneNumber";
        public const string FamilyName = "FamilyName";
        public const string? Id = "Id";
        public static string NactionalCode = "NactionalCode";
        public static string ImageProfile = "ImageProfile";

        static ClaimKeyName()
        {
        }


    }
}