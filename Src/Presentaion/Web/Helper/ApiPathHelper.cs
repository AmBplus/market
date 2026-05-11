namespace Web.Helper
{
    public class ApiPathHelper
    {
        public class Admin
        {
            public const string BaseAdmin = "api/admin";
            public class ID
            {
                public const string BaseID = $"{BaseAdmin}/id";
                public class User
                {
                    public const string BaseUser = $"{BaseID}/user" ;
                    public const string GetUserByID = "get_user_by_id";
                    public const string GetAll = "GetAll";
                    public const string Delete = "Delete";
                }
            }
        }
    }
}
