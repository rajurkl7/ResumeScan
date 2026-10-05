CREATE PROCEDURE [Engineer].[FieldServiceEngineer_Update]
	@FieldServiceEngineerId INT,
	@EngineerName VARCHAR(100),
	@BaseLocation VARCHAR(200),
	@MobileNumber VARCHAR(20),
	@EmailAddress VARCHAR(255),
	@PasswordHash NVARCHAR(500),
	@EmploymentStatus VARCHAR(20),
	@IsEnabled BIT,
	@CapabilitiesJson NVARCHAR(MAX),
	@StatesJson NVARCHAR(MAX),
	@CitiesJson NVARCHAR(MAX),
	@DocumentsJson NVARCHAR(MAX)
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [Engineer].[FieldServiceEngineer]
	SET [EngineerName] = @EngineerName,
		[BaseLocation] = @BaseLocation,
		[MobileNumber] = @MobileNumber,
		[EmailAddress] = @EmailAddress,
		[PasswordHash] = @PasswordHash,
		[EmploymentStatus] = @EmploymentStatus,
		[IsEnabled] = @IsEnabled,
		[ModifiedDateTime] = GETDATE()
	WHERE [FieldServiceEngineerId] = @FieldServiceEngineerId;
	DECLARE @Updated INT = CONVERT(INT, @@ROWCOUNT);

	DELETE FROM Engineer.FieldServiceEngineerCapability WHERE FieldServiceEngineerId = @FieldServiceEngineerId;
	INSERT INTO Engineer.FieldServiceEngineerCapability (FieldServiceEngineerId, EquipmentModalityId, ManufactureId)
	SELECT @FieldServiceEngineerId, EquipmentModalityId, ManufactureId FROM OPENJSON(@CapabilitiesJson) WITH (EquipmentModalityId INT, ManufactureId INT);
	DELETE FROM Engineer.FieldServiceEngineerState WHERE FieldServiceEngineerId = @FieldServiceEngineerId;
	INSERT INTO Engineer.FieldServiceEngineerState (FieldServiceEngineerId, StateId)
	SELECT @FieldServiceEngineerId, StateId FROM OPENJSON(@StatesJson) WITH (StateId INT);
	DELETE FROM Engineer.FieldServiceEngineerCity WHERE FieldServiceEngineerId = @FieldServiceEngineerId;
	INSERT INTO Engineer.FieldServiceEngineerCity (FieldServiceEngineerId, CityId)
	SELECT @FieldServiceEngineerId, CityId FROM OPENJSON(@CitiesJson) WITH (CityId INT);
	DELETE FROM Engineer.FieldServiceEngineerDocument WHERE FieldServiceEngineerId = @FieldServiceEngineerId;
	INSERT INTO Engineer.FieldServiceEngineerDocument (FieldServiceEngineerId, DocumentType, OriginalFileName, StoredFilePath, ContentType, FileSizeBytes)
	SELECT @FieldServiceEngineerId, DocumentType, OriginalFileName, StoredFilePath, ContentType, FileSizeBytes FROM OPENJSON(@DocumentsJson) WITH (DocumentType VARCHAR(20), OriginalFileName VARCHAR(255), StoredFilePath VARCHAR(1000), ContentType VARCHAR(100), FileSizeBytes BIGINT);

	SELECT @Updated;
END;
