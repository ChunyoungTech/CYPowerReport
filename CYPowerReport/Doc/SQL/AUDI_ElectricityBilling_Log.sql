use CYPowerReportDB
go

IF OBJECT_ID(N'AUDI_ElectricityBilling_Log') IS NOT NULL
    DROP TRIGGER [dbo].AUDI_ElectricityBilling_Log;
GO

CREATE TRIGGER [dbo].[AUDI_ElectricityBilling_Log]
	ON [dbo].[ElectricityBilling]
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
				INSERT INTO ElectricityBilling_Log
					(EC_SEQ_ID,CBD_ID
					,EC_Last_Meter_Reading,EC_Current_Meter_Reading
					,EC_Next_Meter_Reading,EC_Days
					,EC_Meter_Record,EC_Calculation_Rate
					,EC_Calculation_Record,EC_Calculation_Amt
					,EC_Daily_Amount,EC_Duarantee_Rate
					,EC_Daily_Billing,EC_Check_Amount
					,EC_Line_Loss
					,Update_User,Update_Time)
				SELECT EC_SEQ_ID,CBD_ID
					,EC_Last_Meter_Reading,EC_Current_Meter_Reading
					,EC_Next_Meter_Reading,EC_Days
					,EC_Meter_Record,EC_Calculation_Rate
					,EC_Calculation_Record,EC_Calculation_Amt
					,EC_Daily_Amount,EC_Duarantee_Rate
					,EC_Daily_Billing,EC_Check_Amount
					,EC_Line_Loss
					,Update_User,Update_Time
				FROM DELETED
			end
		ELSE IF (@strAction='I')
			begin
				INSERT INTO ElectricityBilling_Log
					(EC_SEQ_ID,CBD_ID
					,EC_Last_Meter_Reading,EC_Current_Meter_Reading
					,EC_Next_Meter_Reading,EC_Days
					,EC_Meter_Record,EC_Calculation_Rate
					,EC_Calculation_Record,EC_Calculation_Amt
					,EC_Daily_Amount,EC_Duarantee_Rate
					,EC_Daily_Billing,EC_Check_Amount
					,EC_Line_Loss
					,Update_User,Update_Time)
				SELECT EC_SEQ_ID,CBD_ID
					,EC_Last_Meter_Reading,EC_Current_Meter_Reading
					,EC_Next_Meter_Reading,EC_Days
					,EC_Meter_Record,EC_Calculation_Rate
					,EC_Calculation_Record,EC_Calculation_Amt
					,EC_Daily_Amount,EC_Duarantee_Rate
					,EC_Daily_Billing,EC_Check_Amount
					,EC_Line_Loss
					,Update_User,Update_Time
				FROM INSERTED
			end

		ELSE IF (@strAction='D')
			begin
				INSERT INTO ElectricityBilling_Log
					(EC_SEQ_ID,CBD_ID
					,EC_Last_Meter_Reading,EC_Current_Meter_Reading
					,EC_Next_Meter_Reading,EC_Days
					,EC_Meter_Record,EC_Calculation_Rate
					,EC_Calculation_Record,EC_Calculation_Amt
					,EC_Daily_Amount,EC_Duarantee_Rate
					,EC_Daily_Billing,EC_Check_Amount
					,EC_Line_Loss
					,Update_User,Update_Time)
				SELECT EC_SEQ_ID,CBD_ID
					,EC_Last_Meter_Reading,EC_Current_Meter_Reading
					,EC_Next_Meter_Reading,EC_Days
					,EC_Meter_Record,EC_Calculation_Rate
					,EC_Calculation_Record,EC_Calculation_Amt
					,EC_Daily_Amount,EC_Duarantee_Rate
					,EC_Daily_Billing,EC_Check_Amount
					,EC_Line_Loss
					,Update_User,Update_Time
				FROM DELETED
			end

END
GO

ALTER TABLE [dbo].[ElectricityBilling] ENABLE TRIGGER [AUDI_ElectricityBilling_Log]
GO
