use CYPowerReportDB
go

IF OBJECT_ID(N'sp_getDescription') IS NOT NULL
    DROP PROCEDURE [dbo].sp_getDescription;
GO

CREATE PROCEDURE [dbo].sp_getDescription
(
	@strTableName nvarchar(100)
)
AS
	declare @strError nvarchar(max)='';

	BEGIN TRY

        SELECT
                A.TABLE_NAME                as '表格名稱',
                B.COLUMN_NAME               as 'ColEName',
                B.DATA_TYPE                 as '型態',
                Isnull(Case when B.CHARACTER_MAXIMUM_LENGTH='-1' Then 'Max' Else Cast(B.CHARACTER_MAXIMUM_LENGTH as varchar(10)) End,'') as '長度',
                Isnull(( SELECT value FROM fn_listextendedproperty (NULL, 'schema', 'dbo', 'table',a .TABLE_NAME, 'column', default) 
                        WHERE name ='MS_Description' and objtype= 'COLUMN' and objname Collate Chinese_Taiwan_Stroke_CI_AS=b .COLUMN_NAME
                ),'') as 'ColCName'
                    ,Case When Isnull(CONSTRAINT_NAME,'') != '' Then 'PK' Else '' End As '鍵'
            FROM
                INFORMATION_SCHEMA.TABLES A
                LEFT JOIN INFORMATION_SCHEMA.COLUMNS B ON (A.TABLE_NAME=B.TABLE_NAME)
                    Left outer join INFORMATION_SCHEMA.KEY_COLUMN_USAGE C on OBJECTPROPERTY(OBJECT_ID(constraint_name), 'IsPrimaryKey') = 1 And C.Table_Name =A.TABLE_NAME And C.COLUMN_NAME=B.COLUMN_NAME
            WHERE TABLE_TYPE ='BASE TABLE' And A.TABLE_NAME=@strTableName
            ORDER BY A.TABLE_NAME,B.ordinal_position

	END TRY
	BEGIN CATCH
		set @strError = 'ERROR_NUMBER：'+cast(isnull(ERROR_NUMBER(),'') AS varchar(100))
			+'，ERROR_SEVERITY：'+cast(isnull(ERROR_SEVERITY(),'') AS varchar(100))
				+'，ERROR_STATE：'+cast(isnull(ERROR_STATE(),'') AS VARCHAR(100))
				+'，ERROR_PROCEDURE：'+cast(isnull(ERROR_PROCEDURE(),'') AS VARCHAR(100))
				+'，ERROR_LINE：'+cast(isnull(ERROR_LINE(),'') AS VARCHAR(100))
				+'，ERROR_LINE：'+cast(isnull(ERROR_MESSAGE(),'') AS  VARCHAR(max))

		Insert into dbo.Err_Message_Txt(Err_Object,Err_Flag,Err_Txt,Err_CRDate) VALUES('sp_ChineseDescription','',@strError
				,getdate())

	END CATCH 
