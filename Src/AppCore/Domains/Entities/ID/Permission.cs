using AppCore.Domains.Entities.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppCore.Domains.Entities.ID
{
    public class Permission : BaseEntity
    {

        public required string Title { get; set; }

        public long PermissionTypeId { get; set; }
        public PermissionType? PermissionType { get; set; }

        public ICollection<UserRolePermission>? Assignments { get; set; }
    }

}
