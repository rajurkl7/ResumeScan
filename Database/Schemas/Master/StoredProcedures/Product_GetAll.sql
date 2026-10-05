CREATE PROCEDURE [Master].[Product_GetAll]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [Id], [Name], [Price], [Quantity]
	FROM [Master].[Products]
	ORDER BY [Id];
END;

