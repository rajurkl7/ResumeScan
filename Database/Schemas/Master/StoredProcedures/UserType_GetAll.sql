CREATE PROCEDURE [Master].[UserType_GetAll]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [UserTypeId], [UserType] AS [UserTypeName]
	FROM [Master].[UserType]
	ORDER BY [UserTypeId];
END;


