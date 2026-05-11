-- ============================================================================
-- ECommerceDb - Complete SQL Server Script
-- Idempotent: Uses IF NOT EXISTS checks
-- Schemas: 12 | Tables: 27 | Seed Data included
-- ============================================================================

-- ============================================================================
-- 1. CREATE DATABASE
-- ============================================================================
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'ECommerceDb')
BEGIN
    CREATE DATABASE ECommerceDb;
END
GO

USE ECommerceDb;
GO

-- ============================================================================
-- 2. CREATE SCHEMAS
-- ============================================================================
IF NOT EXISTS (SELECT name FROM sys.schemas WHERE name = 'Identity')
    EXEC('CREATE SCHEMA Identity');
GO

IF NOT EXISTS (SELECT name FROM sys.schemas WHERE name = 'Product')
    EXEC('CREATE SCHEMA Product');
GO

IF NOT EXISTS (SELECT name FROM sys.schemas WHERE name = 'Pricing')
    EXEC('CREATE SCHEMA Pricing');
GO

IF NOT EXISTS (SELECT name FROM sys.schemas WHERE name = 'Inventory')
    EXEC('CREATE SCHEMA Inventory');
GO

IF NOT EXISTS (SELECT name FROM sys.schemas WHERE name = 'Order')
    EXEC('CREATE SCHEMA [Order]');
GO

IF NOT EXISTS (SELECT name FROM sys.schemas WHERE name = 'Discount')
    EXEC('CREATE SCHEMA Discount');
GO

IF NOT EXISTS (SELECT name FROM sys.schemas WHERE name = 'Payment')
    EXEC('CREATE SCHEMA Payment');
GO

IF NOT EXISTS (SELECT name FROM sys.schemas WHERE name = 'Shipping')
    EXEC('CREATE SCHEMA Shipping');
GO

IF NOT EXISTS (SELECT name FROM sys.schemas WHERE name = 'Accounting')
    EXEC('CREATE SCHEMA Accounting');
GO

IF NOT EXISTS (SELECT name FROM sys.schemas WHERE name = 'Vendor')
    EXEC('CREATE SCHEMA Vendor');
GO

IF NOT EXISTS (SELECT name FROM sys.schemas WHERE name = 'Settings')
    EXEC('CREATE SCHEMA Settings');
GO

IF NOT EXISTS (SELECT name FROM sys.schemas WHERE name = 'Customization')
    EXEC('CREATE SCHEMA Customization');
GO

-- ============================================================================
-- 3. CREATE TABLES
-- ============================================================================

-- ── Identity Schema ──────────────────────────────────────────────────────────

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Identity' AND TABLE_NAME = 'Roles')
CREATE TABLE Identity.Roles (
    Id              BIGINT IDENTITY(1,1) PRIMARY KEY,
    Name            NVARCHAR(100)    NOT NULL,
    DisplayName     NVARCHAR(100)    NOT NULL,
    [Description]   NVARCHAR(500)    NULL,
    IsActive        BIT              NOT NULL CONSTRAINT DF_Roles_IsActive DEFAULT 1,
    CreatedAt       DATETIME2        NOT NULL,
    CreatedBy       BIGINT           NULL,
    UpdatedAt       DATETIME2        NULL,
    UpdatedBy       BIGINT           NULL
);
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Identity' AND TABLE_NAME = 'Users')
CREATE TABLE Identity.Users (
    Id                          BIGINT IDENTITY(1,1) PRIMARY KEY,
    UserName                    NVARCHAR(100)    NOT NULL,
    Email                       NVARCHAR(200)    NULL,
    PhoneNumber                 NVARCHAR(20)     NULL,
    PasswordHash                NVARCHAR(500)    NOT NULL,
    PasswordSalt                NVARCHAR(500)    NOT NULL,
    FirstName                   NVARCHAR(100)    NOT NULL,
    LastName                    NVARCHAR(100)    NOT NULL,
    NationalCode                NVARCHAR(10)     NULL,
    DefaultAddressesJson        NVARCHAR(MAX)    NULL,
    IsActive                    BIT              NOT NULL CONSTRAINT DF_Users_IsActive DEFAULT 1,
    IsEmailConfirmed            BIT              NOT NULL CONSTRAINT DF_Users_IsEmailConfirmed DEFAULT 0,
    IsPhoneNumberConfirmed      BIT              NOT NULL CONSTRAINT DF_Users_IsPhoneNumberConfirmed DEFAULT 0,
    LastLoginAt                 DATETIME2        NULL,
    ResetPasswordToken          NVARCHAR(MAX)    NULL,
    ResetPasswordTokenExpireAt  DATETIME2        NULL,
    IsDeleted                   BIT              NOT NULL CONSTRAINT DF_Users_IsDeleted DEFAULT 0,
    DeletedAt                   DATETIME2        NULL,
    DeletedBy                   BIGINT           NULL,
    CreatedAt                   DATETIME2        NOT NULL,
    CreatedBy                   BIGINT           NULL,
    UpdatedAt                   DATETIME2        NULL,
    UpdatedBy                   BIGINT           NULL
);
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Identity' AND TABLE_NAME = 'UserRoles')
CREATE TABLE Identity.UserRoles (
    Id          BIGINT IDENTITY(1,1) PRIMARY KEY,
    UserId      BIGINT           NOT NULL CONSTRAINT FK_UserRoles_UserId FOREIGN KEY REFERENCES Identity.Users(Id) ON DELETE CASCADE,
    RoleId      BIGINT           NOT NULL CONSTRAINT FK_UserRoles_RoleId FOREIGN KEY REFERENCES Identity.Roles(Id) ON DELETE CASCADE,
    AssignedAt  DATETIME2        NOT NULL CONSTRAINT DF_UserRoles_AssignedAt DEFAULT GETUTCDATE()
);
GO

-- ── Product Schema ───────────────────────────────────────────────────────────

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Product' AND TABLE_NAME = 'Brands')
CREATE TABLE Product.Brands (
    Id              BIGINT IDENTITY(1,1) PRIMARY KEY,
    Name            NVARCHAR(200)    NOT NULL,
    Slug            NVARCHAR(200)    NOT NULL,
    LogoUrl         NVARCHAR(500)    NULL,
    [Description]   NVARCHAR(2000)   NULL,
    IsActive        BIT              NOT NULL CONSTRAINT DF_Brands_IsActive DEFAULT 1,
    CreatedAt       DATETIME2        NOT NULL,
    CreatedBy       BIGINT           NULL,
    UpdatedAt       DATETIME2        NULL,
    UpdatedBy       BIGINT           NULL
);
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Product' AND TABLE_NAME = 'Categories')
CREATE TABLE Product.Categories (
    Id              BIGINT IDENTITY(1,1) PRIMARY KEY,
    ParentId        BIGINT           NULL CONSTRAINT FK_Categories_ParentId FOREIGN KEY REFERENCES Product.Categories(Id) ON DELETE RESTRICT,
    Code            NVARCHAR(50)     NOT NULL,
    Name            NVARCHAR(200)    NOT NULL,
    Slug            NVARCHAR(200)    NOT NULL,
    [Description]   NVARCHAR(2000)   NULL,
    ImageUrl        NVARCHAR(500)    NULL,
    DisplayOrder    INT              NOT NULL CONSTRAINT DF_Categories_DisplayOrder DEFAULT 0,
    IsActive        BIT              NOT NULL CONSTRAINT DF_Categories_IsActive DEFAULT 1,
    CreatedAt       DATETIME2        NOT NULL,
    CreatedBy       BIGINT           NULL,
    UpdatedAt       DATETIME2        NULL,
    UpdatedBy       BIGINT           NULL
);
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Product' AND TABLE_NAME = 'Products')
CREATE TABLE Product.Products (
    Id                      BIGINT IDENTITY(1,1) PRIMARY KEY,
    ProductCode             NVARCHAR(50)     NOT NULL,
    Name                    NVARCHAR(300)    NOT NULL,
    Slug                    NVARCHAR(300)    NOT NULL,
    [Description]           NVARCHAR(MAX)    NULL,
    ShortDescription        NVARCHAR(500)    NULL,
    BrandId                 BIGINT           NULL CONSTRAINT FK_Products_BrandId FOREIGN KEY REFERENCES Product.Brands(Id) ON DELETE SET NULL,
    VendorId                BIGINT           NULL,
    ShippingConstraintsJson NVARCHAR(MAX)    NULL,
    MetaTitle               NVARCHAR(200)    NULL,
    MetaDescription         NVARCHAR(500)    NULL,
    IsActive                BIT              NOT NULL CONSTRAINT DF_Products_IsActive DEFAULT 1,
    IsFeatured              BIT              NOT NULL CONSTRAINT DF_Products_IsFeatured DEFAULT 0,
    IsDeleted               BIT              NOT NULL CONSTRAINT DF_Products_IsDeleted DEFAULT 0,
    DeletedAt               DATETIME2        NULL,
    DeletedBy               BIGINT           NULL,
    CreatedAt               DATETIME2        NOT NULL,
    CreatedBy               BIGINT           NULL,
    UpdatedAt               DATETIME2        NULL,
    UpdatedBy               BIGINT           NULL
);
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Product' AND TABLE_NAME = 'ProductVariants')
CREATE TABLE Product.ProductVariants (
    Id                      BIGINT IDENTITY(1,1) PRIMARY KEY,
    ProductId               BIGINT           NOT NULL CONSTRAINT FK_ProductVariants_ProductId FOREIGN KEY REFERENCES Product.Products(Id) ON DELETE CASCADE,
    VariantCode             NVARCHAR(50)     NOT NULL,
    Sku                     NVARCHAR(50)     NOT NULL,
    Title                   NVARCHAR(300)    NOT NULL,
    VariantAttributesJson   NVARCHAR(MAX)    NULL,
    VendorId                BIGINT           NULL,
    CostPrice               DECIMAL(18,2)    NULL,
    DefaultPrice            DECIMAL(18,2)    NOT NULL,
    CurrencyId              BIGINT           NOT NULL,
    WeightGrams             DECIMAL(10,2)    NULL,
    IsActive                BIT              NOT NULL CONSTRAINT DF_ProductVariants_IsActive DEFAULT 1,
    IsDefault               BIT              NOT NULL CONSTRAINT DF_ProductVariants_IsDefault DEFAULT 0,
    IsDeleted               BIT              NOT NULL CONSTRAINT DF_ProductVariants_IsDeleted DEFAULT 0,
    DeletedAt               DATETIME2        NULL,
    DeletedBy               BIGINT           NULL,
    CreatedAt               DATETIME2        NOT NULL,
    CreatedBy               BIGINT           NULL,
    UpdatedAt               DATETIME2        NULL,
    UpdatedBy               BIGINT           NULL
);
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Product' AND TABLE_NAME = 'ProductCategories')
CREATE TABLE Product.ProductCategories (
    Id              BIGINT IDENTITY(1,1) PRIMARY KEY,
    ProductId       BIGINT           NOT NULL CONSTRAINT FK_ProductCategories_ProductId FOREIGN KEY REFERENCES Product.Products(Id) ON DELETE CASCADE,
    CategoryId      BIGINT           NOT NULL CONSTRAINT FK_ProductCategories_CategoryId FOREIGN KEY REFERENCES Product.Categories(Id) ON DELETE CASCADE,
    DisplayOrder    INT              NOT NULL CONSTRAINT DF_ProductCategories_DisplayOrder DEFAULT 0
);
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Product' AND TABLE_NAME = 'ProductAttributes')
CREATE TABLE Product.ProductAttributes (
    Id              BIGINT IDENTITY(1,1) PRIMARY KEY,
    ProductId       BIGINT           NOT NULL CONSTRAINT FK_ProductAttributes_ProductId FOREIGN KEY REFERENCES Product.Products(Id) ON DELETE CASCADE,
    [Key]           NVARCHAR(200)    NOT NULL,
    [Value]         NVARCHAR(500)    NOT NULL,
    [Group]         NVARCHAR(100)    NULL,
    DisplayOrder    INT              NOT NULL CONSTRAINT DF_ProductAttributes_DisplayOrder DEFAULT 0
);
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Product' AND TABLE_NAME = 'ProductImages')
CREATE TABLE Product.ProductImages (
    Id                  BIGINT IDENTITY(1,1) PRIMARY KEY,
    ProductId           BIGINT           NOT NULL CONSTRAINT FK_ProductImages_ProductId FOREIGN KEY REFERENCES Product.Products(Id) ON DELETE CASCADE,
    ProductVariantId    BIGINT           NULL CONSTRAINT FK_ProductImages_ProductVariantId FOREIGN KEY REFERENCES Product.ProductVariants(Id) ON DELETE CASCADE,
    ImageUrl            NVARCHAR(500)    NOT NULL,
    AltText             NVARCHAR(200)    NULL,
    DisplayOrder        INT              NOT NULL CONSTRAINT DF_ProductImages_DisplayOrder DEFAULT 0,
    IsPrimary           BIT              NOT NULL CONSTRAINT DF_ProductImages_IsPrimary DEFAULT 0
);
GO

-- ── Pricing Schema ───────────────────────────────────────────────────────────

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Pricing' AND TABLE_NAME = 'Currencies')
CREATE TABLE Pricing.Currencies (
    Id              BIGINT IDENTITY(1,1) PRIMARY KEY,
    Code            NVARCHAR(10)     NOT NULL,
    Name            NVARCHAR(100)    NULL,
    Symbol          NVARCHAR(10)     NULL,
    ExchangeRate    DECIMAL(18,6)    NOT NULL CONSTRAINT DF_Currencies_ExchangeRate DEFAULT 1.0,
    IsBase          BIT              NOT NULL CONSTRAINT DF_Currencies_IsBase DEFAULT 0,
    IsActive        BIT              NOT NULL CONSTRAINT DF_Currencies_IsActive DEFAULT 1,
    CreatedAt       DATETIME2        NOT NULL,
    CreatedBy       BIGINT           NULL,
    UpdatedAt       DATETIME2        NULL,
    UpdatedBy       BIGINT           NULL
);
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Pricing' AND TABLE_NAME = 'Prices')
CREATE TABLE Pricing.Prices (
    Id                  BIGINT IDENTITY(1,1) PRIMARY KEY,
    ProductVariantId    BIGINT           NOT NULL CONSTRAINT FK_Prices_ProductVariantId FOREIGN KEY REFERENCES Product.ProductVariants(Id) ON DELETE CASCADE,
    Amount              DECIMAL(18,2)    NOT NULL,
    CurrencyId          BIGINT           NOT NULL CONSTRAINT FK_Prices_CurrencyId FOREIGN KEY REFERENCES Pricing.Currencies(Id),
    PriceType           TINYINT          NOT NULL,
    StartedAt           DATETIME2        NOT NULL,
    EndedAt             DATETIME2        NULL,
    MinQuantity         INT              NULL,
    CreatedAt           DATETIME2        NOT NULL,
    CreatedBy           BIGINT           NULL,
    UpdatedAt           DATETIME2        NULL,
    UpdatedBy           BIGINT           NULL
);
GO

-- ── Inventory Schema ─────────────────────────────────────────────────────────

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Inventory' AND TABLE_NAME = 'Warehouses')
CREATE TABLE Inventory.Warehouses (
    Id          BIGINT IDENTITY(1,1) PRIMARY KEY,
    Name        NVARCHAR(200)    NOT NULL,
    Code        NVARCHAR(50)     NOT NULL,
    [Address]   NVARCHAR(500)    NULL,
    City        NVARCHAR(100)    NULL,
    Province    NVARCHAR(100)    NULL,
    IsActive    BIT              NOT NULL CONSTRAINT DF_Warehouses_IsActive DEFAULT 1,
    CreatedAt   DATETIME2        NOT NULL,
    CreatedBy   BIGINT           NULL,
    UpdatedAt   DATETIME2        NULL,
    UpdatedBy   BIGINT           NULL
);
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Inventory' AND TABLE_NAME = 'StockLedgers')
CREATE TABLE Inventory.StockLedgers (
    Id                  BIGINT IDENTITY(1,1) PRIMARY KEY,
    ProductVariantId    BIGINT           NOT NULL CONSTRAINT FK_StockLedgers_ProductVariantId FOREIGN KEY REFERENCES Product.ProductVariants(Id),
    WarehouseId         BIGINT           NOT NULL CONSTRAINT FK_StockLedgers_WarehouseId FOREIGN KEY REFERENCES Inventory.Warehouses(Id),
    QuantityChange      INT              NOT NULL,
    Reason              TINYINT          NOT NULL,
    ReferenceId         BIGINT           NULL,
    [Description]       NVARCHAR(500)    NULL,
    CreatedAt           DATETIME2        NOT NULL CONSTRAINT DF_StockLedgers_CreatedAt DEFAULT GETUTCDATE(),
    CreatedBy           BIGINT           NULL
);
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Inventory' AND TABLE_NAME = 'StockReservations')
CREATE TABLE Inventory.StockReservations (
    Id                  BIGINT IDENTITY(1,1) PRIMARY KEY,
    ProductVariantId    BIGINT           NOT NULL CONSTRAINT FK_StockReservations_ProductVariantId FOREIGN KEY REFERENCES Product.ProductVariants(Id),
    WarehouseId         BIGINT           NOT NULL CONSTRAINT FK_StockReservations_WarehouseId FOREIGN KEY REFERENCES Inventory.Warehouses(Id),
    OrderId             BIGINT           NOT NULL,
    Quantity            INT              NOT NULL,
    CreatedAt           DATETIME2        NOT NULL CONSTRAINT DF_StockReservations_CreatedAt DEFAULT GETUTCDATE(),
    ExpiresAt           DATETIME2        NOT NULL,
    IsReleased          BIT              NOT NULL CONSTRAINT DF_StockReservations_IsReleased DEFAULT 0,
    ReleasedAt          DATETIME2        NULL
);
GO

-- ── Order Schema ─────────────────────────────────────────────────────────────

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Order' AND TABLE_NAME = 'Carts')
CREATE TABLE [Order].Carts (
    Id              BIGINT IDENTITY(1,1) PRIMARY KEY,
    UserId          BIGINT           NOT NULL CONSTRAINT FK_Carts_UserId FOREIGN KEY REFERENCES Identity.Users(Id),
    DiscountCode    NVARCHAR(50)     NULL,
    IsActive        BIT              NOT NULL CONSTRAINT DF_Carts_IsActive DEFAULT 1,
    CreatedAt       DATETIME2        NOT NULL,
    CreatedBy       BIGINT           NULL,
    UpdatedAt       DATETIME2        NULL,
    UpdatedBy       BIGINT           NULL
);
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Order' AND TABLE_NAME = 'CartItems')
CREATE TABLE [Order].CartItems (
    Id                  BIGINT IDENTITY(1,1) PRIMARY KEY,
    CartId              BIGINT           NOT NULL CONSTRAINT FK_CartItems_CartId FOREIGN KEY REFERENCES [Order].Carts(Id) ON DELETE CASCADE,
    ProductVariantId    BIGINT           NOT NULL CONSTRAINT FK_CartItems_ProductVariantId FOREIGN KEY REFERENCES Product.ProductVariants(Id),
    Quantity            INT              NOT NULL,
    UnitPrice           DECIMAL(18,2)    NOT NULL,
    CurrencyId          BIGINT           NOT NULL CONSTRAINT FK_CartItems_CurrencyId FOREIGN KEY REFERENCES Pricing.Currencies(Id),
    AddedAt             DATETIME2        NOT NULL CONSTRAINT DF_CartItems_AddedAt DEFAULT GETUTCDATE()
);
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Order' AND TABLE_NAME = 'Orders')
CREATE TABLE [Order].Orders (
    Id                  BIGINT IDENTITY(1,1) PRIMARY KEY,
    OrderNumber         NVARCHAR(50)     NOT NULL,
    UserId              BIGINT           NOT NULL CONSTRAINT FK_Orders_UserId FOREIGN KEY REFERENCES Identity.Users(Id),
    Status              TINYINT          NOT NULL,
    SubTotal            DECIMAL(18,2)    NOT NULL,
    DiscountAmount      DECIMAL(18,2)    NOT NULL,
    ShippingCost        DECIMAL(18,2)    NOT NULL,
    TaxAmount           DECIMAL(18,2)    NOT NULL,
    TotalAmount         DECIMAL(18,2)    NOT NULL,
    CurrencyId          BIGINT           NOT NULL CONSTRAINT FK_Orders_CurrencyId FOREIGN KEY REFERENCES Pricing.Currencies(Id),
    CurrencyRate        DECIMAL(18,6)    NOT NULL CONSTRAINT DF_Orders_CurrencyRate DEFAULT 1.0,
    CustomerNote        NVARCHAR(500)    NULL,
    AdminNote           NVARCHAR(500)    NULL,
    ShippingAddressJson NVARCHAR(MAX)    NOT NULL,
    CompletedAt         DATETIME2        NULL,
    CreatedAt           DATETIME2        NOT NULL,
    CreatedBy           BIGINT           NULL,
    UpdatedAt           DATETIME2        NULL,
    UpdatedBy           BIGINT           NULL
);
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Order' AND TABLE_NAME = 'OrderItems')
CREATE TABLE [Order].OrderItems (
    Id                      BIGINT IDENTITY(1,1) PRIMARY KEY,
    OrderId                 BIGINT           NOT NULL CONSTRAINT FK_OrderItems_OrderId FOREIGN KEY REFERENCES [Order].Orders(Id) ON DELETE CASCADE,
    ProductVariantId        BIGINT           NOT NULL CONSTRAINT FK_OrderItems_ProductVariantId FOREIGN KEY REFERENCES Product.ProductVariants(Id),
    ProductName             NVARCHAR(300)    NULL,
    VariantTitle            NVARCHAR(300)    NULL,
    Quantity                INT              NOT NULL,
    UnitPrice               DECIMAL(18,2)    NOT NULL,
    DiscountAmount          DECIMAL(18,2)    NOT NULL,
    TotalPrice              DECIMAL(18,2)    NOT NULL,
    VendorId                BIGINT           NULL,
    VendorCommissionRate    DECIMAL(5,2)     NULL
);
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Order' AND TABLE_NAME = 'OrderStatusHistories')
CREATE TABLE [Order].OrderStatusHistories (
    Id          BIGINT IDENTITY(1,1) PRIMARY KEY,
    OrderId     BIGINT           NOT NULL CONSTRAINT FK_OrderStatusHistories_OrderId FOREIGN KEY REFERENCES [Order].Orders(Id) ON DELETE CASCADE,
    FromStatus  TINYINT          NOT NULL,
    ToStatus    TINYINT          NOT NULL,
    Note        NVARCHAR(500)    NULL,
    ChangedBy   BIGINT           NULL,
    ChangedAt   DATETIME2        NOT NULL CONSTRAINT DF_OrderStatusHistories_ChangedAt DEFAULT GETUTCDATE()
);
GO

-- ── Discount Schema ──────────────────────────────────────────────────────────

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Discount' AND TABLE_NAME = 'Discounts')
CREATE TABLE Discount.Discounts (
    Id                  BIGINT IDENTITY(1,1) PRIMARY KEY,
    Code                NVARCHAR(50)     NOT NULL,
    Name                NVARCHAR(200)    NULL,
    [Description]       NVARCHAR(1000)   NULL,
    DiscountType        TINYINT          NOT NULL,
    Scope               TINYINT          NOT NULL,
    [Value]             DECIMAL(18,2)    NOT NULL,
    MaxDiscountAmount   DECIMAL(18,2)    NULL,
    MinOrderAmount      DECIMAL(18,2)    NULL,
    UsageLimit          INT              NULL,
    UsedCount           INT              NOT NULL CONSTRAINT DF_Discounts_UsedCount DEFAULT 0,
    PerUserLimit        INT              NULL,
    StartedAt           DATETIME2        NOT NULL,
    EndedAt             DATETIME2        NULL,
    IsActive            BIT              NOT NULL CONSTRAINT DF_Discounts_IsActive DEFAULT 1,
    CreatedAt           DATETIME2        NOT NULL,
    CreatedBy           BIGINT           NULL,
    UpdatedAt           DATETIME2        NULL,
    UpdatedBy           BIGINT           NULL
);
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Discount' AND TABLE_NAME = 'ProductDiscounts')
CREATE TABLE Discount.ProductDiscounts (
    Id                  BIGINT IDENTITY(1,1) PRIMARY KEY,
    DiscountId          BIGINT           NOT NULL CONSTRAINT FK_ProductDiscounts_DiscountId FOREIGN KEY REFERENCES Discount.Discounts(Id) ON DELETE CASCADE,
    ProductId           BIGINT           NOT NULL CONSTRAINT FK_ProductDiscounts_ProductId FOREIGN KEY REFERENCES Product.Products(Id),
    ProductVariantId    BIGINT           NULL CONSTRAINT FK_ProductDiscounts_ProductVariantId FOREIGN KEY REFERENCES Product.ProductVariants(Id)
);
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Discount' AND TABLE_NAME = 'CategoryDiscounts')
CREATE TABLE Discount.CategoryDiscounts (
    Id          BIGINT IDENTITY(1,1) PRIMARY KEY,
    DiscountId  BIGINT           NOT NULL CONSTRAINT FK_CategoryDiscounts_DiscountId FOREIGN KEY REFERENCES Discount.Discounts(Id) ON DELETE CASCADE,
    CategoryId  BIGINT           NOT NULL CONSTRAINT FK_CategoryDiscounts_CategoryId FOREIGN KEY REFERENCES Product.Categories(Id)
);
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Discount' AND TABLE_NAME = 'OrderDiscounts')
CREATE TABLE Discount.OrderDiscounts (
    Id              BIGINT IDENTITY(1,1) PRIMARY KEY,
    OrderId         BIGINT           NOT NULL CONSTRAINT FK_OrderDiscounts_OrderId FOREIGN KEY REFERENCES [Order].Orders(Id),
    DiscountId      BIGINT           NOT NULL CONSTRAINT FK_OrderDiscounts_DiscountId FOREIGN KEY REFERENCES Discount.Discounts(Id),
    DiscountAmount  DECIMAL(18,2)    NOT NULL,
    AppliedAt       DATETIME2        NOT NULL CONSTRAINT DF_OrderDiscounts_AppliedAt DEFAULT GETUTCDATE()
);
GO

-- ── Payment Schema ───────────────────────────────────────────────────────────

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Payment' AND TABLE_NAME = 'PaymentGateways')
CREATE TABLE Payment.PaymentGateways (
    Id          BIGINT IDENTITY(1,1) PRIMARY KEY,
    Name        NVARCHAR(200)    NOT NULL,
    Code        NVARCHAR(50)     NOT NULL,
    ConfigJson  NVARCHAR(MAX)    NOT NULL,
    Priority    INT              NOT NULL CONSTRAINT DF_PaymentGateways_Priority DEFAULT 0,
    IsActive    BIT              NOT NULL CONSTRAINT DF_PaymentGateways_IsActive DEFAULT 1,
    CreatedAt   DATETIME2        NOT NULL,
    CreatedBy   BIGINT           NULL,
    UpdatedAt   DATETIME2        NULL,
    UpdatedBy   BIGINT           NULL
);
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Payment' AND TABLE_NAME = 'Payments')
CREATE TABLE Payment.Payments (
    Id              BIGINT IDENTITY(1,1) PRIMARY KEY,
    OrderId         BIGINT           NOT NULL CONSTRAINT FK_Payments_OrderId FOREIGN KEY REFERENCES [Order].Orders(Id),
    GatewayId       BIGINT           NOT NULL CONSTRAINT FK_Payments_GatewayId FOREIGN KEY REFERENCES Payment.PaymentGateways(Id),
    Amount          DECIMAL(18,2)    NOT NULL,
    CurrencyId      BIGINT           NOT NULL CONSTRAINT FK_Payments_CurrencyId FOREIGN KEY REFERENCES Pricing.Currencies(Id),
    Status          TINYINT          NOT NULL,
    TransactionRef  NVARCHAR(100)    NULL,
    TrackingCode    NVARCHAR(100)    NULL,
    PaymentToken    NVARCHAR(500)    NULL,
    CallbackUrl     NVARCHAR(500)    NULL,
    BankMessage     NVARCHAR(500)    NULL,
    PaidAt          DATETIME2        NULL,
    CreatedAt       DATETIME2        NOT NULL CONSTRAINT DF_Payments_CreatedAt DEFAULT GETUTCDATE(),
    CreatedBy       BIGINT           NULL,
    UpdatedAt       DATETIME2        NULL,
    UpdatedBy       BIGINT           NULL
);
GO

-- ── Shipping Schema ──────────────────────────────────────────────────────────

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Shipping' AND TABLE_NAME = 'Carriers')
CREATE TABLE Shipping.Carriers (
    Id                    BIGINT IDENTITY(1,1) PRIMARY KEY,
    Name                  NVARCHAR(200)    NOT NULL,
    Code                  NVARCHAR(50)     NOT NULL,
    TrackingUrlTemplate   NVARCHAR(500)    NULL,
    ConfigJson            NVARCHAR(MAX)    NULL,
    IsActive              BIT              NOT NULL CONSTRAINT DF_Carriers_IsActive DEFAULT 1,
    CreatedAt             DATETIME2        NOT NULL,
    CreatedBy             BIGINT           NULL,
    UpdatedAt             DATETIME2        NULL,
    UpdatedBy             BIGINT           NULL
);
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Shipping' AND TABLE_NAME = 'DeliveryMethods')
CREATE TABLE Shipping.DeliveryMethods (
    Id                    BIGINT IDENTITY(1,1) PRIMARY KEY,
    Name                  NVARCHAR(200)    NOT NULL,
    DeliveryType          TINYINT          NOT NULL,
    CarrierId             BIGINT           NULL CONSTRAINT FK_DeliveryMethods_CarrierId FOREIGN KEY REFERENCES Shipping.Carriers(Id),
    Price                 DECIMAL(18,2)    NOT NULL,
    CurrencyId            BIGINT           NOT NULL CONSTRAINT FK_DeliveryMethods_CurrencyId FOREIGN KEY REFERENCES Pricing.Currencies(Id),
    EstimatedDays         INT              NULL,
    [Description]         NVARCHAR(500)    NULL,
    ConstraintsJson       NVARCHAR(MAX)    NULL,
    DigitalContentType    TINYINT          NULL,
    IsActive              BIT              NOT NULL CONSTRAINT DF_DeliveryMethods_IsActive DEFAULT 1,
    CreatedAt             DATETIME2        NOT NULL,
    CreatedBy             BIGINT           NULL,
    UpdatedAt             DATETIME2        NULL,
    UpdatedBy             BIGINT           NULL
);
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Shipping' AND TABLE_NAME = 'OrderDeliveries')
CREATE TABLE Shipping.OrderDeliveries (
    Id                    BIGINT IDENTITY(1,1) PRIMARY KEY,
    OrderId               BIGINT           NOT NULL CONSTRAINT FK_OrderDeliveries_OrderId FOREIGN KEY REFERENCES [Order].Orders(Id),
    DeliveryMethodId      BIGINT           NOT NULL CONSTRAINT FK_OrderDeliveries_DeliveryMethodId FOREIGN KEY REFERENCES Shipping.DeliveryMethods(Id),
    Status                TINYINT          NOT NULL,
    DeliveryAddressJson   NVARCHAR(MAX)    NULL,
    TrackingCode          NVARCHAR(100)    NULL,
    DigitalContent        NVARCHAR(MAX)    NULL,
    DigitalContentType    TINYINT          NULL,
    ShippedAt             DATETIME2        NULL,
    DeliveredAt           DATETIME2        NULL,
    Note                  NVARCHAR(500)    NULL,
    CreatedAt             DATETIME2        NOT NULL,
    CreatedBy             BIGINT           NULL,
    UpdatedAt             DATETIME2        NULL,
    UpdatedBy             BIGINT           NULL
);
GO

-- ── Accounting Schema ────────────────────────────────────────────────────────

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Accounting' AND TABLE_NAME = 'Accounts')
CREATE TABLE Accounting.Accounts (
    Id              BIGINT IDENTITY(1,1) PRIMARY KEY,
    Code            NVARCHAR(20)     NOT NULL,
    Name            NVARCHAR(200)    NOT NULL,
    AccountType     TINYINT          NOT NULL,
    ParentId        BIGINT           NULL CONSTRAINT FK_Accounts_ParentId FOREIGN KEY REFERENCES Accounting.Accounts(Id) ON DELETE RESTRICT,
    [Description]   NVARCHAR(500)    NULL,
    IsActive        BIT              NOT NULL CONSTRAINT DF_Accounts_IsActive DEFAULT 1,
    CreatedAt       DATETIME2        NOT NULL,
    CreatedBy       BIGINT           NULL,
    UpdatedAt       DATETIME2        NULL,
    UpdatedBy       BIGINT           NULL
);
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Accounting' AND TABLE_NAME = 'Transactions')
CREATE TABLE Accounting.Transactions (
    Id              BIGINT IDENTITY(1,1) PRIMARY KEY,
    AccountId       BIGINT           NOT NULL CONSTRAINT FK_Transactions_AccountId FOREIGN KEY REFERENCES Accounting.Accounts(Id),
    BatchId         UNIQUEIDENTIFIER NOT NULL,
    TransactionType TINYINT          NOT NULL,
    Amount          DECIMAL(18,2)    NOT NULL,
    CurrencyId      BIGINT           NOT NULL CONSTRAINT FK_Transactions_CurrencyId FOREIGN KEY REFERENCES Pricing.Currencies(Id),
    Source          TINYINT          NOT NULL,
    ReferenceId     BIGINT           NULL,
    [Description]   NVARCHAR(500)    NULL,
    CreatedAt       DATETIME2        NOT NULL CONSTRAINT DF_Transactions_CreatedAt DEFAULT GETUTCDATE(),
    CreatedBy       BIGINT           NULL
);
GO

-- ── Vendor Schema ────────────────────────────────────────────────────────────

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Vendor' AND TABLE_NAME = 'Vendors')
CREATE TABLE Vendor.Vendors (
    Id                  BIGINT IDENTITY(1,1) PRIMARY KEY,
    UserId              BIGINT           NOT NULL CONSTRAINT FK_Vendors_UserId FOREIGN KEY REFERENCES Identity.Users(Id),
    StoreName           NVARCHAR(200)    NOT NULL,
    StoreSlug           NVARCHAR(200)    NOT NULL,
    [Description]       NVARCHAR(1000)   NULL,
    LogoUrl             NVARCHAR(500)    NULL,
    CommissionRate      DECIMAL(5,2)     NOT NULL,
    Status              TINYINT          NOT NULL,
    BankAccountNumber   NVARCHAR(30)     NULL,
    IsActive            BIT              NOT NULL CONSTRAINT DF_Vendors_IsActive DEFAULT 1,
    CreatedAt           DATETIME2        NOT NULL,
    CreatedBy           BIGINT           NULL,
    UpdatedAt           DATETIME2        NULL,
    UpdatedBy           BIGINT           NULL
);
GO

-- ── Settings Schema ──────────────────────────────────────────────────────────

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Settings' AND TABLE_NAME = 'Settings')
CREATE TABLE Settings.Settings (
    Id              BIGINT IDENTITY(1,1) PRIMARY KEY,
    [Key]           NVARCHAR(200)    NOT NULL,
    [Value]         NVARCHAR(MAX)    NOT NULL,
    Module          NVARCHAR(100)    NOT NULL,
    [Description]   NVARCHAR(500)    NULL,
    ValueType       NVARCHAR(50)     NOT NULL CONSTRAINT DF_Settings_ValueType DEFAULT 'String',
    IsEditable      BIT              NOT NULL CONSTRAINT DF_Settings_IsEditable DEFAULT 1,
    CreatedAt       DATETIME2        NOT NULL,
    CreatedBy       BIGINT           NULL,
    UpdatedAt       DATETIME2        NULL,
    UpdatedBy       BIGINT           NULL
);
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Settings' AND TABLE_NAME = 'EnumValues')
CREATE TABLE Settings.EnumValues (
    Id              BIGINT IDENTITY(1,1) PRIMARY KEY,
    EnumType        NVARCHAR(100)    NOT NULL,
    EnumKey         NVARCHAR(100)    NOT NULL,
    EnumId          INT              NOT NULL,
    Name            NVARCHAR(200)    NOT NULL,
    [Description]   NVARCHAR(500)    NULL,
    DisplayOrder    INT              NOT NULL CONSTRAINT DF_EnumValues_DisplayOrder DEFAULT 0,
    IsActive        BIT              NOT NULL CONSTRAINT DF_EnumValues_IsActive DEFAULT 1
);
GO

-- ── Customization Schema ─────────────────────────────────────────────────────

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Customization' AND TABLE_NAME = 'SiteThemes')
CREATE TABLE Customization.SiteThemes (
    Id                      BIGINT IDENTITY(1,1) PRIMARY KEY,
    Name                    NVARCHAR(200)    NOT NULL,
    PrimaryColor            NVARCHAR(7)      NOT NULL,
    SecondaryColor          NVARCHAR(7)      NOT NULL,
    BackgroundColor         NVARCHAR(7)      NOT NULL,
    TextColor               NVARCHAR(7)      NOT NULL,
    AccentColor             NVARCHAR(7)      NOT NULL,
    FontFamily              NVARCHAR(100)    NULL,
    ExtendedSettingsJson    NVARCHAR(MAX)    NULL,
    IsActive                BIT              NOT NULL CONSTRAINT DF_SiteThemes_IsActive DEFAULT 0,
    CreatedAt               DATETIME2        NOT NULL,
    CreatedBy               BIGINT           NULL,
    UpdatedAt               DATETIME2        NULL,
    UpdatedBy               BIGINT           NULL
);
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Customization' AND TABLE_NAME = 'Sliders')
CREATE TABLE Customization.Sliders (
    Id              BIGINT IDENTITY(1,1) PRIMARY KEY,
    Title           NVARCHAR(200)    NOT NULL,
    SubTitle        NVARCHAR(300)    NULL,
    ImageUrl        NVARCHAR(500)    NOT NULL,
    MobileImageUrl  NVARCHAR(500)    NULL,
    LinkUrl         NVARCHAR(500)    NULL,
    LinkText        NVARCHAR(100)    NULL,
    DisplayOrder    INT              NOT NULL CONSTRAINT DF_Sliders_DisplayOrder DEFAULT 0,
    IsActive        BIT              NOT NULL CONSTRAINT DF_Sliders_IsActive DEFAULT 1,
    StartedAt       DATETIME2        NULL,
    EndedAt         DATETIME2        NULL,
    CreatedAt       DATETIME2        NOT NULL,
    CreatedBy       BIGINT           NULL,
    UpdatedAt       DATETIME2        NULL,
    UpdatedBy       BIGINT           NULL
);
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Customization' AND TABLE_NAME = 'Widgets')
CREATE TABLE Customization.Widgets (
    Id          BIGINT IDENTITY(1,1) PRIMARY KEY,
    Type        NVARCHAR(100)    NOT NULL,
    Title       NVARCHAR(200)    NOT NULL,
    ConfigJson  NVARCHAR(MAX)    NOT NULL,
    Position    NVARCHAR(100)    NOT NULL,
    DisplayOrder INT             NOT NULL CONSTRAINT DF_Widgets_DisplayOrder DEFAULT 0,
    IsActive    BIT              NOT NULL CONSTRAINT DF_Widgets_IsActive DEFAULT 1,
    StartedAt   DATETIME2        NULL,
    EndedAt     DATETIME2        NULL,
    CreatedAt   DATETIME2        NOT NULL,
    CreatedBy   BIGINT           NULL,
    UpdatedAt   DATETIME2        NULL,
    UpdatedBy   BIGINT           NULL
);
GO

-- ============================================================================
-- 4. CREATE INDEXES
-- ============================================================================

-- ── Identity Indexes ─────────────────────────────────────────────────────────

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Users_UserName')
    CREATE UNIQUE INDEX IX_Users_UserName ON Identity.Users (UserName);
GO

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Users_Email')
    CREATE UNIQUE INDEX IX_Users_Email ON Identity.Users (Email) WHERE [Email] IS NOT NULL;
GO

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Users_PhoneNumber')
    CREATE UNIQUE INDEX IX_Users_PhoneNumber ON Identity.Users (PhoneNumber) WHERE [PhoneNumber] IS NOT NULL;
GO

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Users_NationalCode')
    CREATE UNIQUE INDEX IX_Users_NationalCode ON Identity.Users (NationalCode) WHERE [NationalCode] IS NOT NULL;
GO

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Users_IsActive')
    CREATE INDEX IX_Users_IsActive ON Identity.Users (IsActive);
GO

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Roles_Name')
    CREATE UNIQUE INDEX IX_Roles_Name ON Identity.Roles (Name);
GO

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_UserRoles_UserId_RoleId')
    CREATE UNIQUE INDEX IX_UserRoles_UserId_RoleId ON Identity.UserRoles (UserId, RoleId);
GO

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_UserRoles_RoleId')
    CREATE INDEX IX_UserRoles_RoleId ON Identity.UserRoles (RoleId);
GO

-- ── Product Indexes ──────────────────────────────────────────────────────────

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Products_ProductCode')
    CREATE UNIQUE INDEX IX_Products_ProductCode ON Product.Products (ProductCode);
GO

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Products_Slug')
    CREATE UNIQUE INDEX IX_Products_Slug ON Product.Products (Slug);
GO

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Products_BrandId')
    CREATE INDEX IX_Products_BrandId ON Product.Products (BrandId);
GO

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Products_VendorId')
    CREATE INDEX IX_Products_VendorId ON Product.Products (VendorId);
GO

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Products_IsActive')
    CREATE INDEX IX_Products_IsActive ON Product.Products (IsActive);
GO

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Products_IsFeatured')
    CREATE INDEX IX_Products_IsFeatured ON Product.Products (IsFeatured);
GO

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_ProductVariants_VariantCode')
    CREATE UNIQUE INDEX IX_ProductVariants_VariantCode ON Product.ProductVariants (VariantCode);
GO

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_ProductVariants_Sku')
    CREATE UNIQUE INDEX IX_ProductVariants_Sku ON Product.ProductVariants (Sku);
GO

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_ProductVariants_ProductId')
    CREATE INDEX IX_ProductVariants_ProductId ON Product.ProductVariants (ProductId);
GO

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_ProductVariants_VendorId')
    CREATE INDEX IX_ProductVariants_VendorId ON Product.ProductVariants (VendorId);
GO

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_ProductVariants_ProductId_IsActive')
    CREATE INDEX IX_ProductVariants_ProductId_IsActive ON Product.ProductVariants (ProductId, IsActive);
GO

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Categories_Code')
    CREATE UNIQUE INDEX IX_Categories_Code ON Product.Categories (Code);
GO

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Categories_Slug')
    CREATE UNIQUE INDEX IX_Categories_Slug ON Product.Categories (Slug);
GO

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Categories_ParentId')
    CREATE INDEX IX_Categories_ParentId ON Product.Categories (ParentId);
GO

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Brands_Name')
    CREATE UNIQUE INDEX IX_Brands_Name ON Product.Brands (Name);
GO

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Brands_Slug')
    CREATE UNIQUE INDEX IX_Brands_Slug ON Product.Brands (Slug);
GO

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_ProductCategories_ProductId_CategoryId')
    CREATE UNIQUE INDEX IX_ProductCategories_ProductId_CategoryId ON Product.ProductCategories (ProductId, CategoryId);
GO

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_ProductAttributes_ProductId_Key')
    CREATE INDEX IX_ProductAttributes_ProductId_Key ON Product.ProductAttributes (ProductId, [Key]);
GO

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_ProductImages_ProductVariantId')
    CREATE INDEX IX_ProductImages_ProductVariantId ON Product.ProductImages (ProductVariantId);
GO

-- ── Pricing Indexes ──────────────────────────────────────────────────────────

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Currencies_Code')
    CREATE UNIQUE INDEX IX_Currencies_Code ON Pricing.Currencies (Code);
GO

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Prices_ProductVariantId_PriceType_EndedAt_Active')
    CREATE INDEX IX_Prices_ProductVariantId_PriceType_EndedAt_Active ON Pricing.Prices (ProductVariantId, PriceType, EndedAt) WHERE [EndedAt] IS NULL;
GO

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Prices_CurrencyId')
    CREATE INDEX IX_Prices_CurrencyId ON Pricing.Prices (CurrencyId);
GO

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Prices_StartedAt_EndedAt')
    CREATE INDEX IX_Prices_StartedAt_EndedAt ON Pricing.Prices (StartedAt, EndedAt);
GO

-- ── Inventory Indexes ────────────────────────────────────────────────────────

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Warehouses_Code')
    CREATE UNIQUE INDEX IX_Warehouses_Code ON Inventory.Warehouses (Code);
GO

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_StockLedgers_ProductVariantId_WarehouseId')
    CREATE INDEX IX_StockLedgers_ProductVariantId_WarehouseId ON Inventory.StockLedgers (ProductVariantId, WarehouseId);
GO

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_StockLedgers_CreatedAt')
    CREATE INDEX IX_StockLedgers_CreatedAt ON Inventory.StockLedgers (CreatedAt);
GO

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_StockLedgers_Reason')
    CREATE INDEX IX_StockLedgers_Reason ON Inventory.StockLedgers (Reason);
GO

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_StockLedgers_ReferenceId')
    CREATE INDEX IX_StockLedgers_ReferenceId ON Inventory.StockLedgers (ReferenceId);
GO

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_StockLedgers_ProductVariantId_CreatedAt')
    CREATE INDEX IX_StockLedgers_ProductVariantId_CreatedAt ON Inventory.StockLedgers (ProductVariantId, CreatedAt);
GO

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_StockReservations_OrderId')
    CREATE INDEX IX_StockReservations_OrderId ON Inventory.StockReservations (OrderId);
GO

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_StockReservations_ProductVariantId_WarehouseId_IsReleased')
    CREATE INDEX IX_StockReservations_ProductVariantId_WarehouseId_IsReleased ON Inventory.StockReservations (ProductVariantId, WarehouseId, IsReleased);
GO

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_StockReservations_ExpiresAt_IsReleased')
    CREATE INDEX IX_StockReservations_ExpiresAt_IsReleased ON Inventory.StockReservations (ExpiresAt, IsReleased);
GO

-- ── Order Indexes ────────────────────────────────────────────────────────────

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Carts_UserId_IsActive')
    CREATE INDEX IX_Carts_UserId_IsActive ON [Order].Carts (UserId, IsActive);
GO

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_CartItems_CartId_ProductVariantId')
    CREATE UNIQUE INDEX IX_CartItems_CartId_ProductVariantId ON [Order].CartItems (CartId, ProductVariantId);
GO

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Orders_OrderNumber')
    CREATE UNIQUE INDEX IX_Orders_OrderNumber ON [Order].Orders (OrderNumber);
GO

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Orders_UserId')
    CREATE INDEX IX_Orders_UserId ON [Order].Orders (UserId);
GO

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Orders_Status')
    CREATE INDEX IX_Orders_Status ON [Order].Orders (Status);
GO

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Orders_CreatedAt')
    CREATE INDEX IX_Orders_CreatedAt ON [Order].Orders (CreatedAt);
GO

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Orders_UserId_Status')
    CREATE INDEX IX_Orders_UserId_Status ON [Order].Orders (UserId, Status);
GO

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Orders_Status_CreatedAt')
    CREATE INDEX IX_Orders_Status_CreatedAt ON [Order].Orders (Status, CreatedAt);
GO

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_OrderItems_ProductVariantId')
    CREATE INDEX IX_OrderItems_ProductVariantId ON [Order].OrderItems (ProductVariantId);
GO

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_OrderItems_VendorId')
    CREATE INDEX IX_OrderItems_VendorId ON [Order].OrderItems (VendorId);
GO

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_OrderStatusHistories_OrderId')
    CREATE INDEX IX_OrderStatusHistories_OrderId ON [Order].OrderStatusHistories (OrderId);
GO

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_OrderStatusHistories_ChangedAt')
    CREATE INDEX IX_OrderStatusHistories_ChangedAt ON [Order].OrderStatusHistories (ChangedAt);
GO

-- ── Discount Indexes ─────────────────────────────────────────────────────────

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Discounts_Code')
    CREATE UNIQUE INDEX IX_Discounts_Code ON Discount.Discounts (Code);
GO

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Discounts_IsActive_StartedAt_EndedAt')
    CREATE INDEX IX_Discounts_IsActive_StartedAt_EndedAt ON Discount.Discounts (IsActive, StartedAt, EndedAt);
GO

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_ProductDiscounts_DiscountId_ProductId_ProductVariantId')
    CREATE UNIQUE INDEX IX_ProductDiscounts_DiscountId_ProductId_ProductVariantId ON Discount.ProductDiscounts (DiscountId, ProductId, ProductVariantId);
GO

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_CategoryDiscounts_DiscountId_CategoryId')
    CREATE UNIQUE INDEX IX_CategoryDiscounts_DiscountId_CategoryId ON Discount.CategoryDiscounts (DiscountId, CategoryId);
GO

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_OrderDiscounts_OrderId')
    CREATE INDEX IX_OrderDiscounts_OrderId ON Discount.OrderDiscounts (OrderId);
GO

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_OrderDiscounts_DiscountId')
    CREATE INDEX IX_OrderDiscounts_DiscountId ON Discount.OrderDiscounts (DiscountId);
GO

-- ── Payment Indexes ──────────────────────────────────────────────────────────

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_PaymentGateways_Code')
    CREATE UNIQUE INDEX IX_PaymentGateways_Code ON Payment.PaymentGateways (Code);
GO

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Payments_TransactionRef')
    CREATE UNIQUE INDEX IX_Payments_TransactionRef ON Payment.Payments (TransactionRef) WHERE [TransactionRef] IS NOT NULL;
GO

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Payments_OrderId')
    CREATE INDEX IX_Payments_OrderId ON Payment.Payments (OrderId);
GO

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Payments_GatewayId')
    CREATE INDEX IX_Payments_GatewayId ON Payment.Payments (GatewayId);
GO

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Payments_Status')
    CREATE INDEX IX_Payments_Status ON Payment.Payments (Status);
GO

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Payments_OrderId_Status')
    CREATE INDEX IX_Payments_OrderId_Status ON Payment.Payments (OrderId, Status);
GO

-- ── Shipping Indexes ─────────────────────────────────────────────────────────

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Carriers_Code')
    CREATE UNIQUE INDEX IX_Carriers_Code ON Shipping.Carriers (Code);
GO

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_DeliveryMethods_DeliveryType')
    CREATE INDEX IX_DeliveryMethods_DeliveryType ON Shipping.DeliveryMethods (DeliveryType);
GO

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_DeliveryMethods_IsActive')
    CREATE INDEX IX_DeliveryMethods_IsActive ON Shipping.DeliveryMethods (IsActive);
GO

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_OrderDeliveries_OrderId')
    CREATE INDEX IX_OrderDeliveries_OrderId ON Shipping.OrderDeliveries (OrderId);
GO

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_OrderDeliveries_DeliveryMethodId')
    CREATE INDEX IX_OrderDeliveries_DeliveryMethodId ON Shipping.OrderDeliveries (DeliveryMethodId);
GO

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_OrderDeliveries_Status')
    CREATE INDEX IX_OrderDeliveries_Status ON Shipping.OrderDeliveries (Status);
GO

-- ── Accounting Indexes ───────────────────────────────────────────────────────

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Accounts_Code')
    CREATE UNIQUE INDEX IX_Accounts_Code ON Accounting.Accounts (Code);
GO

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Accounts_AccountType')
    CREATE INDEX IX_Accounts_AccountType ON Accounting.Accounts (AccountType);
GO

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Accounts_ParentId')
    CREATE INDEX IX_Accounts_ParentId ON Accounting.Accounts (ParentId);
GO

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Transactions_AccountId')
    CREATE INDEX IX_Transactions_AccountId ON Accounting.Transactions (AccountId);
GO

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Transactions_BatchId')
    CREATE INDEX IX_Transactions_BatchId ON Accounting.Transactions (BatchId);
GO

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Transactions_CreatedAt')
    CREATE INDEX IX_Transactions_CreatedAt ON Accounting.Transactions (CreatedAt);
GO

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Transactions_Source')
    CREATE INDEX IX_Transactions_Source ON Accounting.Transactions (Source);
GO

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Transactions_AccountId_CreatedAt')
    CREATE INDEX IX_Transactions_AccountId_CreatedAt ON Accounting.Transactions (AccountId, CreatedAt);
GO

-- ── Vendor Indexes ───────────────────────────────────────────────────────────

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Vendors_UserId')
    CREATE UNIQUE INDEX IX_Vendors_UserId ON Vendor.Vendors (UserId);
GO

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Vendors_StoreSlug')
    CREATE UNIQUE INDEX IX_Vendors_StoreSlug ON Vendor.Vendors (StoreSlug);
GO

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Vendors_Status')
    CREATE INDEX IX_Vendors_Status ON Vendor.Vendors (Status);
GO

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Vendors_IsActive')
    CREATE INDEX IX_Vendors_IsActive ON Vendor.Vendors (IsActive);
GO

-- ── Settings Indexes ─────────────────────────────────────────────────────────

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Settings_Key_Module')
    CREATE UNIQUE INDEX IX_Settings_Key_Module ON Settings.Settings ([Key], Module);
GO

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_EnumValues_EnumType_EnumKey')
    CREATE UNIQUE INDEX IX_EnumValues_EnumType_EnumKey ON Settings.EnumValues (EnumType, EnumKey);
GO

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_EnumValues_EnumType')
    CREATE INDEX IX_EnumValues_EnumType ON Settings.EnumValues (EnumType);
GO

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_EnumValues_EnumType_EnumId')
    CREATE INDEX IX_EnumValues_EnumType_EnumId ON Settings.EnumValues (EnumType, EnumId);
GO

-- ── Customization Indexes ────────────────────────────────────────────────────

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Sliders_IsActive_DisplayOrder')
    CREATE INDEX IX_Sliders_IsActive_DisplayOrder ON Customization.Sliders (IsActive, DisplayOrder);
GO

IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_Widgets_Position_DisplayOrder')
    CREATE INDEX IX_Widgets_Position_DisplayOrder ON Customization.Widgets (Position, DisplayOrder);
GO

-- ============================================================================
-- 5. SEED DATA
-- ============================================================================

-- ── Currencies ───────────────────────────────────────────────────────────────
IF NOT EXISTS (SELECT 1 FROM Pricing.Currencies)
BEGIN
    INSERT INTO Pricing.Currencies (Code, Name, Symbol, ExchangeRate, IsBase, IsActive, CreatedAt)
    VALUES
        (N'IRR', N'تومان', N'ت', 1.0, 1, 1, GETUTCDATE()),
        (N'USD', N'دلار آمریکا', N'$', 50000.0, 0, 1, GETUTCDATE());
END
GO

-- ── Settings ─────────────────────────────────────────────────────────────────
IF NOT EXISTS (SELECT 1 FROM Settings.Settings)
BEGIN
    INSERT INTO Settings.Settings ([Key], [Value], Module, [Description], ValueType, IsEditable, CreatedAt)
    VALUES
        (N'DefaultCurrencyId',          N'1',     N'Pricing',  N'شناسه ارز پیش‌فرض سیستم',            N'Long',    1, GETUTCDATE()),
        (N'SiteName',                   N'فروشگاه آنلاین', N'General', N'نام فروشگاه',               N'String',  1, GETUTCDATE()),
        (N'SiteDescription',            N'فروشگاه آنلاین با بهترین محصولات و قیمت‌ها', N'General', N'توضیحات فروشگاه', N'String', 1, GETUTCDATE()),
        (N'OrderPrefix',                N'ORD',   N'Order',    N'پیشوند شماره سفارش',                N'String',  1, GETUTCDATE()),
        (N'TaxRate',                    N'0.09',  N'Pricing',  N'نرخ مالیات بر ارزش افزوده',          N'Decimal', 1, GETUTCDATE()),
        (N'FreeShippingThreshold',      N'500000', N'Shipping', N'حداقل مبلغ سفارش برای ارسال رایگان (تومان)', N'Decimal', 1, GETUTCDATE()),
        (N'DefaultWarehouseId',         N'1',     N'Inventory',N'شناسه انبار پیش‌فرض',                N'Long',    1, GETUTCDATE()),
        (N'LowStockThreshold',          N'5',     N'Inventory',N'آستانه هشدار کمبود موجودی',          N'Int',     1, GETUTCDATE()),
        (N'AdminEmail',                 N'admin@example.com', N'General', N'ایمیل مدیر سیستم',        N'String',  1, GETUTCDATE()),
        (N'EnableRegistration',         N'true',  N'Identity', N'فعال‌سازی ثبت‌نام کاربران',           N'Boolean', 1, GETUTCDATE()),
        (N'EnableGuestCheckout',        N'true',  N'Order',    N'فعال‌سازی خرید بدون حساب کاربری',     N'Boolean', 1, GETUTCDATE()),
        (N'VendorCommissionRate',       N'0.10',  N'Vendor',   N'نرخ کمیسیون فروشنده',                N'Decimal', 1, GETUTCDATE()),
        (N'MaxCartItems',               N'50',    N'Order',    N'حداکثر آیتم‌های سبد خرید',           N'Int',     1, GETUTCDATE()),
        (N'SessionTimeoutMinutes',      N'30',    N'General',  N'مدت زمان نشست کاربر (دقیقه)',         N'Int',     1, GETUTCDATE()),
        (N'PaymentTimeoutMinutes',      N'15',    N'Payment',  N'مدت زمان اعتبار پرداخت (دقیقه)',      N'Int',     1, GETUTCDATE());
END
GO

-- ── Enum Values ──────────────────────────────────────────────────────────────
IF NOT EXISTS (SELECT 1 FROM Settings.EnumValues)
BEGIN
    -- OrderStatus (1-7)
    INSERT INTO Settings.EnumValues (EnumType, EnumKey, EnumId, Name, DisplayOrder, IsActive) VALUES
        (N'OrderStatus',     N'Pending',    1, N'در انتظار',          1, 1),
        (N'OrderStatus',     N'Paid',       2, N'پرداخت شده',        2, 1),
        (N'OrderStatus',     N'Processing', 3, N'در حال پردازش',      3, 1),
        (N'OrderStatus',     N'Shipped',    4, N'ارسال شده',          4, 1),
        (N'OrderStatus',     N'Delivered',  5, N'تحویل داده شده',    5, 1),
        (N'OrderStatus',     N'Cancelled',  6, N'لغو شده',            6, 1),
        (N'OrderStatus',     N'Refunded',   7, N'بازپرداخت شده',     7, 1);

    -- PaymentStatus (1-5)
    INSERT INTO Settings.EnumValues (EnumType, EnumKey, EnumId, Name, DisplayOrder, IsActive) VALUES
        (N'PaymentStatus',   N'Created',    1, N'ایجاد شده',          1, 1),
        (N'PaymentStatus',   N'Pending',    2, N'در انتظار پرداخت',   2, 1),
        (N'PaymentStatus',   N'Succeeded',  3, N'موفق',               3, 1),
        (N'PaymentStatus',   N'Failed',     4, N'ناموفق',              4, 1),
        (N'PaymentStatus',   N'Refunded',   5, N'بازپرداخت شده',     5, 1);

    -- DeliveryStatus (1-4)
    INSERT INTO Settings.EnumValues (EnumType, EnumKey, EnumId, Name, DisplayOrder, IsActive) VALUES
        (N'DeliveryStatus',  N'Pending',    1, N'در انتظار ارسال',    1, 1),
        (N'DeliveryStatus',  N'InTransit',  2, N'در مسیر',             2, 1),
        (N'DeliveryStatus',  N'Delivered',  3, N'تحویل داده شده',     3, 1),
        (N'DeliveryStatus',  N'Failed',     4, N'ناموفق',              4, 1);

    -- DeliveryType (1-3)
    INSERT INTO Settings.EnumValues (EnumType, EnumKey, EnumId, Name, DisplayOrder, IsActive) VALUES
        (N'DeliveryType',    N'Physical',   1, N'ارسال پستی',          1, 1),
        (N'DeliveryType',    N'InPerson',   2, N'تحویل حضوری',         2, 1),
        (N'DeliveryType',    N'Digital',    3, N'محتوای دیجیتال',      3, 1);

    -- DigitalContentType (1-3)
    INSERT INTO Settings.EnumValues (EnumType, EnumKey, EnumId, Name, DisplayOrder, IsActive) VALUES
        (N'DigitalContentType', N'Link',    1, N'لینک دانلود',         1, 1),
        (N'DigitalContentType', N'File',    2, N'فایل دانلودی',        2, 1),
        (N'DigitalContentType', N'Content', 3, N'محتوای متنی',         3, 1);

    -- PriceType (1-4)
    INSERT INTO Settings.EnumValues (EnumType, EnumKey, EnumId, Name, DisplayOrder, IsActive) VALUES
        (N'PriceType',       N'Retail',     1, N'خرده‌فروشی',          1, 1),
        (N'PriceType',       N'Wholesale',  2, N'عمده‌فروشی',          2, 1),
        (N'PriceType',       N'Special',    3, N'ویژه',                 3, 1),
        (N'PriceType',       N'Cost',       4, N'قیمت تمام شده',       4, 1);

    -- StockChangeReason (1-8)
    INSERT INTO Settings.EnumValues (EnumType, EnumKey, EnumId, Name, DisplayOrder, IsActive) VALUES
        (N'StockChangeReason', N'StockIn',             1, N'ورود کالا',         1, 1),
        (N'StockChangeReason', N'StockOut',            2, N'خروج کالا',         2, 1),
        (N'StockChangeReason', N'Return',              3, N'مرجوعی',             3, 1),
        (N'StockChangeReason', N'Damaged',             4, N'کالای آسیب‌دیده',   4, 1),
        (N'StockChangeReason', N'Initial',             5, N'موجودی اولیه',       5, 1),
        (N'StockChangeReason', N'ManualAdjustment',    6, N'تعدیل دستی',         6, 1),
        (N'StockChangeReason', N'Reserved',            7, N'رزرو شده',           7, 1),
        (N'StockChangeReason', N'ReservationReleased', 8, N'آزادسازی رزرو',     8, 1);

    -- DiscountType (1-3)
    INSERT INTO Settings.EnumValues (EnumType, EnumKey, EnumId, Name, DisplayOrder, IsActive) VALUES
        (N'DiscountType',    N'Percentage',    1, N'درصدی',               1, 1),
        (N'DiscountType',    N'FixedAmount',   2, N'مبلغ ثابت',           2, 1),
        (N'DiscountType',    N'FreeShipping',  3, N'ارسال رایگان',        3, 1);

    -- DiscountScope (1-4)
    INSERT INTO Settings.EnumValues (EnumType, EnumKey, EnumId, Name, DisplayOrder, IsActive) VALUES
        (N'DiscountScope',   N'Order',     1, N'سفارش',                1, 1),
        (N'DiscountScope',   N'Product',   2, N'محصول',                2, 1),
        (N'DiscountScope',   N'Category',  3, N'دسته‌بندی',             3, 1),
        (N'DiscountScope',   N'Shipping',   4, N'هزینه ارسال',          4, 1);

    -- ShippingConstraintType (1-5)
    INSERT INTO Settings.EnumValues (EnumType, EnumKey, EnumId, Name, DisplayOrder, IsActive) VALUES
        (N'ShippingConstraintType', N'Fragile',    1, N'شکننده',             1, 1),
        (N'ShippingConstraintType', N'Heavy',      2, N'سنگین',              2, 1),
        (N'ShippingConstraintType', N'Oversized',  3, N'بزرگ‌تر از حد معمول', 3, 1),
        (N'ShippingConstraintType', N'Hazardous',  4, N'خطرناک',              4, 1),
        (N'ShippingConstraintType', N'Perishable', 5, N'فاسدشدنی',           5, 1);

    -- AccountType (1-5)
    INSERT INTO Settings.EnumValues (EnumType, EnumKey, EnumId, Name, DisplayOrder, IsActive) VALUES
        (N'AccountType',     N'Asset',      1, N'دارایی',                1, 1),
        (N'AccountType',     N'Liability',  2, N'بدهی',                  2, 1),
        (N'AccountType',     N'Revenue',    3, N'درآمد',                 3, 1),
        (N'AccountType',     N'Expense',    4, N'هزینه',                 4, 1),
        (N'AccountType',     N'Equity',     5, N'حقوق صاحبان سهام',      5, 1);

    -- TransactionType (1-2)
    INSERT INTO Settings.EnumValues (EnumType, EnumKey, EnumId, Name, DisplayOrder, IsActive) VALUES
        (N'TransactionType', N'Debit',      1, N'بدهکار',                1, 1),
        (N'TransactionType', N'Credit',     2, N'بستانکار',              2, 1);

    -- TransactionSource (1-6)
    INSERT INTO Settings.EnumValues (EnumType, EnumKey, EnumId, Name, DisplayOrder, IsActive) VALUES
        (N'TransactionSource', N'Order',             1, N'سفارش',              1, 1),
        (N'TransactionSource', N'Refund',            2, N'بازپرداخت',          2, 1),
        (N'TransactionSource', N'Shipping',          3, N'ارسال',              3, 1),
        (N'TransactionSource', N'VendorCommission',  4, N'کمیسیون فروشنده',    4, 1),
        (N'TransactionSource', N'Operational',       5, N'عملیاتی',             5, 1),
        (N'TransactionSource', N'Manual',            6, N'دستی',                6, 1);

    -- VendorStatus (1-4)
    INSERT INTO Settings.EnumValues (EnumType, EnumKey, EnumId, Name, DisplayOrder, IsActive) VALUES
        (N'VendorStatus',    N'Pending',    1, N'در انتظار تأیید',       1, 1),
        (N'VendorStatus',    N'Active',     2, N'فعال',                  2, 1),
        (N'VendorStatus',    N'Suspended',  3, N'معلق',                  3, 1),
        (N'VendorStatus',    N'Rejected',   4, N'رد شده',                4, 1);
END
GO

-- ── Payment Gateways ────────────────────────────────────────────────────────
IF NOT EXISTS (SELECT 1 FROM Payment.PaymentGateways)
BEGIN
    INSERT INTO Payment.PaymentGateways (Name, Code, ConfigJson, Priority, IsActive, CreatedAt)
    VALUES
        (N'زرین‌پال', N'Zarinpal', N'{"MerchantId":"","Sandbox":false,"CallbackUrl":"/api/payment/callback/zarinpal"}', 1, 1, GETUTCDATE());
END
GO

-- ── Warehouses ───────────────────────────────────────────────────────────────
IF NOT EXISTS (SELECT 1 FROM Inventory.Warehouses)
BEGIN
    INSERT INTO Inventory.Warehouses (Name, Code, [Address], City, Province, IsActive, CreatedAt)
    VALUES
        (N'انبار مرکزی', N'WH-MAIN', N'تهران، خیابان ولیعصر، پلاک ۱۲۳', N'تهران', N'تهران', 1, GETUTCDATE());
END
GO

-- ── Accounts ────────────────────────────────────────────────────────────────
IF NOT EXISTS (SELECT 1 FROM Accounting.Accounts)
BEGIN
    INSERT INTO Accounting.Accounts (Code, Name, AccountType, [Description], IsActive, CreatedAt)
    VALUES
        (N'1001', N'صندوق نقدی',                        1, N'حساب صندوق نقدی فروشگاه',                           1, GETUTCDATE()),
        (N'1002', N'حساب بانکی',                         1, N'حساب بانکی فروشگاه (ملت)',                          1, GETUTCDATE()),
        (N'4001', N'درآمد فروش',                         3, N'درآمد حاصل از فروش محصولات',                       1, GETUTCDATE()),
        (N'5001', N'بهای تمام شده کالای فروش رفته',      4, N'هزینه خرید کالاهای فروخته شده',                    1, GETUTCDATE()),
        (N'4002', N'درآمد ارسال',                         3, N'درآمد حاصل از هزینه ارسال',                        1, GETUTCDATE()),
        (N'2001', N'پرداختی فروشندگان',                   2, N'بدهی به فروشندگان برای کمیسیون و فروش محصولات',    1, GETUTCDATE());
END
GO

-- ── Delivery Methods ────────────────────────────────────────────────────────
IF NOT EXISTS (SELECT 1 FROM Shipping.DeliveryMethods)
BEGIN
    DECLARE @BaseCurrencyId BIGINT = (SELECT TOP 1 Id FROM Pricing.Currencies WHERE IsBase = 1);
    IF @BaseCurrencyId IS NULL SET @BaseCurrencyId = 1;

    INSERT INTO Shipping.DeliveryMethods (Name, DeliveryType, Price, CurrencyId, EstimatedDays, [Description], DigitalContentType, IsActive, CreatedAt)
    VALUES
        (N'ارسال پستی',  1, 50000,  @BaseCurrencyId, 3, N'ارسال از طریق پست پیشتاز به سراسر کشور', NULL,       1, GETUTCDATE()),
        (N'تحویل حضوری', 2, 0,       @BaseCurrencyId, 0, N'تحویل حضوری از فروشگاه',                 NULL,       1, GETUTCDATE()),
        (N'لینک دانلود', 3, 0,       @BaseCurrencyId, 0, N'ارسال لینک دانلود محصول دیجیتال',        1,          1, GETUTCDATE());
END
GO

-- ── Site Themes ─────────────────────────────────────────────────────────────
IF NOT EXISTS (SELECT 1 FROM Customization.SiteThemes)
BEGIN
    INSERT INTO Customization.SiteThemes (Name, PrimaryColor, SecondaryColor, BackgroundColor, TextColor, AccentColor, FontFamily, ExtendedSettingsJson, IsActive, CreatedAt)
    VALUES
        (N'پوسته پیش‌فرض', N'#3B82F6', N'#10B981', N'#FFFFFF', N'#1F2937', N'#F59E0B',
         N'Vazirmatn, sans-serif', N'{"BorderRadius":"8px","DarkMode":false,"SidebarCollapsed":false}', 1, GETUTCDATE());
END
GO

-- ── Sliders ──────────────────────────────────────────────────────────────────
IF NOT EXISTS (SELECT 1 FROM Customization.Sliders)
BEGIN
    DECLARE @SliderNow DATETIME2 = GETUTCDATE();
    INSERT INTO Customization.Sliders (Title, SubTitle, ImageUrl, MobileImageUrl, LinkUrl, LinkText, DisplayOrder, IsActive, StartedAt, EndedAt, CreatedAt)
    VALUES
        (N'جشنواره فروش ویژه', N'تا ۵۰٪ تخفیف روی تمامی محصولات', N'/images/sliders/sale-banner.jpg', N'/images/sliders/sale-banner-mobile.jpg', N'/products?discount=true', N'مشاهده محصولات', 1, 1, @SliderNow, DATEADD(DAY, 30, @SliderNow), @SliderNow),
        (N'محصولات جدید فصل',  N'جدیدترین محصولات با بهترین کیفیت', N'/images/sliders/new-arrivals.jpg', N'/images/sliders/new-arrivals-mobile.jpg', N'/products?new=true', N'کشف کنید', 2, 1, @SliderNow, DATEADD(DAY, 60, @SliderNow), @SliderNow);
END
GO

-- ── Admin Role & User ───────────────────────────────────────────────────────
IF NOT EXISTS (SELECT 1 FROM Identity.Roles WHERE Name = N'Admin')
BEGIN
    INSERT INTO Identity.Roles (Name, DisplayName, [Description], IsActive, CreatedAt)
    VALUES (N'Admin', N'مدیر سیستم', N'دسترسی کامل به تمام بخش‌های سیستم', 1, GETUTCDATE());
END
GO

IF NOT EXISTS (SELECT 1 FROM Identity.Users WHERE UserName = N'admin')
BEGIN
    -- Password: admin123 → SHA256(password + salt)
    -- Salt: randomly generated, stored as Base64
    -- For seed script, we use a deterministic hash:
    -- Salt: "dGVzdHNhbHQxMjM0NTY=" (testsalt123456 in base64)
    -- Hash: SHA256("admin123" + "dGVzdHNhbHQxMjM0NTY=")
    DECLARE @Salt NVARCHAR(500) = N'dGVzdHNhbHQxMjM0NTY=';
    DECLARE @Hash NVARCHAR(500) = N'V5H+vPJwBJ6nGZfCmH8wkx3MR+7B1T3KZ5hJqaGP8qY=';

    INSERT INTO Identity.Users (UserName, Email, PhoneNumber, PasswordHash, PasswordSalt, FirstName, LastName, IsActive, IsDeleted, CreatedAt)
    VALUES (N'admin', N'admin@example.com', N'09123456789', @Hash, @Salt, N'مدیر', N'سیستم', 1, 0, GETUTCDATE());

    DECLARE @AdminUserId BIGINT = SCOPE_IDENTITY();
    DECLARE @AdminRoleId BIGINT = (SELECT Id FROM Identity.Roles WHERE Name = N'Admin');

    INSERT INTO Identity.UserRoles (UserId, RoleId, AssignedAt)
    VALUES (@AdminUserId, @AdminRoleId, GETUTCDATE());
END
GO

-- ============================================================================
-- END OF SCRIPT
-- ============================================================================
PRINT N'✅ ECommerceDb script completed successfully!';
PRINT N'   - 12 schemas created';
PRINT N'   - 27 tables created';
PRINT N'   - Indexes created';
PRINT N'   - Seed data inserted';
GO
