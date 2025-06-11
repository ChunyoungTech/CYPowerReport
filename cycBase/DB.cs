using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Dapper;

namespace cyc.DB
{
    public class SqlDapperConn : IDisposable
    {
        public const int mID = int.MaxValue;
        public SqlConnection Connection = null;
        public SqlTransaction Transaction = null;
        public string Command { get; set; }
        public object Object { get; set; } = null;
        public cyc.ExeResult Result { get; set; }

        public SqlDapperConn(ExeResult oResult = null, string ConnectSting = "", bool IsTransaction = false)
        {
            Result = oResult ?? new ExeResult();
            Connection = new SqlConnection(string.IsNullOrWhiteSpace(ConnectSting) ? ConnString.Main : ConnectSting);
            if (IsTransaction) { Connection.Open(); Transaction = Connection.BeginTransaction(); }
        }
        public void Dispose()
        {
            if (Transaction != null) { Transaction.Dispose(); Transaction = null; }
            if (Connection != null)
            {
                //if (Connection.State == ConnectionState.Open) Connection.Close();
                if (Connection.State != ConnectionState.Closed) Connection.Close();
                Connection.Dispose(); Connection = null;
            }
        }
        public void Reset() { Result.Reset(); }

        #region #Query
        //public IEnumerable<T> QueryList<T>() => Shared.QueryList<T>(this);
        public IEnumerable<T> QueryList<T>(string sCommand, object oObj = null) => Shared.QueryList<T>(this, sCommand, oObj);
        //public T QueryOne<T>() => Shared.QueryOne<T>(this);
        public T QueryOne<T>(string sCommand, object oObj = null) => Shared.QueryOne<T>(this, sCommand, oObj);
        //public SqlMapper.GridReader QueryMultiple() => Shared.QueryMultiple(this);
        public SqlMapper.GridReader QueryMultiple(string sCommand, object oObj = null) => Shared.QueryMultiple(this, sCommand, oObj);
        //public DataTable QueryDataTable() => Shared.QueryDataTable(this);
        public DataTable QueryDataTable(string sCommand, object oObj = null) => Shared.QueryDataTable(this, sCommand, oObj);
        #endregion

        #region #Query With SqlDBPara
        public DataTable QueryDataTable(SqlDBPara oPara) => Shared.QueryDataTable(this, oPara);
        public DataSet QueryDataSet(SqlDBPara oPara) => Shared.QueryDataSet(this, oPara);
        #endregion

        #region #Execute
        //public int Execute(int OldID = mID) => Shared.Execute(this, OldID);
        public int Execute(string sCommand, object oObject = null, int OldID = mID) => Shared.Execute(this, OldID, sCommand, oObject);
        #endregion

        #region #Transaction
        public void ResultTransaction()
        {
            if (Transaction != null)
                if (Result.Success) { Transaction.Commit(); } else { Transaction.Rollback(); }
        }
        #endregion
    }

    //public class SqlDBConn : IDisposable
    //{
    //    public SqlConnection oConn;
    //    public SqlTransaction oTran;

    //    public SqlDBConn(string sConnStr = "", bool bTran = false)
    //    {
    //        oConn = new SqlConnection(sConnStr == "" ? ConnString.Main : sConnStr);
    //        if (bTran) { oConn.Open(); oTran = oConn.BeginTransaction(); }
    //    }

    //    public void Dispose()
    //    {
    //        if (oTran != null) { oTran.Dispose(); oTran = null; }
    //        if (oConn != null)
    //        {
    //            //if (oConn.State == ConnectionState.Open) { oConn.Close(); }
    //            if (oConn.State != ConnectionState.Closed) { oConn.Close(); }
    //            oConn.Dispose(); oConn = null;
    //        }
    //    }

    //    #region #Query
    //    public DataTable QueryDataTable(SqlDBPara oPara)
    //    {
    //        return Shared.QueryDT(oPara, this);
    //    }
    //    public DataSet QueryDataSet(SqlDBPara oPara, string sTable = "")
    //    {
    //        return Shared.QueryDS(oPara, sTable);
    //    }
    //    public IEnumerable<T> QueryList<T>(SqlDBPara oPara)
    //    {
    //        return Shared.QueryDapper<T>(oPara, this);
    //    }
    //    public IEnumerable<T> QueryList<T>(string sCommand, object oObject = null)
    //    {
    //        return Shared.QueryList<T>(sCommand, oObject, this);
    //    }
    //    public T QueryOne<T>(SqlDBPara oPara)
    //    {
    //        return Shared.QueryDapper<T>(oPara, this).FirstOrDefault();
    //    }
    //    #endregion

    //    #region #Execute
    //    public void Execute(SqlDBPara oPara, ref int iID)
    //    {
    //        if (oPara.Object != null)
    //            iID = Shared.ExecuteDapper(oPara, true, this);
    //        else
    //            Shared.Execute(oPara, ref iID, this);
    //    }
    //    public void Execute(SqlDBPara oPara)
    //    {
    //        if (oPara.Object != null)
    //            Shared.ExecuteDapper(oPara, false, this);
    //        else
    //            Shared.Execute(oPara, this);
    //    }
    //    #endregion

    //    #region #Transaction
    //    public void Commit() { if (oTran != null) oTran.Commit(); }
    //    public void Rollback() { if (oTran != null) oTran.Rollback(); }
    //    public void Result(ExeResult oResult)
    //    {
    //        if (oTran != null)
    //            if (oResult.Success) { oTran.Commit(); } else { oTran.Rollback(); }
    //    }
    //    #endregion
    //}

    public static class Shared
    {
        #region "SqlDBConn"
        //public static DataTable QueryDT(SqlDBPara oPara, SqlDBConn oDB = null)
        //{
        //    DataTable oDT = new DataTable();
        //    Query(oPara, oDT, oDB);
        //    return oDT;
        //}

        //public static DataSet QueryDS(SqlDBPara oPara, string sTable = "", SqlDBConn oDB = null)
        //{
        //    DataSet oDS = new DataSet();
        //    Query(oPara, oDS, sTable, oDB);
        //    return oDS;
        //}

        //public static void Query(SqlDBPara oPara, DataTable oDT, SqlDBConn oDB = null)
        //{
        //    bool IsNew = ParaReset(oPara, ref oDB);
        //    try
        //    {
        //        if (oDB.oConn != null)
        //        {
        //            using (SqlDataAdapter oAdp = new SqlDataAdapter(oPara.Command, oDB.oConn))
        //            {
        //                if (oPara.Parameter != null) { oAdp.SelectCommand.Parameters.AddRange(oPara.Parameter.ToArray()); }
        //                if (oDB.oTran != null) { oAdp.SelectCommand.Transaction = oDB.oTran; }
        //                oAdp.Fill(oDT);
        //            }
        //        }
        //    }
        //    catch (Exception ex) { Global.WriteSysError(ex.Message, oPara.Result); }
        //    if (IsNew) { oDB.Dispose(); }
        //}

        //public static void Query(SqlDBPara oPara, DataSet oDS, string sTable = "", SqlDBConn oDB = null)
        //{
        //    bool IsNew = ParaReset(oPara, ref oDB);
        //    try
        //    {
        //        if (oDB.oConn != null)
        //        {
        //            using (SqlDataAdapter oAdp = new SqlDataAdapter(oPara.Command, oDB.oConn))
        //            {
        //                if (oPara.Parameter != null) { oAdp.SelectCommand.Parameters.AddRange(oPara.Parameter.ToArray()); }
        //                if (oDB.oTran != null) { oAdp.SelectCommand.Transaction = oDB.oTran; }
        //                if (sTable == "")
        //                    oAdp.Fill(oDS);
        //                else
        //                    oAdp.Fill(oDS, sTable);
        //            }
        //        }
        //    }
        //    catch (Exception ex) { Global.WriteSysError(ex.Message, oPara.Result); }
        //    if (IsNew) { oDB.Dispose(); }
        //}

        //public static void Execute(SqlDBPara oPara, ref int iID, SqlDBConn oDB = null)
        //{
        //    bool IsNew = ParaReset(oPara, ref oDB);
        //    try
        //    {
        //        if (oDB.oConn != null)
        //        {
        //            if (oDB.oConn.State == ConnectionState.Closed) { oDB.oConn.Open(); }
        //            using (SqlCommand oCmd = new SqlCommand(oPara.Command + ";SELECT CAST(SCOPE_IDENTITY() AS INT);", oDB.oConn))
        //            {
        //                if (oPara.Parameter != null) oCmd.Parameters.AddRange(oPara.Parameter.ToArray());
        //                if (oDB.oTran != null) oCmd.Transaction = oDB.oTran;
        //                iID = Convert.ToInt32(oCmd.ExecuteScalar());
        //            }
        //        }
        //    }
        //    catch (Exception ex) { Global.WriteSysError(ex.Message, oPara.Result); }
        //    if (IsNew) { oDB.Dispose(); }
        //}

        //public static void Execute(SqlDBPara oPara, SqlDBConn oDB = null)
        //{
        //    bool IsNew = ParaReset(oPara, ref oDB);
        //    try
        //    {
        //        if (oDB.oConn != null)
        //        {
        //            if (oDB.oConn.State == ConnectionState.Closed) { oDB.oConn.Open(); }
        //            using (SqlCommand oCmd = new SqlCommand(oPara.Command, oDB.oConn))
        //            {
        //                if (oPara.Parameter != null) oCmd.Parameters.AddRange(oPara.Parameter.ToArray());
        //                if (oDB.oTran != null) oCmd.Transaction = oDB.oTran;
        //                oCmd.ExecuteNonQuery();
        //            }
        //        }
        //    }
        //    catch (Exception ex) { Global.WriteSysError(ex.Message, oPara.Result); }
        //    if (IsNew) { oDB.Dispose(); }
        //}

        //private static bool ParaReset(SqlDBPara oPara, ref SqlDBConn oDB)
        //{
        //    if (oPara != null && oPara.Result != null) { oPara.Result.Reset(); }
        //    if (oDB == null) { oDB = new SqlDBConn(); return true; }
        //    return false;
        //}

        //public static IEnumerable<T> QueryDapper<T>(SqlDBPara oPara, SqlDBConn oDB = null)
        //{
        //    bool IsNew = ParaReset(oPara, ref oDB);
        //    try
        //    {
        //        if (oDB.oConn.State == ConnectionState.Closed) { oDB.oConn.Open(); }
        //        return oDB.oConn.Query<T>(oPara.Command, oPara.Object, oDB.oTran);
        //    }
        //    catch (Exception ex) { Global.WriteSysError(ex.Message, oPara.Result); }
        //    if (IsNew) { oDB.Dispose(); }
        //    return null;
        //}

        //public static IEnumerable<T> QueryList<T>(string sCommand, object oObject, SqlDBConn oDB = null)
        //{
        //    bool IsNew = ParaReset(null, ref oDB);
        //    try
        //    {
        //        if (oDB.oConn.State == ConnectionState.Closed) { oDB.oConn.Open(); }
        //        return oDB.oConn.Query<T>(sCommand, oObject, oDB.oTran);
        //    }
        //    catch (Exception ex) { Global.WriteSysError(ex.Message); }
        //    if (IsNew) { oDB.Dispose(); }
        //    return null;
        //}

        //public static int ExecuteDapper(SqlDBPara oPara, bool IsGetID = false, SqlDBConn oDB = null)
        //{
        //    bool IsNew = ParaReset(oPara, ref oDB);
        //    try
        //    {
        //        if (oDB.oConn.State == ConnectionState.Closed) { oDB.oConn.Open(); }
        //        if (!IsGetID)
        //            return oDB.oConn.Execute(oPara.Command, oPara.Object, oDB.oTran);
        //        else
        //            return oDB.oConn.QuerySingle<int>(oPara.Command + ";SELECT CAST(SCOPE_IDENTITY() AS INT);", oPara.Object, oDB.oTran);
        //    }
        //    catch (Exception ex) { Global.WriteSysError(ex.Message, oPara.Result); }
        //    if (IsNew) { oDB.Dispose(); }
        //    return 0;
        //}
        #endregion

        #region "SqlDapperConn"
        public static IEnumerable<T> QueryList<T>(SqlDapperConn dDB, string sCommand, object oObj = null)
        {
            dDB.Command = sCommand;
            dDB.Object = oObj;
            return QueryList<T>(dDB);
        }
        public static IEnumerable<T> QueryList<T>(SqlDapperConn dDB, SqlDBPara oPara)
        {
            TransSqlParaToObject(dDB, oPara);
            return QueryList<T>(dDB);
        }
        public static IEnumerable<T> QueryList<T>(SqlDapperConn dDB)
        {
            dDB.Reset();
            try
            {
                return dDB.Connection.Query<T>(dDB.Command, dDB.Object, dDB.Transaction);
            }
            catch (Exception ex) { cyc.Global.WriteSysError(ex.Message, dDB.Result); }
            return null;
        }
        public static T QueryOne<T>(SqlDapperConn dDB, string sCommand, object oObj = null)
        {
            dDB.Command = sCommand;
            dDB.Object = oObj;
            return QueryOne<T>(dDB);
        }
        public static T QueryOne<T>(SqlDapperConn dDB)
        {
            dDB.Reset();
            try
            {
                return dDB.Connection.QueryFirstOrDefault<T>(dDB.Command, dDB.Object, dDB.Transaction);
            }
            catch (Exception ex) { cyc.Global.WriteSysError(ex.Message, dDB.Result); }
            return default;
        }
        public static SqlMapper.GridReader QueryMultiple(SqlDapperConn dDB, string sCommand, object oObj = null)
        {
            dDB.Command = sCommand;
            dDB.Object = oObj;
            return QueryMultiple(dDB);
        }
        public static SqlMapper.GridReader QueryMultiple(SqlDapperConn dDB)
        {
            dDB.Reset();
            try
            {
                return dDB.Connection.QueryMultiple(dDB.Command, dDB.Object, dDB.Transaction);
            }
            catch (Exception ex) { cyc.Global.WriteSysError(ex.Message, dDB.Result); }
            return null;
        }
        public static DataSet QueryDataSet(SqlDapperConn dDB, SqlDBPara oPara)
        {
            dDB.Reset();
            try
            {
                using (SqlDataAdapter oAdapter = new SqlDataAdapter(oPara.Command, dDB.Connection))
                {
                    if (oPara.Parameter != null) { oAdapter.SelectCommand.Parameters.AddRange(oPara.Parameter.ToArray()); }
                    if (dDB.Transaction != null) { oAdapter.SelectCommand.Transaction = dDB.Transaction; }
                    using (DataSet oDS = new DataSet())
                    {
                        oAdapter.Fill(oDS);
                        return oDS;
                    }
                }
            }
            catch (Exception ex) { cyc.Global.WriteSysError(ex.Message, oPara.Result); }
            return null;
        }
        public static DataTable QueryDataTable(SqlDapperConn dDB, string sCommand, object oObj = null)
        {
            dDB.Command = sCommand;
            dDB.Object = oObj;
            return QueryDataTable(dDB);
        }
        public static DataTable QueryDataTable(SqlDapperConn dDB, SqlDBPara oPara)
        {
            TransSqlParaToObject(dDB, oPara);
            return QueryDataTable(dDB);
        }
        public static DataTable QueryDataTable(SqlDapperConn dDB)
        {
            dDB.Reset();
            try
            {
                using (var oReader = dDB.Connection.ExecuteReader(dDB.Command, dDB.Object, dDB.Transaction))
                {
                    using (DataTable oDT = new DataTable())
                    {
                        oDT.Load(oReader);
                        oReader.Close();
                        return oDT;
                    }
                }
            }
            catch (Exception ex) { cyc.Global.WriteSysError(ex.Message, dDB.Result); }
            return null;
        }
        public static int Execute(SqlDapperConn dDB, int OldID, string sCommand, object oObj = null)
        {
            dDB.Command = sCommand;
            dDB.Object = oObj;
            return Execute(dDB, OldID);
        }
        public static int Execute(SqlDapperConn dDB, int OldID)
        {
            int NewID = OldID;
            dDB.Reset();
            try
            {
                if (NewID > 0)
                    dDB.Connection.Execute(dDB.Command, dDB.Object, dDB.Transaction);
                else
                    NewID = dDB.Connection.QuerySingle<int>(dDB.Command + ";SELECT CAST(SCOPE_IDENTITY() AS INT);", dDB.Object, dDB.Transaction);
            }
            catch (Exception ex) { cyc.Global.WriteSysError(ex.Message, dDB.Result); }
            return NewID;
        }
        #endregion

        private static void TransSqlParaToObject(SqlDapperConn dDB, SqlDBPara oPara)
        {
            dDB.Command = oPara.Command;
            dDB.Object = null;
            if (oPara.Parameter.Count > 0)
            {
                var dObject = new System.Dynamic.ExpandoObject() as IDictionary<string, Object>;
                foreach (var x in oPara.Parameter) dObject.Add(x.ParameterName, x.Value);
                dDB.Object = dObject;
            }
        }
        public static string GetEditSQL(string sTable, string[] sColumn, string[] sKey, bool IsInsert)
        {
            if (IsInsert)
                return string.Format("insert into {0} ({1}) values (@{2})", sTable, string.Join(",", sColumn), string.Join(",@", sColumn));
            else
                return string.Format("update {0} set {1} where {2}", sTable, string.Join(",", sColumn.Select(p => string.Format("{0}=@{0}", p))), string.Join(" and ", sKey.Select(p => string.Format("{0}=@{0}", p))));
        }
        public static string GetEditSQL(string sTable, string sColumnAndKey, bool IsInsert)
        {
            string[] sTemp = sColumnAndKey.Split(new string[] { ";;" }, StringSplitOptions.RemoveEmptyEntries);
            if (sTemp.Length == 2)
                return GetEditSQL(sTable, sTemp[0].Split(','), sTemp[1].Split(','), IsInsert);
            return string.Empty;
        }
    }

    public class SqlDBPara
    {
        public string Command { get; set; }
        public List<SqlParameter> Parameter { get; set; }
        public ExeResult Result { get; set; }
        public object Object { get; set; }
        public SqlDBPara() { Parameter = new List<SqlParameter>(); }
        public void Reset() { this.Command.Remove(0); this.Parameter.Clear(); }
    }

    public class SqlBulkPara
    {
        public List<SqlBulkCopyColumnMapping> Mapping { get; set; }
        public string TableName { get; set; }
        public DataTable DataTable { get; set; }
        public ExeResult Result { get; set; }
    }

    public class ConnString
    {
        public static readonly string Main = Comm.SysQuery.GetConnectString("Main"); // ConfigurationManager.ConnectionStrings["Main"].ToString();
        //public static readonly string Other = Comm.SysQuery.GetConnectString("Other"); //ConfigurationManager.ConnectionStrings["Other"].ToString();
    }

    public class DataReader : IDisposable
    {
        public SqlDataReader Reader;
        public bool Read()
        {
            if (Reader != null)
                return Reader.Read();
            else
                return false;
        }
        public void Close()
        {
            if (Reader != null) if (!Reader.IsClosed) Reader.Close();
        }
        public void Dispose()
        {
            if (Reader != null) if (!Reader.IsClosed) Reader.Close();
            Reader = null;
        }
    }
}
