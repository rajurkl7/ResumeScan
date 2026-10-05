CREATE PROCEDURE [Master].[CoverageStatus_Create]
	@CoverageStatusId INT,
	@CoverageStatusName VARCHAR(100),
	@IsEnabled BIT,
	@CreatedDateTime DATETIME
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [Master].[CoverageStatus] ([CoverageStatusId], [CoverageStatusName], [IsEnabled], [CreatedDateTime])
	VALUES (@CoverageStatusId, @CoverageStatusName, @IsEnabled, @CreatedDateTime);

	SELECT [CoverageStatusId], [CoverageStatusName], [IsEnabled], [CreatedDateTime]
	FROM [Master].[CoverageStatus]
	WHERE [CoverageStatusId] = @CoverageStatusId;
END;
