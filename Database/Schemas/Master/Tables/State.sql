CREATE TABLE [Master].[State]
(
	[StateId] INT NOT NULL,
	[StateName] VARCHAR(100) NOT NULL,
	[IsEnabled] BIT NOT NULL CONSTRAINT [DF_Master_State_IsEnabled] DEFAULT ((1)),
	[CreatedDateTime] DATETIME NOT NULL CONSTRAINT [DF_Master_State_CreatedDateTime] DEFAULT (GETDATE()),
	CONSTRAINT [PK_Master.State] PRIMARY KEY CLUSTERED ([StateId] ASC),
	CONSTRAINT [UQ_Master.State_StateName] UNIQUE NONCLUSTERED ([StateName] ASC)
);
