CREATE PROCEDURE [Master].[State_Create]
	@StateId INT,
	@StateName VARCHAR(100),
	@IsEnabled BIT,
	@CreatedDateTime DATETIME
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [Master].[State] ([StateId], [StateName], [IsEnabled], [CreatedDateTime])
	VALUES (@StateId, @StateName, @IsEnabled, @CreatedDateTime);

	SELECT [StateId], [StateName], [IsEnabled], [CreatedDateTime]
	FROM [Master].[State]
	WHERE [StateId] = @StateId;
END;
