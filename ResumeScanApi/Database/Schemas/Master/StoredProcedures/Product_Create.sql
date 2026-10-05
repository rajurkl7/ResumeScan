CREATE PROCEDURE [Master].[Product_Create]
	@Name NVARCHAR (200),
	@Price DECIMAL (18, 2),
	@Quantity INT
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [Master].[Products] ([Name], [Price], [Quantity])
	VALUES (@Name, @Price, @Quantity);

	SELECT CONVERT(INT, SCOPE_IDENTITY());
END;

