CREATE PROCEDURE [Master].[State_GetById]
	@StateId INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [StateId], [StateName], [IsEnabled], [CreatedDateTime]
	FROM [Master].[State]
	WHERE [StateId] = @StateId;
END;
