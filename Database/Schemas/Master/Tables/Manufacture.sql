CREATE TABLE [Master].[Manufacture]
(
	[ManufactureId] [int] NOT NULL,
	[ManufactureName] [varchar](50) NOT NULL,
	[IsEnabled] [bit] NOT NULL,
	[CreatedDateTime] [datetime] NOT NULL,
	CONSTRAINT [PK_Master.Manufacture] PRIMARY KEY CLUSTERED ([ManufactureId] ASC)
);

