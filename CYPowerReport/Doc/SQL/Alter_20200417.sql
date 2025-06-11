use CYPowerReportDB
go

--資料表中文說明
EXEC [dbo].[sp_ChineseDescription] 'dbo' , 'ElectricityBilling',null,'電費報表資料' ;

--欄位中文說明
EXEC [dbo].[sp_ChineseDescription] 'dbo', 'ElectricityBilling', 'EC_SEQ_ID','編號';
EXEC [dbo].[sp_ChineseDescription] 'dbo', 'ElectricityBilling', 'CBD_ID','案場名稱';
EXEC [dbo].[sp_ChineseDescription] 'dbo', 'ElectricityBilling', 'EC_Last_Meter_Reading','上次抄表日期';
EXEC [dbo].[sp_ChineseDescription] 'dbo', 'ElectricityBilling', 'EC_Current_Meter_Reading','本次抄表日期';
EXEC [dbo].[sp_ChineseDescription] 'dbo', 'ElectricityBilling', 'EC_Next_Meter_Reading','下次抄表日期';

EXEC [dbo].[sp_ChineseDescription] 'dbo', 'ElectricityBilling', 'EC_Days','天數';
EXEC [dbo].[sp_ChineseDescription] 'dbo', 'ElectricityBilling', 'EC_Meter_Record','抄表紀錄公司購電度數';
EXEC [dbo].[sp_ChineseDescription] 'dbo', 'ElectricityBilling', 'EC_Calculation_Rate','電費計算費率';
EXEC [dbo].[sp_ChineseDescription] 'dbo', 'ElectricityBilling', 'EC_Calculation_Record','電費計算計費度數';
EXEC [dbo].[sp_ChineseDescription] 'dbo', 'ElectricityBilling', 'EC_Calculation_Amt','電費計算金額';

EXEC [dbo].[sp_ChineseDescription] 'dbo', 'ElectricityBilling', 'EC_Daily_Amount','日金額';
EXEC [dbo].[sp_ChineseDescription] 'dbo', 'ElectricityBilling', 'EC_Duarantee_Rate','日保發度數';
EXEC [dbo].[sp_ChineseDescription] 'dbo', 'ElectricityBilling', 'EC_Daily_Billing','日計費度數';
EXEC [dbo].[sp_ChineseDescription] 'dbo', 'ElectricityBilling', 'EC_Check_Amount','驗算';
EXEC [dbo].[sp_ChineseDescription] 'dbo', 'ElectricityBilling', 'EC_Line_Loss','線路損失率';

EXEC [dbo].[sp_ChineseDescription] 'dbo', 'ElectricityBilling', 'Update_User','更新人員';
EXEC [dbo].[sp_ChineseDescription] 'dbo', 'ElectricityBilling', 'Update_Time','更新時間';

--IF EXISTS(SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES
--WHERE TABLE_NAME = 'ElectricityBilling_Log')
--	DROP TABLE ElectricityBilling_Log;
CREATE TABLE ElectricityBilling_Log(
	EC_SEQ_ID int NOT NULL,					/*編號*/
	CBD_ID int NOT NULL,					/*案場項次*/
	EC_Last_Meter_Reading date NULL,		/*上次抄表日期*/
	EC_Current_Meter_Reading date NULL,		/*本次抄表日期*/
	EC_Next_Meter_Reading date NULL,		/*下次抄表日期*/

	EC_Days int NULL,						/*天數*/
	EC_Meter_Record int NULL,				/*抄表紀錄 公司購電-度數*/
	EC_Calculation_Rate decimal(18, 3) NULL,/*電費計算費率*/
	EC_Calculation_Record int NULL,			/*電費計算 計費度數*/
	EC_Calculation_Amt decimal(18, 3) NULL,	/*電費計算金額*/
		
	EC_Daily_Amount decimal(18, 3) NULL,	/*日金額*/
	EC_Duarantee_Rate decimal(18, 3) NULL,	/*日保發度數*/
	EC_Daily_Billing decimal(18, 3) NULL,	/*日計費度數*/
	EC_Check_Amount decimal(18, 3) NULL,	/*驗算*/
	EC_Line_Loss decimal(18, 4) NULL,		/*線路損失率*/

	Update_User int NULL,					/*更新人員*/
	Update_Time datetime NULL				/*更新時間*/
);
go

--資料表中文說明
EXEC [dbo].[sp_ChineseDescription] 'dbo' , 'ElectricityBilling_Log',null,'電費報表資料Log檔' ;

--欄位中文說明
EXEC [dbo].[sp_ChineseDescription] 'dbo', 'ElectricityBilling_Log', 'EC_SEQ_ID','編號';
EXEC [dbo].[sp_ChineseDescription] 'dbo', 'ElectricityBilling_Log', 'CBD_ID','案場名稱';
EXEC [dbo].[sp_ChineseDescription] 'dbo', 'ElectricityBilling_Log', 'EC_Last_Meter_Reading','上次抄表日期';
EXEC [dbo].[sp_ChineseDescription] 'dbo', 'ElectricityBilling_Log', 'EC_Current_Meter_Reading','本次抄表日期';
EXEC [dbo].[sp_ChineseDescription] 'dbo', 'ElectricityBilling_Log', 'EC_Next_Meter_Reading','下次抄表日期';

EXEC [dbo].[sp_ChineseDescription] 'dbo', 'ElectricityBilling_Log', 'EC_Days','天數';
EXEC [dbo].[sp_ChineseDescription] 'dbo', 'ElectricityBilling_Log', 'EC_Meter_Record','抄表紀錄公司購電度數';
EXEC [dbo].[sp_ChineseDescription] 'dbo', 'ElectricityBilling_Log', 'EC_Calculation_Rate','電費計算費率';
EXEC [dbo].[sp_ChineseDescription] 'dbo', 'ElectricityBilling_Log', 'EC_Calculation_Record','電費計算計費度數';
EXEC [dbo].[sp_ChineseDescription] 'dbo', 'ElectricityBilling_Log', 'EC_Calculation_Amt','電費計算金額';

EXEC [dbo].[sp_ChineseDescription] 'dbo', 'ElectricityBilling_Log', 'EC_Daily_Amount','日金額';
EXEC [dbo].[sp_ChineseDescription] 'dbo', 'ElectricityBilling_Log', 'EC_Duarantee_Rate','日保發度數';
EXEC [dbo].[sp_ChineseDescription] 'dbo', 'ElectricityBilling_Log', 'EC_Daily_Billing','日計費度數';
EXEC [dbo].[sp_ChineseDescription] 'dbo', 'ElectricityBilling_Log', 'EC_Check_Amount','驗算';
EXEC [dbo].[sp_ChineseDescription] 'dbo', 'ElectricityBilling_Log', 'EC_Line_Loss','線路損失率';

EXEC [dbo].[sp_ChineseDescription] 'dbo', 'ElectricityBilling_Log', 'Update_User','更新人員';
EXEC [dbo].[sp_ChineseDescription] 'dbo', 'ElectricityBilling_Log', 'Update_Time','更新時間';


