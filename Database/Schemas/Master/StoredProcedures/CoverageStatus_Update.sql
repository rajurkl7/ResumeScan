CREATE PROCEDURE [Master].[CoverageStatus_Update]
	@CoverageStatusId INT,
	@CoverageStatusName VARCHAR(100),
	@IsEnabled BIT
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [Master].[CoverageStatus]
	SET [CoverageStatusName] = @CoverageStatusName,
		[IsEnabled] = @IsEnabled
	WHERE [CoverageStatusId] = @CoverageStatusId;

	SELECT CONVERT(INT, @@ROWCOUNT);
END;
