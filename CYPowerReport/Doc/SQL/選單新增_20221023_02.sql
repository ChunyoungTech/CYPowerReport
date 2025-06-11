use CYPowerReportDB
go

insert into [SysProgSub]
	([Name],[Path]
	,[UpperID],[isShow])

select '½s¿è','Module_ModelEdit.aspx'
	,ID,0

	FROM [SysProg]
	where [Name]='¼Ò²Õ«¬¸¹'
