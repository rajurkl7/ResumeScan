CREATE PROCEDURE [Master].[State_Delete]
	@StateId INT
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [Master].[State]
	WHERE [StateId] = @StateId;

	SELECT CONVERT(INT, @@ROWCOUNT);
END;
