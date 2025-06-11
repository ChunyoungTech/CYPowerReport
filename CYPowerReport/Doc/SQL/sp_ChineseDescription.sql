use CYPowerReportDB
go

IF OBJECT_ID(N'sp_ChineseDescription') IS NOT NULL
    DROP PROCEDURE [dbo].sp_ChineseDescription;
GO

CREATE PROCEDURE [dbo].sp_ChineseDescription
     (@strSchemaName nvarchar(100) = NULL
     , @strTableName nvarchar(100) = NULL
     , @strColumnName nvarchar(100) = NULL
	 ,@strDescription nvarchar(1000)
     )
AS
	DECLARE @schmaType nvarchar(10) = N'SCHEMA';
	DECLARE @tableType nvarchar(10) = N'TABLE';
	DECLARE @columnType nvarchar(10) = N'COLUMN';
	declare @strError nvarchar(max)='';

	BEGIN TRY

		IF (@strColumnName IS NULL)
			BEGIN
				 IF OBJECT_ID(QUOTENAME(@strSchemaName) + '.' + QUOTENAME(@strTableName)) IS NULL
					 RAISERROR ('查無此資料表', -- Message text.
						   16, -- Severity.
						   1 -- State.
						   );
				 SET @columnType = NULL;
				 SET @strColumnName = NULL;
			END

		--檢查是否已存在描述(有:update, 無:insert)
		IF NOT EXISTS (
			SELECT * FROM ::fn_listextENDedproperty (default, @schmaType, @strSchemaName, @tableType, @strTableName, @columnType, @strColumnName)
		) 
		BEGIN 
			EXEC sp_addextENDedproperty N'MS_Description', @strDescription
				, @schmaType, @strSchemaName
				, @tableType, @strTableName
				, @columnType, @strColumnName;
		END 
		ELSE 
		BEGIN 
			EXEC sp_updateextENDedproperty N'MS_Description', @strDescription
				, @schmaType, @strSchemaName
				, @tableType, @strTableName
				, @columnType, @strColumnName;
		END


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

GO