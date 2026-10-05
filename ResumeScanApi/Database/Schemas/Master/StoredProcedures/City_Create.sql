CREATE PROCEDURE [Master].[City_Create]
	@CityId INT,
	@StateId INT,
	@CityName VARCHAR(100),
	@IsEnabled BIT,
	@CreatedDateTime DATETIME
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [Master].[City] ([CityId], [StateId], [CityName], [IsEnabled], [CreatedDateTime])
	VALUES (@CityId, @StateId, @CityName, @IsEnabled, @CreatedDateTime);

	SELECT c.[CityId], c.[StateId], s.[StateName], c.[CityName], c.[IsEnabled], c.[CreatedDateTime]
	FROM [Master].[City] c
	INNER JOIN [Master].[State] s ON s.[StateId] = c.[StateId]
	WHERE c.[CityId] = @CityId;
END;
