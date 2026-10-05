CREATE PROCEDURE [Master].[City_GetById]
	@CityId INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT c.[CityId], c.[StateId], s.[StateName], c.[CityName], c.[IsEnabled], c.[CreatedDateTime]
	FROM [Master].[City] c
	INNER JOIN [Master].[State] s ON s.[StateId] = c.[StateId]
	WHERE c.[CityId] = @CityId;
END;
