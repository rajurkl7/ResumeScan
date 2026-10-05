CREATE PROCEDURE [Master].[City_Update]
	@CityId INT,
	@StateId INT,
	@CityName VARCHAR(100),
	@IsEnabled BIT
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [Master].[City]
	SET [StateId] = @StateId,
		[CityName] = @CityName,
		[IsEnabled] = @IsEnabled
	WHERE [CityId] = @CityId;

	SELECT CONVERT(INT, @@ROWCOUNT);
END;
