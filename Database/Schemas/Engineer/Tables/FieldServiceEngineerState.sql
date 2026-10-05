CREATE TABLE [Engineer].[FieldServiceEngineerState]
(
	[FieldServiceEngineerId] INT NOT NULL,
	[StateId] INT NOT NULL,
	CONSTRAINT [PK_FieldServiceEngineerState]
		PRIMARY KEY CLUSTERED ([FieldServiceEngineerId], [StateId]),
	CONSTRAINT [FK_FieldServiceEngineerState_FieldServiceEngineer]
		FOREIGN KEY ([FieldServiceEngineerId]) REFERENCES [Engineer].[FieldServiceEngineer] ([FieldServiceEngineerId]),
	CONSTRAINT [FK_FieldServiceEngineerState_State]
		FOREIGN KEY ([StateId]) REFERENCES [Master].[State] ([StateId])
);
