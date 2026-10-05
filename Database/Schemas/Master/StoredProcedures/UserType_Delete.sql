CREATE PROCEDURE [Master].[UserType_Delete]
	@UserTypeId INT
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [Master].[UserType]
	WHERE [UserTypeId] = @UserTypeId;

	SELECT CONVERT(INT, @@ROWCOUNT);
END;

