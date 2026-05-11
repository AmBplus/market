using Framework.ResultHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Security;

public interface ISignInApp<TUser>
{
    public  Task<ResultOperation> SignInUser(TUser user , AddtionalClaimSignIn addtionalClaimSignIn );
}

public class AddtionalClaimSignIn
{
    public bool IsPersistent = false;
    public bool isTemporaryLogin = false;
    public PreferredRole? PreferredRole {  get; set; }   
}
public class PreferredRole
{
    public long RoleDomainId { get; set; }
}