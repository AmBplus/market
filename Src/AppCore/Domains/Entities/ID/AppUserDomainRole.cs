using AppCore.Domains.Entities.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppCore.Domains.Entities.ID
{
    public class AppUserDomainRole :BaseEntity
    {

        public long UserId { get; set; }

        public long RoleId { get; set; }

        public AppUser? User { get; set; }

        public AppRole? Role { get; set; }
    }
}
