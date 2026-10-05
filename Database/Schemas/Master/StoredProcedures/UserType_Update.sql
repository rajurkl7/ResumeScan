CREATE PROCEDURE [Master].[UserType_Update]
	@UserTypeId INT,
	@UserType VARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [Master].[UserType]
	SET [UserType] = @UserType
	WHERE [UserTypeId] = @UserTypeId;

	SELECT CONVERT(INT, @@ROWCOUNT);
END;

