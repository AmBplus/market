using AppCore.Domains.Entities.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppCore.Domains.Entities.Shop.ProductAggregate
{

    public class ProductTag : BaseEntity
    {
        public required string Name { get; set; }
        public string? Slug { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDelete { get; set; } = false;
    }
}
