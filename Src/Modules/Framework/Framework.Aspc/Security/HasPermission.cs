using Infrastructure.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Aspc.Security; 

public interface IAccessValidator
{
    Task<bool> HasPermission(long? permissionNameId);
    Task<bool> HasSubDomainAccess(long domainId);
}


