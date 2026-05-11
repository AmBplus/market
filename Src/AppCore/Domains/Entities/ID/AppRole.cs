using AppCore.Domains.Entities.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppCore.Domains.Entities.ID
{
    public class AppRole : BaseEntity
    {


        public required string Title { get; set; }
        
        public ICollection<AppUserDomainRole>? UserRoles { get; set; }

        public ICollection<UserRolePermission>? Permissions { get; set; }
    }
}
