using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dapper;
using System.Reflection;

namespace cyc
{
    public static partial class Global
    {
        private static cyc.DB.SqlDapperConn _gDB;
        public static cyc.DB.SqlDapperConn gDB { get { if (_gDB == null) { _gDB = new DB.SqlDapperConn(); } return _gDB; } }

        static Dictionary<string, ISysCache> SysCacheList = new Dictionary<string, ISysCache>();

        public static SysCacheObj<SysDept> SysDept { get; set; } = new SysCacheObj<SysDept>(1);
        public static SysCacheObj<SysDir> SysDir { get; set; } = new SysCacheObj<SysDir>(2);
        public static SysCacheObj<SysProg> SysProg { get; set; } = new SysCacheObj<SysProg>(3);
        public static SysCacheObj<SysProgSub> SysProgSub { get; set; } = new SysCacheObj<SysProgSub>(4);
        public static SysCacheObj<SysRole> SysRole { get; set; } = new SysCacheObj<SysRole>(5);
        public static SysCacheObj<SysRoleProg> SysRoleProg { get; set; } = new SysCacheObj<SysRoleProg>(6);
        public static SysCacheObj<SysRoleProgSub> SysRoleProgSub { get; set; } = new SysCacheObj<SysRoleProgSub>(7);
        public static SysCacheObj<SysRoleUser> SysRoleUser { get; set; } = new SysCacheObj<SysRoleUser>(8);
        public static SysCacheObj<SysSetting> SysSetting { get; set; } = new SysCacheObj<SysSetting>(9);

        public static void Close()
        {
            foreach (var oCache in SysCacheList.Select(p => p.Value)) { oCache.Clear(); }
            if (_gDB != null) { _gDB.Dispose(); _gDB = null; }
        }

        public static void AddSysCache(ISysCache nCache, string sKey)
        {
            SysCacheList.Remove(sKey);
            SysCacheList.Add(sKey, nCache);
        }

        public static void WriteSysError(string error, ExeResult oResult = null, string msg = "")
        {
            if (oResult != null) { oResult.Error(msg == "" ? "發生不可預期錯誤" : msg); }
            System.Threading.Tasks.Task.Run(() => DoWriteError(error));
        }

        static void DoWriteError(string error)
        {
            try
            {
                gDB.Connection.Execute("insert into SysErrorLog (Message) values (@Msg)", new { Msg = error });
            }
            catch (Exception ex) { ExceptionWrite(ex.Message); }
        }

        public static System.Data.DataTable ObjToDataTable<T>(List<T> items)
        {
            System.Data.DataTable dataTable = new System.Data.DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }

        public static void CopyObjectValues<T>(T From, T To)
        {
            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            for (int i = 0; i < Props.Length; i++)
                Props[i].SetValue(To, Props[i].GetValue(From, null), null);
        }

        private static void ExceptionWrite(string msg)
        {
            //Do Something
        }
    }

    public interface ISysCache
    {
        void Clear();
    }

    public class SysCacheObj<T> : ISysCache
    {
        object oLock = new object();
        List<T> _List = null;
        int iSQL { get; set; }
        string sSQL { get; set; }
        object oSQL { get; set; } = null;

        public SysCacheObj(int i, string s = "", object o = null)
        {
            iSQL = i; sSQL = s; oSQL = o;
        }

        public List<T> List
        {
            get
            {
                if (_List == null) { Init(); }
                return _List;
            }
        }

        public void Init(cyc.DB.SqlDapperConn dDB = null, bool IsReset = false)
        {
            lock (oLock)
            {
                if (IsReset && _List != null) { Clear(); }
                if (_List == null) { GetList(); }
            }
            void GetList()
            {
                bool IsNew = dDB == null;
                if (IsNew) { dDB = new DB.SqlDapperConn(); }
                _List = dDB.QueryList<T>(string.IsNullOrEmpty(sSQL) ? SysCacheSQL[iSQL] : sSQL, oSQL).ToList();
                Global.AddSysCache(this, typeof(T).Name);
                if (IsNew) { dDB.Dispose(); }
            }
        }

        public void Clear()
        {
            if (_List != null) { _List.Clear(); _List = null; }
        }

        static string[] SysCacheSQL =
        {
            "select * from SysUser",
            "select * from SysDept",
            "select * from SysDir order by Seq",
            "select A.*,B.Name as DirName from SysProg A inner join SysDir B on A.DirID=B.ID order by B.Seq,A.Seq",
            "select * from SysProgSub",
            "select * from SysRole",
            "select * from SysRoleProg",
            "select * from SysRoleProgSub",
            "select distinct * from SysRoleUser",
            "select * from SysSetting"
        };
    }

    public static class ExecLog
    {
        //public static void WriteKeyLog(LogKeyItem log, cyc.DB.SqlDBConn oDB = null)
        //{
        //    if (log != null)
        //    {
        //        bool isNewDB = false;
        //        if (oDB == null) { oDB = new cyc.DB.SqlDBConn(); isNewDB = true; }

        //        try
        //        { oDB.oConn.Execute("insert into SysOperationLog (SYS_PROG_ID,OPERATION_TYPE,OPERATION_DESC,OPERATION_USER,OPERATION_KEY) values (@ExecID,@ExecType,@ExecDesc,@UserID,@KeyValue)", log); }
        //        catch (Exception ex) { cyc.Global.WriteSysError(ex.Message + ":" + ex.StackTrace); }

        //        if (isNewDB) { oDB.Dispose(); }
        //    }
        //}

        public static void WriteKeyLog(LogKeyItem log, cyc.DB.SqlDapperConn dDB)
        {
            if (log != null)
            {
                try
                { dDB.Execute("insert into SysOperationLog (SYS_PROG_ID,OPERATION_TYPE,OPERATION_DESC,OPERATION_USER,OPERATION_KEY) values (@ExecID,@ExecType,@ExecDesc,@UserID,@KeyValue)", log); }
                catch (Exception ex) { cyc.Global.WriteSysError(ex.Message + ":" + ex.StackTrace); }
            }
        }

        public class LogKeyItem
        {
            public int ExecID { get; set; }
            public string ExecType { get; set; }
            public string ExecDesc { get; set; }
            public int UserID { get; set; }
            public string KeyValue { get; set; }
        }
    }

    #region "Class"
    public class UserInfo
    {
        public SysUser User { get; set; }
        public SysDept Dept { get; set; }
        //public List<SysRole> UserRole { get; set; }
        public List<int> Roles { get; set; }
        public string Guid { get; set; }
        public string IP { get; set; }
    }

    public class SysUser
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int DeptID { get; set; }
        public bool isManager { get; set; }
        public bool Enabled { get; set; }
        public string Password { get; set; }
        public object Clone() { return this.MemberwiseClone(); }
    }

    public class SysDept
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int UpperID { get; set; }
        public string UpperCode { get; set; }
        public string Manager { get; set; }
        public bool isShow { get; set; }
        public object Clone() { return this.MemberwiseClone(); }
    }

    public class SysDir
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int Seq { get; set; }
    }

    public class SysProg
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int DirID { get; set; }
        public string DirName { get; set; }
        public string Folder { get; set; }
        public string Path { get; set; }
        public int Seq { get; set; }
        public bool Enabled { get; set; }
        public object Clone() { return this.MemberwiseClone(); }
    }

    public class SysProgSub
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public int UpperID { get; set; }
        public bool isShow { get; set; }
    }

    public class SysRole
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int? LevelNo { get; set; }
        public bool IsDefault { get; set; }
        public bool Enabled { get; set; }
        public int User { get; set; }
    }

    public class SysRoleProg
    {
        public int RoleID { get; set; }
        public int ProgID { get; set; }
        public bool isAllSub { get; set; }
    }

    public class SysRoleProgSub
    {
        public int RoleID { get; set; }
        public int ProgID { get; set; }
        public int SubID { get; set; }
    }

    public class SysRoleUser
    {
        public int RoleID { get; set; }
        public int UserID { get; set; }
    }

    public class SysSetting
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }

    public class ExeResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public ExeResult() { Reset(); }
        public void Error(string msg) { Success = false; Message = msg; }
        public void Reset() { Success = true; Message = ""; }
    }

    public class UIMenuMain
    {
        public string Name { get; set; }
        public int Seq { get; set; }
        public List<UIMenuItem> Items { get; set; }
    }

    public class UIMenuItem
    {
        public int ID { get; set; }
        public string Dir { get; set; }
        public string Name { get; set; }
        public int Seq { get; set; }
    }
    #endregion
}

