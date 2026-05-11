using AppCore.Domains.Entities.Base;

namespace AppCore.Domains.Entities.Shop.ProductAggregate;

public class Guarantee : BaseEntity
{
    public required string Title { get; set; }
    
    public bool IsDelete { get;  set; } = false;
    public bool IsActive { get; set; } = true;
    public bool IsUpdate { get; set; }
    
    public DateTime UpdateAt { get;  set; }
    public DateTime CreateAt => CreatedAt;
    
    public long? UpdateBy { get;  set; }
}
