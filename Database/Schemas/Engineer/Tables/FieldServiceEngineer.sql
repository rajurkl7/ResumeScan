CREATE TABLE [Engineer].[FieldServiceEngineer]
(
	[FieldServiceEngineerId] INT IDENTITY(1,1) NOT NULL,
	[EngineerName] VARCHAR(100) NOT NULL,
	[BaseLocation] VARCHAR(200) NOT NULL,
	[MobileNumber] VARCHAR(20) NOT NULL,
	[EmailAddress] VARCHAR(255) NOT NULL,
	[PasswordHash] NVARCHAR(500) NOT NULL,
	[EmploymentStatus] VARCHAR(20) NOT NULL,
	[IsEnabled] BIT NOT NULL
		CONSTRAINT [DF_FieldServiceEngineer_IsEnabled] DEFAULT ((1)),
	[CreatedDateTime] DATETIME NOT NULL
		CONSTRAINT [DF_FieldServiceEngineer_CreatedDateTime] DEFAULT (GETDATE()),
	[ModifiedDateTime] DATETIME NOT NULL
		CONSTRAINT [DF_FieldServiceEngineer_ModifiedDateTime] DEFAULT (GETDATE()),
	CONSTRAINT [PK_FieldServiceEngineer]
		PRIMARY KEY CLUSTERED ([FieldServiceEngineerId] ASC),
	CONSTRAINT [UQ_FieldServiceEngineer_Email]
		UNIQUE NONCLUSTERED ([EmailAddress] ASC),
	CONSTRAINT [UQ_FieldServiceEngineer_Mobile]
		UNIQUE NONCLUSTERED ([MobileNumber] ASC),
	CONSTRAINT [CK_FieldServiceEngineer_EmploymentStatus]
		CHECK ([EmploymentStatus] = 'Part-Time' OR [EmploymentStatus] = 'Full-Time' OR [EmploymentStatus] = 'Freelancer')
);
