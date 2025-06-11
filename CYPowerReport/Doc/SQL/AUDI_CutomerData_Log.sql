use CYPowerReportDB
go

IF OBJECT_ID(N'AUDI_CutomerData_Log') IS NOT NULL
    DROP TRIGGER [dbo].AUDI_CutomerData_Log;
GO

CREATE TRIGGER [dbo].[AUDI_CutomerData_Log]
	ON [dbo].[CutomerData]
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
				INSERT INTO CutomerData_Log
					(CD_SEQ_ID
					,CD_NAME,CD_ADDRESS
					,CD_Uniform_Numbers,CD_Tel
					,CD_EMail,CD_Contact
					,Update_User,Update_Time
					,CD_TYPE)
				SELECT CD_SEQ_ID
					,CD_NAME,CD_ADDRESS
					,CD_Uniform_Numbers,CD_Tel
					,CD_EMail,CD_Contact
					,Update_User,Update_Time
					,CD_TYPE
				FROM DELETED
			end
		ELSE IF (@strAction='I')
			begin
				INSERT INTO CutomerData_Log
					(CD_SEQ_ID
					,CD_NAME,CD_ADDRESS
					,CD_Uniform_Numbers,CD_Tel
					,CD_EMail,CD_Contact
					,Update_User,Update_Time
					,CD_TYPE)
				SELECT CD_SEQ_ID
					,CD_NAME,CD_ADDRESS
					,CD_Uniform_Numbers,CD_Tel
					,CD_EMail,CD_Contact
					,Update_User,Update_Time
					,CD_TYPE
				FROM INSERTED
			end

		ELSE IF (@strAction='D')
			begin
				INSERT INTO CutomerData_Log
					(CD_SEQ_ID
					,CD_NAME,CD_ADDRESS
					,CD_Uniform_Numbers,CD_Tel
					,CD_EMail,CD_Contact
					,Update_User,Update_Time
					,CD_TYPE)
				SELECT CD_SEQ_ID
					,CD_NAME,CD_ADDRESS
					,CD_Uniform_Numbers,CD_Tel
					,CD_EMail,CD_Contact
					,Update_User,Update_Time
					,CD_TYPE
				FROM DELETED
			end

END
GO

ALTER TABLE [dbo].CutomerData ENABLE TRIGGER [AUDI_CutomerData_Log]
GO
