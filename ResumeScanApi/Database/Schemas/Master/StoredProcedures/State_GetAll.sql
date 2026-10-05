CREATE PROCEDURE [Master].[State_GetAll]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [StateId], [StateName], [IsEnabled], [CreatedDateTime]
	FROM [Master].[State]
	ORDER BY [StateId];
END;
