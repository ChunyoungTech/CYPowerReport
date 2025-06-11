use CYPowerReportDB
go

--資料表中文說明
EXEC [dbo].[sp_ChineseDescription] 'dbo' , 'CutomerData',null,'業主基本資料' ;

--欄位中文說明
EXEC [dbo].[sp_ChineseDescription] 'dbo', 'CutomerData', 'CD_SEQ_ID','編號';
EXEC [dbo].[sp_ChineseDescription] 'dbo', 'CutomerData', 'CD_NAME','名稱';
EXEC [dbo].[sp_ChineseDescription] 'dbo', 'CutomerData', 'CD_ADDRESS','地址';
EXEC [dbo].[sp_ChineseDescription] 'dbo', 'CutomerData', 'CD_Uniform_Numbers','統編';
EXEC [dbo].[sp_ChineseDescription] 'dbo', 'CutomerData', 'CD_Tel','電話';

EXEC [dbo].[sp_ChineseDescription] 'dbo', 'CutomerData', 'CD_EMail','E-mail';
EXEC [dbo].[sp_ChineseDescription] 'dbo', 'CutomerData', 'CD_Contact','聯絡人';
EXEC [dbo].[sp_ChineseDescription] 'dbo', 'CutomerData', 'Update_User','更新人員';
EXEC [dbo].[sp_ChineseDescription] 'dbo', 'CutomerData', 'Update_Time','更新日期';
EXEC [dbo].[sp_ChineseDescription] 'dbo', 'CutomerData', 'CD_TYPE','類別';

CREATE TABLE CutomerData_Log(
	CD_SEQ_ID int NOT NULL,					/*編號*/
	CD_NAME nvarchar(100) NOT NULL,			/*名稱*/
	CD_ADDRESS nvarchar(100) NULL,			/*地址*/
	CD_Uniform_Numbers char(10) NULL,		/*統編*/
	CD_Tel nvarchar(50) NULL,				/*電話*/

	CD_EMail nvarchar(100) NULL,			/*E-mail*/
	CD_Contact nvarchar(100) NULL,			/*聯絡人*/
	Update_User int NULL,					/*更新人員*/
	Update_Time datetime NULL,				/*更新日期*/
	CD_TYPE nvarchar(50) NULL				/**/
);
go

--資料表中文說明
EXEC [dbo].[sp_ChineseDescription] 'dbo' , 'CutomerData_Log',null,'業主基本資料Log檔' ;

--欄位中文說明
EXEC [dbo].[sp_ChineseDescription] 'dbo', 'CutomerData_Log', 'CD_SEQ_ID','編號';
EXEC [dbo].[sp_ChineseDescription] 'dbo', 'CutomerData_Log', 'CD_NAME','名稱';
EXEC [dbo].[sp_ChineseDescription] 'dbo', 'CutomerData_Log', 'CD_ADDRESS','地址';
EXEC [dbo].[sp_ChineseDescription] 'dbo', 'CutomerData_Log', 'CD_Uniform_Numbers','統編';
EXEC [dbo].[sp_ChineseDescription] 'dbo', 'CutomerData_Log', 'CD_Tel','電話';

EXEC [dbo].[sp_ChineseDescription] 'dbo', 'CutomerData_Log', 'CD_EMail','E-mail';
EXEC [dbo].[sp_ChineseDescription] 'dbo', 'CutomerData_Log', 'CD_Contact','聯絡人';
EXEC [dbo].[sp_ChineseDescription] 'dbo', 'CutomerData_Log', 'Update_User','更新人員';
EXEC [dbo].[sp_ChineseDescription] 'dbo', 'CutomerData_Log', 'Update_Time','更新日期';
EXEC [dbo].[sp_ChineseDescription] 'dbo', 'CutomerData_Log', 'CD_TYPE','類別';

--資料表中文說明
EXEC [dbo].[sp_ChineseDescription] 'dbo' , 'CaseBaseData',null,'案場資料Log檔' ;

--欄位中文說明
EXEC [dbo].[sp_ChineseDescription] 'dbo', 'CaseBaseData', 'CBD_SEQ_ID','項次';
EXEC [dbo].[sp_ChineseDescription] 'dbo', 'CaseBaseData', 'CBD_Case_Name','案場名稱';
EXEC [dbo].[sp_ChineseDescription] 'dbo', 'CaseBaseData', 'CBD_Case_Owner','案場業主';
EXEC [dbo].[sp_ChineseDescription] 'dbo', 'CaseBaseData', 'CBD_Case_No','案場代碼';
EXEC [dbo].[sp_ChineseDescription] 'dbo', 'CaseBaseData', 'CBD_Equipment_Brand','監控廠商';

EXEC [dbo].[sp_ChineseDescription] 'dbo', 'CaseBaseData', 'CBD_County_ID','縣市';
EXEC [dbo].[sp_ChineseDescription] 'dbo', 'CaseBaseData', 'CBD_Address','住址';
EXEC [dbo].[sp_ChineseDescription] 'dbo', 'CaseBaseData', 'CBD_GPS','定位點';
EXEC [dbo].[sp_ChineseDescription] 'dbo', 'CaseBaseData', 'CBD_KW','kw數';
EXEC [dbo].[sp_ChineseDescription] 'dbo', 'CaseBaseData', 'CBD_Slices','片數';

EXEC [dbo].[sp_ChineseDescription] 'dbo', 'CaseBaseData', 'CBD_Module_Model','模組型號';
EXEC [dbo].[sp_ChineseDescription] 'dbo', 'CaseBaseData', 'CBD_Bearing','方位';
EXEC [dbo].[sp_ChineseDescription] 'dbo', 'CaseBaseData', 'CBD_Case_Type','案場類型';
EXEC [dbo].[sp_ChineseDescription] 'dbo', 'CaseBaseData', 'CBD_Voltage_Type','電壓型態';
EXEC [dbo].[sp_ChineseDescription] 'dbo', 'CaseBaseData', 'Update_User','更新人員';

EXEC [dbo].[sp_ChineseDescription] 'dbo', 'CaseBaseData', 'Update_Time','更新時間';

CREATE TABLE CaseBaseData_Log(
	CBD_SEQ_ID int NOT NULL,				/*項次*/
	CBD_Case_Name nvarchar(50) NOT NULL,	/*案場名稱*/
	CBD_Case_Owner int NOT NULL,			/*案場業主*/
	CBD_Case_No nvarchar(50) NULL,			/*案場代碼*/
	CBD_Equipment_Brand nvarchar(50) NULL,	/*監控廠商*/

	CBD_County_ID nvarchar(4) NOT NULL,		/*縣市*/
	CBD_Address nvarchar(255) NOT NULL,		/*住址*/
	CBD_GPS varchar(50) NULL,				/*定位點*/
	CBD_KW decimal(18, 3) NULL,				/*kw數*/
	CBD_Slices int NULL,					/*片數*/

	CBD_Module_Model int NULL,				/*模組型號*/
	CBD_Bearing nvarchar(50) NULL,			/*方位*/
	CBD_Case_Type nvarchar(50) NULL,		/*案場類型*/
	CBD_Voltage_Type nvarchar(50) NULL,		/*電壓型態*/
	Update_User int NULL,					/*更新人員*/

	Update_Time datetime NULL				/*更新時間*/
);
go

--資料表中文說明
EXEC [dbo].[sp_ChineseDescription] 'dbo' , 'CaseBaseData_Log',null,'案場資料Log檔' ;

--欄位中文說明
EXEC [dbo].[sp_ChineseDescription] 'dbo', 'CaseBaseData_Log', 'CBD_SEQ_ID','項次';
EXEC [dbo].[sp_ChineseDescription] 'dbo', 'CaseBaseData_Log', 'CBD_Case_Name','案場名稱';
EXEC [dbo].[sp_ChineseDescription] 'dbo', 'CaseBaseData_Log', 'CBD_Case_Owner','案場業主';
EXEC [dbo].[sp_ChineseDescription] 'dbo', 'CaseBaseData_Log', 'CBD_Case_No','案場代碼';
EXEC [dbo].[sp_ChineseDescription] 'dbo', 'CaseBaseData_Log', 'CBD_Equipment_Brand','監控廠商';

EXEC [dbo].[sp_ChineseDescription] 'dbo', 'CaseBaseData_Log', 'CBD_County_ID','縣市';
EXEC [dbo].[sp_ChineseDescription] 'dbo', 'CaseBaseData_Log', 'CBD_Address','住址';
EXEC [dbo].[sp_ChineseDescription] 'dbo', 'CaseBaseData_Log', 'CBD_GPS','定位點';
EXEC [dbo].[sp_ChineseDescription] 'dbo', 'CaseBaseData_Log', 'CBD_KW','kw數';
EXEC [dbo].[sp_ChineseDescription] 'dbo', 'CaseBaseData_Log', 'CBD_Slices','片數';

EXEC [dbo].[sp_ChineseDescription] 'dbo', 'CaseBaseData_Log', 'CBD_Module_Model','模組型號';
EXEC [dbo].[sp_ChineseDescription] 'dbo', 'CaseBaseData_Log', 'CBD_Bearing','方位';
EXEC [dbo].[sp_ChineseDescription] 'dbo', 'CaseBaseData_Log', 'CBD_Case_Type','案場類型';
EXEC [dbo].[sp_ChineseDescription] 'dbo', 'CaseBaseData_Log', 'CBD_Voltage_Type','電壓型態';
EXEC [dbo].[sp_ChineseDescription] 'dbo', 'CaseBaseData_Log', 'Update_User','更新人員';

EXEC [dbo].[sp_ChineseDescription] 'dbo', 'CaseBaseData_Log', 'Update_Time','更新時間';
