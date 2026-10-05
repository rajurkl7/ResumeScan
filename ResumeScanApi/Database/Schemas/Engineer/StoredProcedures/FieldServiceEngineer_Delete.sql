CREATE PROCEDURE [Engineer].[FieldServiceEngineer_Delete]
	@FieldServiceEngineerId INT
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [Engineer].[FieldServiceEngineerDocument]
	WHERE [FieldServiceEngineerId] = @FieldServiceEngineerId;
	DELETE FROM [Engineer].[FieldServiceEngineerState]
	WHERE [FieldServiceEngineerId] = @FieldServiceEngineerId;
	DELETE FROM [Engineer].[FieldServiceEngineerCity]
	WHERE [FieldServiceEngineerId] = @FieldServiceEngineerId;
	DELETE FROM [Engineer].[FieldServiceEngineerCapability]
	WHERE [FieldServiceEngineerId] = @FieldServiceEngineerId;
	DELETE FROM [Engineer].[FieldServiceEngineer]
	WHERE [FieldServiceEngineerId] = @FieldServiceEngineerId;

	SELECT CONVERT(INT, @@ROWCOUNT);
END;
