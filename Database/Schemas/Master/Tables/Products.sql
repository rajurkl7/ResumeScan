CREATE TABLE [Master].[Products]
(
	[Id] INT IDENTITY (1, 1) NOT NULL,
	[Name] NVARCHAR (200) NOT NULL,
	[Price] DECIMAL (18, 2) NOT NULL,
	[Quantity] INT NOT NULL,
	[CreatedUtc] DATETIME2 (3) CONSTRAINT [DF_Products_CreatedUtc] DEFAULT (SYSUTCDATETIME()) NOT NULL,
	[ModifiedUtc] DATETIME2 (3) NULL,
	CONSTRAINT [PK_Products] PRIMARY KEY CLUSTERED ([Id]),
	CONSTRAINT [CK_Products_Price_NonNegative] CHECK ([Price] >= 0),
	CONSTRAINT [CK_Products_Quantity_NonNegative] CHECK ([Quantity] >= 0)
);

