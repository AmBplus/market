using AppCore.Domains.Entities.Base;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppCore.Domains.Entities.ID
{
    public class UserRolePermission :BaseEntity
    {
        public long PermissionId { get; set; }
        public Permission? Permission { get; set; }

        public long? UserId { get; set; }
        public  AppUser? User { get; set; }

        public long? RoleId { get; set; }
        public  AppRole?  Role { get; set; }

        public bool IsAllowed { get; set; } = true;
    }
}
