CREATE TABLE [Master].[EquipmentModality]
(
	[EquipmentModalityId] INT NOT NULL,
	[EquipmentModalityName] VARCHAR(100) NOT NULL,
	[IsEnabled] BIT NOT NULL,
	[CreatedDateTime] DATETIME NOT NULL,
	CONSTRAINT [PK_Master.EquipmentModality] PRIMARY KEY CLUSTERED ([EquipmentModalityId] ASC)
);
