CREATE PROCEDURE [Master].[CoverageStatus_GetById]
	@CoverageStatusId INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [CoverageStatusId], [CoverageStatusName], [IsEnabled], [CreatedDateTime]
	FROM [Master].[CoverageStatus]
	WHERE [CoverageStatusId] = @CoverageStatusId;
END;
