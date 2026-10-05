CREATE PROCEDURE [Master].[Manufacture_Create]
	@ManufactureId INT,
	@ManufactureName VARCHAR(50),
	@IsEnabled BIT,
	@CreatedDateTime DATETIME
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [Master].[Manufacture] ([ManufactureId], [ManufactureName], [IsEnabled], [CreatedDateTime])
	VALUES (@ManufactureId, @ManufactureName, @IsEnabled, @CreatedDateTime);

	SELECT [ManufactureId], [ManufactureName], [IsEnabled], [CreatedDateTime]
	FROM [Master].[Manufacture]
	WHERE [ManufactureId] = @ManufactureId;
END;
