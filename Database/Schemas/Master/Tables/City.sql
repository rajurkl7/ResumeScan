CREATE TABLE [Master].[City]
(
	[CityId] INT NOT NULL,
	[StateId] INT NOT NULL,
	[CityName] VARCHAR(100) NOT NULL,
	[IsEnabled] BIT NOT NULL CONSTRAINT [DF_Master_City_IsEnabled] DEFAULT ((1)),
	[CreatedDateTime] DATETIME NOT NULL CONSTRAINT [DF_Master_City_CreatedDateTime] DEFAULT (GETDATE()),
	CONSTRAINT [PK_Master.City] PRIMARY KEY CLUSTERED ([CityId] ASC),
	CONSTRAINT [UQ_Master.City_State_CityName] UNIQUE NONCLUSTERED ([StateId], [CityName]),
	CONSTRAINT [FK_Master.City_State] FOREIGN KEY ([StateId]) REFERENCES [Master].[State] ([StateId])
);
