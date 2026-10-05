CREATE PROCEDURE [Master].[EquipmentModality_Update]
	@EquipmentModalityId INT,
	@EquipmentModalityName VARCHAR(100),
	@IsEnabled BIT
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [Master].[EquipmentModality]
	SET [EquipmentModalityName] = @EquipmentModalityName,
		[IsEnabled] = @IsEnabled
	WHERE [EquipmentModalityId] = @EquipmentModalityId;

	SELECT CONVERT(INT, @@ROWCOUNT);
END;
