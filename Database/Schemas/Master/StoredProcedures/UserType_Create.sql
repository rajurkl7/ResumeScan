CREATE PROCEDURE [Master].[UserType_Create]
	@UserTypeId INT,
	@UserType VARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [Master].[UserType] ([UserTypeId], [UserType])
	VALUES (@UserTypeId, @UserType);

	SELECT [UserTypeId], [UserType] AS [UserTypeName]
	FROM [Master].[UserType]
	WHERE [UserTypeId] = @UserTypeId;
END;



