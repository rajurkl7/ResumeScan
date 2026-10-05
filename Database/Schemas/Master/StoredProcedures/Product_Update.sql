CREATE PROCEDURE [Master].[Product_Update]
	@Id INT,
	@Name NVARCHAR (200),
	@Price DECIMAL (18, 2),
	@Quantity INT
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [Master].[Products]
	SET [Name] = @Name,
		[Price] = @Price,
		[Quantity] = @Quantity,
		[ModifiedUtc] = SYSUTCDATETIME()
	WHERE [Id] = @Id;

	SELECT CONVERT(INT, @@ROWCOUNT);
END;

