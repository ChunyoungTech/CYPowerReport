use CYPowerReportDB
go

IF OBJECT_ID(N'AUDI_CaseBaseData_Log') IS NOT NULL
    DROP TRIGGER [dbo].AUDI_CaseBaseData_Log;
GO

CREATE TRIGGER [dbo].[AUDI_CaseBaseData_Log]
	ON [dbo].[CaseBaseData]
FOR INSERT,UPDATE,DELETE--目前應沒有DELETE，以防萬一
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @strAction VARCHAR(1);

	--BEGIN TRY

		SET @strAction=(CASE WHEN (EXISTS(SELECT 1 FROM INSERTED) AND EXISTS(SELECT 1 FROM DELETED)) THEN 'U'
			WHEN EXISTS(SELECT 1 FROM INSERTED) THEN 'I'
			WHEN EXISTS(SELECT 1 FROM DELETED) THEN 'D'
			ELSE NULL END);

		IF (@strAction='U')
			begin
				INSERT INTO CaseBaseData_Log
					(CBD_SEQ_ID
					,CBD_Case_Name,CBD_Case_Owner
					,CBD_Case_No,CBD_Equipment_Brand
					,CBD_County_ID,CBD_Address
					,CBD_GPS,CBD_KW
					,CBD_Slices,CBD_Module_Model
					,CBD_Bearing,CBD_Case_Type
					,CBD_Voltage_Type
					,Update_User,Update_Time)
				SELECT CBD_SEQ_ID
					,CBD_Case_Name,CBD_Case_Owner
					,CBD_Case_No,CBD_Equipment_Brand
					,CBD_County_ID,CBD_Address
					,CBD_GPS,CBD_KW
					,CBD_Slices,CBD_Module_Model
					,CBD_Bearing,CBD_Case_Type
					,CBD_Voltage_Type
					,Update_User,Update_Time
				FROM DELETED
			end
		ELSE IF (@strAction='I')
			begin
				INSERT INTO CaseBaseData_Log
					(CBD_SEQ_ID
					,CBD_Case_Name,CBD_Case_Owner
					,CBD_Case_No,CBD_Equipment_Brand
					,CBD_County_ID,CBD_Address
					,CBD_GPS,CBD_KW
					,CBD_Slices,CBD_Module_Model
					,CBD_Bearing,CBD_Case_Type
					,CBD_Voltage_Type
					,Update_User,Update_Time)
				SELECT CBD_SEQ_ID
					,CBD_Case_Name,CBD_Case_Owner
					,CBD_Case_No,CBD_Equipment_Brand
					,CBD_County_ID,CBD_Address
					,CBD_GPS,CBD_KW
					,CBD_Slices,CBD_Module_Model
					,CBD_Bearing,CBD_Case_Type
					,CBD_Voltage_Type
					,Update_User,Update_Time
				FROM INSERTED
			end

		ELSE IF (@strAction='D')
			begin
				INSERT INTO CaseBaseData_Log
					(CBD_SEQ_ID
					,CBD_Case_Name,CBD_Case_Owner
					,CBD_Case_No,CBD_Equipment_Brand
					,CBD_County_ID,CBD_Address
					,CBD_GPS,CBD_KW
					,CBD_Slices,CBD_Module_Model
					,CBD_Bearing,CBD_Case_Type
					,CBD_Voltage_Type
					,Update_User,Update_Time)
				SELECT CBD_SEQ_ID
					,CBD_Case_Name,CBD_Case_Owner
					,CBD_Case_No,CBD_Equipment_Brand
					,CBD_County_ID,CBD_Address
					,CBD_GPS,CBD_KW
					,CBD_Slices,CBD_Module_Model
					,CBD_Bearing,CBD_Case_Type
					,CBD_Voltage_Type
					,Update_User,Update_Time
				FROM DELETED
			end

END
GO

ALTER TABLE [dbo].CaseBaseData ENABLE TRIGGER [AUDI_CaseBaseData_Log]
GO
