CREATE PROCEDURE [Master].[CoverageStatus_Delete]
	@CoverageStatusId INT
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [Master].[CoverageStatus]
	WHERE [CoverageStatusId] = @CoverageStatusId;

	SELECT CONVERT(INT, @@ROWCOUNT);
END;
