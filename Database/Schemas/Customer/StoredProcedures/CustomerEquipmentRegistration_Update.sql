CREATE PROCEDURE [Customer].[CustomerEquipmentRegistration_Update]
	@CustomerEquipmentRegistrationId INT,
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
	@IsEnabled BIT
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [Customer].[CustomerEquipmentRegistration]
	SET [FacilityName] = @FacilityName,
		[CustomerContactName] = @CustomerContactName,
		[CustomerContactMobile] = @CustomerContactMobile,
		[PrimaryContactEmail] = @PrimaryContactEmail,
		[Password] = @Password,
		[FacilityAddress] = @FacilityAddress,
		[EquipmentModalityId] = @EquipmentModalityId,
		[ManufactureId] = @ManufactureId,
		[ModelIdentifier] = @ModelIdentifier,
		[EquipmentSerialNumber] = @EquipmentSerialNumber,
		[SoftwareFirmwareVersion] = @SoftwareFirmwareVersion,
		[CoverageStatusId] = @CoverageStatusId,
		[IsEnabled] = @IsEnabled,
		[ModifiedDateTime] = GETDATE()
	WHERE [CustomerEquipmentRegistrationId] = @CustomerEquipmentRegistrationId;

	SELECT CONVERT(INT, @@ROWCOUNT);
END;
