using Framework.ResultHelper;

namespace Web.Feature.Auth.Service
{
    public interface ILoginService
    {
        public ResultOperation Signin(string username, string password);
    }

}
