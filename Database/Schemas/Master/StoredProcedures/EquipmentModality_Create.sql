CREATE PROCEDURE [Master].[EquipmentModality_Create]
	@EquipmentModalityId INT,
	@EquipmentModalityName VARCHAR(100),
	@IsEnabled BIT,
	@CreatedDateTime DATETIME
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [Master].[EquipmentModality] ([EquipmentModalityId], [EquipmentModalityName], [IsEnabled], [CreatedDateTime])
	VALUES (@EquipmentModalityId, @EquipmentModalityName, @IsEnabled, @CreatedDateTime);

	SELECT [EquipmentModalityId], [EquipmentModalityName], [IsEnabled], [CreatedDateTime]
	FROM [Master].[EquipmentModality]
	WHERE [EquipmentModalityId] = @EquipmentModalityId;
END;
