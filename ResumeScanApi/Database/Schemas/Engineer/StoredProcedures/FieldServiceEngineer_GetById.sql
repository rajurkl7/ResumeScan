CREATE PROCEDURE [Engineer].[FieldServiceEngineer_GetById]
	@FieldServiceEngineerId INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT e.*,
		(SELECT EquipmentModalityId, ManufactureId FROM Engineer.FieldServiceEngineerCapability WHERE FieldServiceEngineerId = e.FieldServiceEngineerId FOR JSON PATH) AS CapabilitiesJson,
		(SELECT es.StateId, s.StateName FROM Engineer.FieldServiceEngineerState es INNER JOIN Master.State s ON s.StateId = es.StateId WHERE es.FieldServiceEngineerId = e.FieldServiceEngineerId FOR JSON PATH) AS StatesJson,
		(SELECT ec.CityId, c.CityName, c.StateId, s.StateName FROM Engineer.FieldServiceEngineerCity ec INNER JOIN Master.City c ON c.CityId = ec.CityId INNER JOIN Master.State s ON s.StateId = c.StateId WHERE ec.FieldServiceEngineerId = e.FieldServiceEngineerId FOR JSON PATH) AS CitiesJson,
		(SELECT FieldServiceEngineerDocumentId, DocumentType, OriginalFileName, StoredFilePath, ContentType, FileSizeBytes, UploadedDateTime FROM Engineer.FieldServiceEngineerDocument WHERE FieldServiceEngineerId = e.FieldServiceEngineerId FOR JSON PATH) AS DocumentsJson
	FROM [Engineer].[FieldServiceEngineer] e
	WHERE e.[FieldServiceEngineerId] = @FieldServiceEngineerId;
END;
