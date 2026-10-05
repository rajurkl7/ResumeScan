CREATE PROCEDURE [Customer].[CustomerEquipmentRegistration_GetById]
	@CustomerEquipmentRegistrationId INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT *
	FROM [Customer].[CustomerEquipmentRegistration]
	WHERE [CustomerEquipmentRegistrationId] = @CustomerEquipmentRegistrationId;
END;
