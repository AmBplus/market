namespace Web.Helper;

public class ApiPathHelper
{
    public class Admin
    {
        public const string BaseAdmin = "api/admin";

        public class Identity
        {
            public const string BaseIdentity = $"{BaseAdmin}/identity";
            public class User
            {
                public const string BaseUser = $"{BaseIdentity}/user";
                public const string GetById = "get-by-id";
                public const string GetAll = "get-all";
                public const string Create = "create";
                public const string Update = "update";
                public const string Delete = "delete";
                public const string ChangePassword = "change-password";
                public const string Select2 = "select2";
            }
            public class Role
            {
                public const string BaseRole = $"{BaseIdentity}/role";
                public const string GetById = "get-by-id";
                public const string GetAll = "get-all";
                public const string Create = "create";
                public const string Update = "update";
                public const string Delete = "delete";
                public const string Select2 = "select2";
            }
        }

        public class Product
        {
            public const string BaseProduct = $"{BaseAdmin}/product";
            public class Products
            {
                public const string Base = $"{BaseProduct}/products";
                public const string GetById = "get-by-id";
                public const string GetAll = "get-all";
                public const string Create = "create";
                public const string Update = "update";
                public const string Delete = "delete";
                public const string Select2 = "select2";
            }
            public class ProductVariants
            {
                public const string Base = $"{BaseProduct}/product-variants";
                public const string GetById = "get-by-id";
                public const string GetAll = "get-all";
                public const string Create = "create";
                public const string Update = "update";
                public const string Delete = "delete";
                public const string Select2 = "select2";
            }
            public class Categories
            {
                public const string Base = $"{BaseProduct}/categories";
                public const string GetById = "get-by-id";
                public const string GetAll = "get-all";
                public const string Create = "create";
                public const string Update = "update";
                public const string Delete = "delete";
                public const string Select2 = "select2";
            }
            public class Brands
            {
                public const string Base = $"{BaseProduct}/brands";
                public const string GetById = "get-by-id";
                public const string GetAll = "get-all";
                public const string Create = "create";
                public const string Update = "update";
                public const string Delete = "delete";
                public const string Select2 = "select2";
            }
        }

        public class Pricing
        {
            public const string BasePricing = $"{BaseAdmin}/pricing";
            public class Currencies
            {
                public const string Base = $"{BasePricing}/currencies";
                public const string GetById = "get-by-id";
                public const string GetAll = "get-all";
                public const string Create = "create";
                public const string Update = "update";
                public const string Select2 = "select2";
            }
            public class Prices
            {
                public const string Base = $"{BasePricing}/prices";
                public const string Create = "create";
                public const string BulkUpdate = "bulk-update";
                public const string GetByVariantId = "get-by-variant-id";
                public const string GetHistory = "get-history";
            }
        }

        public class Inventory
        {
            public const string BaseInventory = $"{BaseAdmin}/inventory";
            public class Stock
            {
                public const string Base = $"{BaseInventory}/stock";
                public const string StockIn = "stock-in";
                public const string StockOut = "stock-out";
                public const string Adjust = "adjust";
                public const string Initial = "initial";
                public const string GetCurrent = "get-current";
                public const string GetHistory = "get-history";
            }
            public class Warehouses
            {
                public const string Base = $"{BaseInventory}/warehouses";
                public const string GetById = "get-by-id";
                public const string GetAll = "get-all";
                public const string Create = "create";
                public const string Update = "update";
                public const string Select2 = "select2";
            }
        }

        public class Order
        {
            public const string BaseOrder = $"{BaseAdmin}/order";
            public class Carts
            {
                public const string Base = $"{BaseOrder}/carts";
                public const string AddItem = "add-item";
                public const string RemoveItem = "remove-item";
                public const string UpdateQuantity = "update-quantity";
                public const string ApplyDiscount = "apply-discount";
                public const string GetActive = "get-active";
            }
            public class Orders
            {
                public const string Base = $"{BaseOrder}/orders";
                public const string GetById = "get-by-id";
                public const string GetAll = "get-all";
                public const string Create = "create";
                public const string Cancel = "cancel";
                public const string UpdateStatus = "update-status";
            }
        }

        public class Discount
        {
            public const string BaseDiscount = $"{BaseAdmin}/discount";
            public class Discounts
            {
                public const string Base = $"{BaseDiscount}/discounts";
                public const string GetById = "get-by-id";
                public const string GetAll = "get-all";
                public const string Create = "create";
                public const string Update = "update";
                public const string Delete = "delete";
                public const string ValidateCode = "validate-code";
            }
        }

        public class Payment
        {
            public const string BasePayment = $"{BaseAdmin}/payment";
            public class Payments
            {
                public const string Base = $"{BasePayment}/payments";
                public const string GetById = "get-by-id";
                public const string GetByOrderId = "get-by-order-id";
                public const string Create = "create";
                public const string Verify = "verify";
                public const string Refund = "refund";
            }
            public class Gateways
            {
                public const string Base = $"{BasePayment}/gateways";
                public const string GetAll = "get-all";
                public const string GetActive = "get-active";
                public const string Create = "create";
                public const string Update = "update";
            }
        }

        public class Shipping
        {
            public const string BaseShipping = $"{BaseAdmin}/shipping";
            public class Carriers
            {
                public const string Base = $"{BaseShipping}/carriers";
                public const string GetAll = "get-all";
                public const string Create = "create";
                public const string Update = "update";
                public const string Select2 = "select2";
            }
            public class DeliveryMethods
            {
                public const string Base = $"{BaseShipping}/delivery-methods";
                public const string GetAll = "get-all";
                public const string Create = "create";
                public const string Update = "update";
                public const string Select2 = "select2";
            }
            public class OrderDeliveries
            {
                public const string Base = $"{BaseShipping}/order-deliveries";
                public const string GetByOrderId = "get-by-order-id";
                public const string UpdateStatus = "update-status";
                public const string SetTrackingCode = "set-tracking-code";
            }
        }

        public class Accounting
        {
            public const string BaseAccounting = $"{BaseAdmin}/accounting";
            public class Accounts
            {
                public const string Base = $"{BaseAccounting}/accounts";
                public const string GetById = "get-by-id";
                public const string GetAll = "get-all";
                public const string Create = "create";
                public const string Update = "update";
                public const string Select2 = "select2";
            }
            public class Transactions
            {
                public const string Base = $"{BaseAccounting}/transactions";
                public const string CreateBatch = "create-batch";
                public const string GetByAccountId = "get-by-account-id";
                public const string GetByBatchId = "get-by-batch-id";
            }
        }

        public class Vendor
        {
            public const string BaseVendor = $"{BaseAdmin}/vendor";
            public class Vendors
            {
                public const string Base = $"{BaseVendor}/vendors";
                public const string GetById = "get-by-id";
                public const string GetAll = "get-all";
                public const string Create = "create";
                public const string Update = "update";
                public const string Approve = "approve";
                public const string Suspend = "suspend";
                public const string Select2 = "select2";
            }
        }

        public class Settings
        {
            public const string BaseSettings = $"{BaseAdmin}/settings";
            public class AppSettings
            {
                public const string Base = $"{BaseSettings}/settings";
                public const string GetByKey = "get-by-key";
                public const string GetByModule = "get-by-module";
                public const string GetAll = "get-all";
                public const string Update = "update";
                public const string BulkUpdate = "bulk-update";
            }
            public class EnumValues
            {
                public const string Base = $"{BaseSettings}/enum-values";
                public const string GetByType = "get-by-type";
                public const string Create = "create";
                public const string Update = "update";
            }
        }

        public class Customization
        {
            public const string BaseCustomization = $"{BaseAdmin}/customization";
            public class Themes
            {
                public const string Base = $"{BaseCustomization}/themes";
                public const string GetActive = "get-active";
                public const string GetAll = "get-all";
                public const string Create = "create";
                public const string Update = "update";
                public const string Activate = "activate";
            }
            public class Sliders
            {
                public const string Base = $"{BaseCustomization}/sliders";
                public const string GetAll = "get-all";
                public const string GetActive = "get-active";
                public const string Create = "create";
                public const string Update = "update";
                public const string Delete = "delete";
            }
            public class Widgets
            {
                public const string Base = $"{BaseCustomization}/widgets";
                public const string GetAll = "get-all";
                public const string GetByPosition = "get-by-position";
                public const string Create = "create";
                public const string Update = "update";
                public const string Delete = "delete";
            }
        }
    }
}
