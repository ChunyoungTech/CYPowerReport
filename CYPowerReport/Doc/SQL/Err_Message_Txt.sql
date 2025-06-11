use CYPowerReportDB
go

IF EXISTS(SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_NAME = 'Err_Message_Txt')
	DROP TABLE Err_Message_Txt;

CREATE TABLE [dbo].Err_Message_Txt(
	[Err_Object] [varchar](100) NULL,
	[Err_Flag] [nvarchar](50) NULL,
	[Err_Txt] [nvarchar](500) NULL,
	[Err_CRDate] [datetime] NULL
) ON [PRIMARY]

GO