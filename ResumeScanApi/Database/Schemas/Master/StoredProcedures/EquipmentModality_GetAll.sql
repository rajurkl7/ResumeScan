CREATE PROCEDURE [Master].[EquipmentModality_GetAll]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [EquipmentModalityId], [EquipmentModalityName], [IsEnabled], [CreatedDateTime]
	FROM [Master].[EquipmentModality]
	ORDER BY [EquipmentModalityId];
END;
