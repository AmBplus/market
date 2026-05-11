using AppCore.Domains.Entities.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppCore.Domains.Entities.ID
{
    public class PermissionType :BaseEntity
    {
    

        public required string Title { get; set; } // Read, Write, Delete, Update

        public ICollection<Permission>? Permissions { get; set; }
    }

}
