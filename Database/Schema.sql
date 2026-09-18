-- ECommerceShoes full schema + seed
-- Run on SQL Server before starting the API

IF DB_ID(N'ECommerceShoes') IS NULL
    CREATE DATABASE ECommerceShoes;
GO

USE ECommerceShoes;
GO

IF OBJECT_ID(N'dbo.Role', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Role (
        RoleId INT NOT NULL PRIMARY KEY,
        Name NVARCHAR(255) NULL
    );
END
GO

IF OBJECT_ID(N'dbo.[User]', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.[User] (
        UserId UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
        RoleId INT NULL,
        FullName NVARCHAR(255) NULL,
        Phone NVARCHAR(12) NULL,
        Email NVARCHAR(255) NULL,
        Password NVARCHAR(MAX) NULL,
        CONSTRAINT FK_User_Role FOREIGN KEY (RoleId) REFERENCES dbo.Role(RoleId)
    );
END
GO

IF OBJECT_ID(N'dbo.Product', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Product (
        ProductId UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
        Type NVARCHAR(255) NULL,
        Name NVARCHAR(255) NULL,
        Quantity INT NULL,
        Price FLOAT NULL,
        Size NVARCHAR(100) NULL,
        Status NVARCHAR(50) NULL,
        ImageUrl VARCHAR(MAX) NULL
    );
END
GO

IF OBJECT_ID(N'dbo.[Order]', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.[Order] (
        OrderId UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
        UserId UNIQUEIDENTIFIER NULL,
        Address NVARCHAR(500) NULL,
        TotalAmount FLOAT NULL,
        CreateAt DATETIME NULL,
        Status NVARCHAR(50) NULL,
        CONSTRAINT FK_Order_User FOREIGN KEY (UserId) REFERENCES dbo.[User](UserId)
    );
END
GO

IF OBJECT_ID(N'dbo.OrderDetail', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.OrderDetail (
        OrderId UNIQUEIDENTIFIER NOT NULL,
        ProductId UNIQUEIDENTIFIER NOT NULL,
        Quantity INT NULL,
        Price FLOAT NULL,
        CONSTRAINT PK_OrderDetail PRIMARY KEY (OrderId, ProductId),
        CONSTRAINT FK_OrderDetail_Order FOREIGN KEY (OrderId) REFERENCES dbo.[Order](OrderId),
        CONSTRAINT FK_OrderDetail_Product FOREIGN KEY (ProductId) REFERENCES dbo.Product(ProductId)
    );
END
GO

IF OBJECT_ID(N'dbo.Cart', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Cart (
        CartId UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
        UserId UNIQUEIDENTIFIER NOT NULL UNIQUE,
        CreatedAt DATETIME NULL,
        CONSTRAINT FK_Cart_User FOREIGN KEY (UserId) REFERENCES dbo.[User](UserId)
    );
END
GO

IF OBJECT_ID(N'dbo.CartItem', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.CartItem (
        CartId UNIQUEIDENTIFIER NOT NULL,
        ProductId UNIQUEIDENTIFIER NOT NULL,
        Quantity INT NOT NULL,
        CONSTRAINT PK_CartItem PRIMARY KEY (CartId, ProductId),
        CONSTRAINT FK_CartItem_Cart FOREIGN KEY (CartId) REFERENCES dbo.Cart(CartId),
        CONSTRAINT FK_CartItem_Product FOREIGN KEY (ProductId) REFERENCES dbo.Product(ProductId)
    );
END
GO

-- Seed roles
IF NOT EXISTS (SELECT 1 FROM dbo.Role WHERE RoleId = 1)
    INSERT INTO dbo.Role (RoleId, Name) VALUES (1, N'Admin');
IF NOT EXISTS (SELECT 1 FROM dbo.Role WHERE RoleId = 2)
    INSERT INTO dbo.Role (RoleId, Name) VALUES (2, N'Staff');
IF NOT EXISTS (SELECT 1 FROM dbo.Role WHERE RoleId = 3)
    INSERT INTO dbo.Role (RoleId, Name) VALUES (3, N'Customer');
GO
