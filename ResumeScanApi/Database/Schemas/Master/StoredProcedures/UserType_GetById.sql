CREATE PROCEDURE [Master].[UserType_GetById]
	@UserTypeId INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [UserTypeId], [UserType] AS [UserTypeName]
	FROM [Master].[UserType]
	WHERE [UserTypeId] = @UserTypeId;
END;


