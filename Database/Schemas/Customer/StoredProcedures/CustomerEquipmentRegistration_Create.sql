CREATE PROCEDURE [Customer].[CustomerEquipmentRegistration_Create]
	@FacilityName VARCHAR(200),
	@CustomerContactName VARCHAR(100),
	@CustomerContactMobile VARCHAR(20),
	@PrimaryContactEmail VARCHAR(255),
	@Password NVARCHAR(MAX),
	@FacilityAddress VARCHAR(500),
	@EquipmentModalityId INT,
	@ManufactureId INT,
	@ModelIdentifier VARCHAR(100),
	@EquipmentSerialNumber VARCHAR(100),
	@SoftwareFirmwareVersion VARCHAR(50),
	@CoverageStatusId INT,
	@IsEnabled BIT,
	@CreatedDateTime DATETIME
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [Customer].[CustomerEquipmentRegistration]
	(
		[FacilityName], [CustomerContactName], [CustomerContactMobile], [PrimaryContactEmail],
		[Password], [FacilityAddress], [EquipmentModalityId], [ManufactureId], [ModelIdentifier],
		[EquipmentSerialNumber], [SoftwareFirmwareVersion], [CoverageStatusId], [IsEnabled], [CreatedDateTime]
	)
	VALUES
	(
		@FacilityName, @CustomerContactName, @CustomerContactMobile, @PrimaryContactEmail,
		@Password, @FacilityAddress, @EquipmentModalityId, @ManufactureId, @ModelIdentifier,
		@EquipmentSerialNumber, @SoftwareFirmwareVersion, @CoverageStatusId, @IsEnabled, @CreatedDateTime
	);

	SELECT *
	FROM [Customer].[CustomerEquipmentRegistration]
	WHERE [CustomerEquipmentRegistrationId] = CONVERT(INT, SCOPE_IDENTITY());
END;
