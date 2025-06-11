Declare @Tab_Name Nvarchar(100)='ElectricityBilling'

--select u.name,o.name As '資料表名稱'
--        ,isnull((select convert(varchar(8000), value) from ::fn_listextendedproperty(NULL, 'user', u.name, 'table', o.name, null, null) where name = 'MS_Description'),'') as '描述'
--    from sys.sysobjects o
--        join sys.schemas  u on (u.schema_id = o.uid)
--    where o.type in ('U') And o.name=@Tab_Name
--    order by 1, 2

SELECT
        A.TABLE_NAME                as '表格名稱',
        B.COLUMN_NAME               as '欄位名稱',
        B.DATA_TYPE                 as '型態',
        Isnull(Case when B.CHARACTER_MAXIMUM_LENGTH='-1' Then 'Max' Else Cast(B.CHARACTER_MAXIMUM_LENGTH as varchar(10)) End,'') as '長度',
        Isnull(( SELECT value FROM fn_listextendedproperty (NULL, 'schema', 'dbo', 'table',a .TABLE_NAME, 'column', default) 
                WHERE name ='MS_Description' and objtype= 'COLUMN' and objname Collate Chinese_Taiwan_Stroke_CI_AS=b .COLUMN_NAME
        ),'') as '說明'
            ,Case When Isnull(CONSTRAINT_NAME,'') != '' Then 'PK' Else '' End As '鍵'
    FROM
        INFORMATION_SCHEMA.TABLES A
        LEFT JOIN INFORMATION_SCHEMA.COLUMNS B ON (A.TABLE_NAME=B.TABLE_NAME)
            Left outer join INFORMATION_SCHEMA.KEY_COLUMN_USAGE C on OBJECTPROPERTY(OBJECT_ID(constraint_name), 'IsPrimaryKey') = 1 And C.Table_Name =A.TABLE_NAME And C.COLUMN_NAME=B.COLUMN_NAME
    WHERE TABLE_TYPE ='BASE TABLE' And A.TABLE_NAME=@Tab_Name
    ORDER BY A.TABLE_NAME,B.ordinal_position