CREATE PROCEDURE [Master].[EquipmentModality_Delete]
	@EquipmentModalityId INT
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [Master].[EquipmentModality]
	WHERE [EquipmentModalityId] = @EquipmentModalityId;

	SELECT CONVERT(INT, @@ROWCOUNT);
END;
