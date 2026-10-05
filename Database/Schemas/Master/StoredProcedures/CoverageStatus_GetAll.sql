CREATE PROCEDURE [Master].[CoverageStatus_GetAll]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [CoverageStatusId], [CoverageStatusName], [IsEnabled], [CreatedDateTime]
	FROM [Master].[CoverageStatus]
	ORDER BY [CoverageStatusId];
END;
