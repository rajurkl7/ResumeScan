CREATE PROCEDURE [Master].[Manufacture_Update]
	@ManufactureId INT,
	@ManufactureName VARCHAR(50),
	@IsEnabled BIT
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [Master].[Manufacture]
	SET [ManufactureName] = @ManufactureName,
		[IsEnabled] = @IsEnabled
	WHERE [ManufactureId] = @ManufactureId;

	SELECT CONVERT(INT, @@ROWCOUNT);
END;
