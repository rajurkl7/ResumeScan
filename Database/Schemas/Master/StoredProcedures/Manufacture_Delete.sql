CREATE PROCEDURE [Master].[Manufacture_Delete]
	@ManufactureId INT
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [Master].[Manufacture]
	WHERE [ManufactureId] = @ManufactureId;

	SELECT CONVERT(INT, @@ROWCOUNT);
END;
