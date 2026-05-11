namespace ECommerce.AppCore.Enums;

public enum OrderStatus : byte
{
    Pending = 1, Paid = 2, Processing = 3, Shipped = 4, Delivered = 5, Cancelled = 6, Refunded = 7
}

public enum PaymentStatus : byte
{
    Created = 1, Pending = 2, Succeeded = 3, Failed = 4, Refunded = 5
}

public enum DeliveryStatus : byte
{
    Pending = 1, InTransit = 2, Delivered = 3, Failed = 4
}

public enum DeliveryType : byte
{
    Physical = 1, InPerson = 2, Digital = 3
}

public enum DigitalContentType : byte
{
    Link = 1, File = 2, Content = 3
}
