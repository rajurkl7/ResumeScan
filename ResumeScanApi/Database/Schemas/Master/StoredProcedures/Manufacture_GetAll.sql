CREATE PROCEDURE [Master].[Manufacture_GetAll]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [ManufactureId], [ManufactureName], [IsEnabled], [CreatedDateTime]
	FROM [Master].[Manufacture]
	ORDER BY [ManufactureId];
END;
