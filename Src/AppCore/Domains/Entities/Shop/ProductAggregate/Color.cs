using AppCore.Domains.Entities.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppCore.Domains.Entities.Shop.ProductAgg
{
    public class Color : BaseEntity
    {
        public required string Title { get; set; }
        public required string ColorCode { get; set; }

        // Lifecycle
        public long CreateBy { get; set; }
        public DateTime CreateAt { get; set; }
        
        public long UpdateBy { get; set; }
        public DateTime UpdateAt { get; set; }
        public bool IsUpdate { get; set; }

        // Soft-delete flags (consistent with AppUser, ProductBrand patterns)
        public bool IsDelete { get; set; } = false;
        public bool IsActive { get; set; } = true;
    }
}
