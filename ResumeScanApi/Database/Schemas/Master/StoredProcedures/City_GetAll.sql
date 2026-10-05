CREATE PROCEDURE [Master].[City_GetAll]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT c.[CityId], c.[StateId], s.[StateName], c.[CityName], c.[IsEnabled], c.[CreatedDateTime]
	FROM [Master].[City] c
	INNER JOIN [Master].[State] s ON s.[StateId] = c.[StateId]
	ORDER BY c.[StateId], c.[CityName];
END;
