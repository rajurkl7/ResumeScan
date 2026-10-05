CREATE PROCEDURE [Customer].[CustomerEquipmentRegistration_GetAll]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT *
	FROM [Customer].[CustomerEquipmentRegistration]
	ORDER BY [CustomerEquipmentRegistrationId];
END;
