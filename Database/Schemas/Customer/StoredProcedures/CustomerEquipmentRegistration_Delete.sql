CREATE PROCEDURE [Customer].[CustomerEquipmentRegistration_Delete]
	@CustomerEquipmentRegistrationId INT
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [Customer].[CustomerEquipmentRegistration]
	WHERE [CustomerEquipmentRegistrationId] = @CustomerEquipmentRegistrationId;

	SELECT CONVERT(INT, @@ROWCOUNT);
END;
