use CYPowerReportDB
go

--資料表中文說明
EXEC [dbo].[sp_ChineseDescription] 'dbo' , 'City',null,'縣市代碼資料檔' ;

--欄位中文說明
EXEC [dbo].[sp_ChineseDescription] 'dbo', 'City', 'CityID','縣市代碼';
