CREATE PROCEDURE [Master].[Manufacture_GetById]
	@ManufactureId INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [ManufactureId], [ManufactureName], [IsEnabled], [CreatedDateTime]
	FROM [Master].[Manufacture]
	WHERE [ManufactureId] = @ManufactureId;
END;
