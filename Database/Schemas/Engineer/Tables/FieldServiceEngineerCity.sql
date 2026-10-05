CREATE TABLE [Engineer].[FieldServiceEngineerCity]
(
	[FieldServiceEngineerId] INT NOT NULL,
	[CityId] INT NOT NULL,
	CONSTRAINT [PK_FieldServiceEngineerCity]
		PRIMARY KEY CLUSTERED ([FieldServiceEngineerId], [CityId]),
	CONSTRAINT [FK_FieldServiceEngineerCity_FieldServiceEngineer]
		FOREIGN KEY ([FieldServiceEngineerId]) REFERENCES [Engineer].[FieldServiceEngineer] ([FieldServiceEngineerId]),
	CONSTRAINT [FK_FieldServiceEngineerCity_City]
		FOREIGN KEY ([CityId]) REFERENCES [Master].[City] ([CityId])
);
