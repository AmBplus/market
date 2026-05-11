namespace AppCore.Enums;

public enum AccountType : byte
{
    Asset = 1, Liability = 2, Revenue = 3, Expense = 4, Equity = 5
}

public enum TransactionType : byte
{
    Debit = 1, Credit = 2
}

public enum TransactionSource : byte
{
    Order = 1, Refund = 2, Shipping = 3, VendorCommission = 4, Operational = 5, Manual = 6
}

public enum VendorStatus : byte
{
    Pending = 1, Active = 2, Suspended = 3, Rejected = 4
}
