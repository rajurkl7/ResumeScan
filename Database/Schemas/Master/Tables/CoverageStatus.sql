CREATE TABLE [Master].[CoverageStatus]
(
	[CoverageStatusId] INT NOT NULL,
	[CoverageStatusName] VARCHAR(100) NOT NULL,
	[IsEnabled] BIT NOT NULL,
	[CreatedDateTime] DATETIME NOT NULL,
	CONSTRAINT [PK_Master.CoverageStatus] PRIMARY KEY CLUSTERED ([CoverageStatusId] ASC)
);
