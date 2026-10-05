CREATE PROCEDURE [Engineer].[FieldServiceEngineer_Create]
	@EngineerName VARCHAR(100),
	@BaseLocation VARCHAR(200),
	@MobileNumber VARCHAR(20),
	@EmailAddress VARCHAR(255),
	@PasswordHash NVARCHAR(500),
	@EmploymentStatus VARCHAR(20),
	@IsEnabled BIT,
	@CreatedDateTime DATETIME,
	@CapabilitiesJson NVARCHAR(MAX),
	@StatesJson NVARCHAR(MAX),
	@CitiesJson NVARCHAR(MAX),
	@DocumentsJson NVARCHAR(MAX)
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRANSACTION;

	INSERT INTO [Engineer].[FieldServiceEngineer]
	(
		[EngineerName], [BaseLocation], [MobileNumber], [EmailAddress], [PasswordHash],
		[EmploymentStatus], [IsEnabled], [CreatedDateTime]
	)
	VALUES
	(
		@EngineerName, @BaseLocation, @MobileNumber, @EmailAddress, @PasswordHash,
		@EmploymentStatus, @IsEnabled, @CreatedDateTime
	);

	DECLARE @Id INT = CONVERT(INT, SCOPE_IDENTITY());

	INSERT INTO [Engineer].[FieldServiceEngineerCapability] ([FieldServiceEngineerId], [EquipmentModalityId], [ManufactureId])
	SELECT @Id, [EquipmentModalityId], [ManufactureId]
	FROM OPENJSON(@CapabilitiesJson) WITH ([EquipmentModalityId] INT, [ManufactureId] INT);

	INSERT INTO [Engineer].[FieldServiceEngineerState] ([FieldServiceEngineerId], [StateId])
	SELECT @Id, [StateId]
	FROM OPENJSON(@StatesJson) WITH ([StateId] INT);

	INSERT INTO [Engineer].[FieldServiceEngineerCity] ([FieldServiceEngineerId], [CityId])
	SELECT @Id, [CityId]
	FROM OPENJSON(@CitiesJson) WITH ([CityId] INT);

	INSERT INTO [Engineer].[FieldServiceEngineerDocument] ([FieldServiceEngineerId], [DocumentType], [OriginalFileName], [StoredFilePath], [ContentType], [FileSizeBytes])
	SELECT @Id, [DocumentType], [OriginalFileName], [StoredFilePath], [ContentType], [FileSizeBytes]
	FROM OPENJSON(@DocumentsJson) WITH ([DocumentType] VARCHAR(20), [OriginalFileName] VARCHAR(255), [StoredFilePath] VARCHAR(1000), [ContentType] VARCHAR(100), [FileSizeBytes] BIGINT);

	COMMIT TRANSACTION;

	EXEC [Engineer].[FieldServiceEngineer_GetById] @FieldServiceEngineerId = @Id;
END;
