CREATE PROCEDURE [Master].[Product_GetById]
	@Id INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [Id], [Name], [Price], [Quantity]
	FROM [Master].[Products]
	WHERE [Id] = @Id;
END;

