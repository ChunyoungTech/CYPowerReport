use CYPowerReportDB
go

insert into [SysProg]
	([Code],[Name]
	,[DirID],[Folder]
	,[Path],[Enabled]
	,[Seq],[c_date]
	,[u_date],[c_user]
	,[u_user],[maintern_table])
SELECT [Code],'模組型號'
	,[DirID],'_app'
	,'Module_Model.aspx',[Enabled]
	,6,getdate()
	,getdate(),[c_user]
	,[u_user],[maintern_table]
	FROM [SysProg]

	where [Folder]='_sys'
	and [Name]='系統角色設定Q'
	order by [Seq]