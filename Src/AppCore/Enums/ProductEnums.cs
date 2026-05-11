namespace AppCore.Enums;

public enum PriceType : byte
{
    Retail = 1, Wholesale = 2, Special = 3, Cost = 4
}

public enum StockChangeReason : byte
{
    StockIn = 1, StockOut = 2, Return = 3, Damaged = 4,
    Initial = 5, ManualAdjustment = 6, Reserved = 7, ReservationReleased = 8
}

public enum DiscountType : byte
{
    Percentage = 1, FixedAmount = 2, FreeShipping = 3
}

public enum DiscountScope : byte
{
    Order = 1, Product = 2, Category = 3, Shipping = 4
}

public enum ShippingConstraintType : byte
{
    Fragile = 1, Heavy = 2, Oversized = 3, Hazardous = 4, Perishable = 5
}
