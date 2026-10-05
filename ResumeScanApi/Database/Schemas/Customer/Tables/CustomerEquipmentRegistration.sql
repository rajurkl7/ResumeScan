CREATE TABLE [Customer].[CustomerEquipmentRegistration]
(
	[CustomerEquipmentRegistrationId] INT IDENTITY(1,1) NOT NULL,
	[FacilityName] VARCHAR(200) NOT NULL,
	[CustomerContactName] VARCHAR(100) NOT NULL,
	[CustomerContactMobile] VARCHAR(20) NOT NULL,
	[PrimaryContactEmail] VARCHAR(255) NOT NULL,
	[Password] NVARCHAR(MAX) NOT NULL,
	[FacilityAddress] VARCHAR(500) NOT NULL,
	[EquipmentModalityId] INT NOT NULL,
	[ManufactureId] INT NOT NULL,
	[ModelIdentifier] VARCHAR(100) NOT NULL,
	[EquipmentSerialNumber] VARCHAR(100) NOT NULL,
	[SoftwareFirmwareVersion] VARCHAR(50) NULL,
	[CoverageStatusId] INT NOT NULL,
	[IsEnabled] BIT NOT NULL
		CONSTRAINT [DF_CustomerEquipmentRegistration_IsEnabled] DEFAULT ((1)),
	[CreatedDateTime] DATETIME NOT NULL
		CONSTRAINT [DF_CustomerEquipmentRegistration_CreatedDateTime] DEFAULT (GETDATE()),
	[ModifiedDateTime] DATETIME NOT NULL
		CONSTRAINT [DF_CustomerEquipmentRegistration_ModifiedDateTime] DEFAULT (GETDATE()),
	CONSTRAINT [PK_CustomerEquipmentRegistration]
		PRIMARY KEY CLUSTERED ([CustomerEquipmentRegistrationId] ASC),
	CONSTRAINT [UQ_CustomerEquipmentRegistration_Email]
		UNIQUE NONCLUSTERED ([PrimaryContactEmail] ASC),
	CONSTRAINT [UQ_CustomerEquipmentRegistration_SerialNumber]
		UNIQUE NONCLUSTERED ([EquipmentSerialNumber] ASC),
	CONSTRAINT [FK_CustomerEquipmentRegistration_CoverageStatus]
		FOREIGN KEY ([CoverageStatusId])
		REFERENCES [Master].[CoverageStatus] ([CoverageStatusId]),
	CONSTRAINT [FK_CustomerEquipmentRegistration_EquipmentModality]
		FOREIGN KEY ([EquipmentModalityId])
		REFERENCES [Master].[EquipmentModality] ([EquipmentModalityId]),
	CONSTRAINT [FK_CustomerEquipmentRegistration_Manufacture]
		FOREIGN KEY ([ManufactureId])
		REFERENCES [Master].[Manufacture] ([ManufactureId])
);
