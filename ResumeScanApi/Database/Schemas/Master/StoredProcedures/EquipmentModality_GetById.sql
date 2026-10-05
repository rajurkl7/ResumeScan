CREATE PROCEDURE [Master].[EquipmentModality_GetById]
	@EquipmentModalityId INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [EquipmentModalityId], [EquipmentModalityName], [IsEnabled], [CreatedDateTime]
	FROM [Master].[EquipmentModality]
	WHERE [EquipmentModalityId] = @EquipmentModalityId;
END;
