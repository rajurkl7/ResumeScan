CREATE PROCEDURE [Master].[State_Update]
	@StateId INT,
	@StateName VARCHAR(100),
	@IsEnabled BIT
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [Master].[State]
	SET [StateName] = @StateName,
		[IsEnabled] = @IsEnabled
	WHERE [StateId] = @StateId;

	SELECT CONVERT(INT, @@ROWCOUNT);
END;
