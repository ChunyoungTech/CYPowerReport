using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using Dapper;
using System.Security.Cryptography;
using System.Reflection;

namespace cyc.Comm
{
    /// <summary>
    /// 系統參數查詢取值功能
    /// </summary>
    public static class SysQuery
    {
        public static string GetSysSettingValue(string code)
        {
            if (cyc.Global.SysSetting.List.FirstOrDefault(p => p.Code == code) != null)
                return cyc.Global.SysSetting.List.FirstOrDefault(p => p.Code == code).Value;
            return "";
        }

        public static string GetAppSettingValue(string name)
        {
            try
            {
                string value = System.Web.Configuration.WebConfigurationManager.AppSettings[name];
                return value ?? "";
            }
            catch { return ""; }
        }

        public static string GetConnectString(string name)
        {
            try
            {
                return System.Configuration.ConfigurationManager.ConnectionStrings[name].ToString();
            }
            catch { return ""; }
        }

        public static ExeResult UpdateSysSetting(string code, string value)
        {
            ExeResult oResult = new ExeResult();
            try
            {
                cyc.Global.gDB.Execute("update SysSetting set Value=@Value where Code=@Code", new { Code = code, Value = value });
                var data = cyc.Global.SysSetting.List.FirstOrDefault(p => p.Code == code);
                if (data != null) { data.Value = value; }
                //pin.SysInit.InitSysSetting();
            }
            catch (Exception ex) { cyc.Global.WriteSysError(ex.Message, oResult); }
            return oResult;
        }
    }

    /// <summary>
    /// 系統公用檢核功能
    /// </summary>
    public static class Check
    {
        public static bool IsNumeric(object exp)
        {
            double retNum;
            return Double.TryParse(Convert.ToString(exp), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum);
        }

        public static bool IsInteger(object exp, bool isOverZero = false)
        {
            int retNum;
            if (int.TryParse(exp.ToString(), out retNum))
                return !isOverZero || (isOverZero && retNum > 0);
            else
                return false;
        }

        public static bool IsDateTime(object exp)
        {
            DateTime dt;
            return DateTime.TryParse(exp.ToString(), out dt);
        }

        public static bool IsOverZero(object exp, bool bEqu = false)
        {
            if (IsNumeric(exp))
            {
                if (!bEqu)
                    return (Convert.ToDouble(exp) > 0);
                else
                    return (Convert.ToDouble(exp) >= 0);
            }
            return false;
        }

        public static bool IsNumBetween(object exp, double max, double min = 0)
        {
            if (IsNumeric(exp)) { return (Convert.ToDouble(exp) >= min && Convert.ToDouble(exp) <= max); }
            return false;
        }

        public static bool IsTWIDNoComplex(string code)
        {
            var d = false;
            if (code.Length == 10)
            {
                code = code.ToUpper();
                if (code[0] >= 0x41 && code[0] <= 0x5A)
                {
                    var a = new[] { 10, 11, 12, 13, 14, 15, 16, 17, 34, 18, 19, 20, 21, 22, 35, 23, 24, 25, 26, 27, 28, 29, 32, 30, 31, 33 };
                    var b = new int[11];
                    b[1] = a[(code[0]) - 65] % 10;
                    var c = b[0] = a[(code[0]) - 65] / 10;
                    for (var i = 1; i <= 9; i++)
                    {
                        b[i + 1] = code[i] - 48;
                        c += b[i] * (10 - i);
                    }
                    if (((c % 10) + b[10]) % 10 == 0)
                    {
                        d = true;
                    }
                }
            }
            return d;
        }

        public static bool IsTWIDNoSimple(string code)
        {
            var d = false;
            if (code.Length == 10)
            {
                code = code.ToUpper();
                if (code[0] >= 0x41 && code[0] <= 0x5A && (code[1] == '1' || code[1] == '2'))
                {
                    d = true;
                    for (int i = 2; i < 10; i++)
                    {
                        if (!(code[i] >= 0x30 && code[i] <= 0x39))
                        {
                            d = false;
                            break;
                        }
                    }
                }
            }
            return d;
        }
    }

    /// <summary>
    /// 系統公用轉換DBNull值
    /// </summary>
    public static class NullPara
    {
        public static object DateTimeNP(string sDate)
        {
            if (sDate.Length > 0 && Check.IsDateTime(sDate))
                return Convert.ToDateTime(sDate);
            else
                return System.DBNull.Value;
        }

        public static object IntegerNP(string sInt)
        {
            if (sInt.Length > 0 && Check.IsInteger(sInt))
                return Convert.ToInt32(sInt);
            else
                return System.DBNull.Value;
        }

        public static object DoubleNP(string sDouble)
        {
            if (sDouble.Length > 0 && Check.IsNumeric(sDouble))
                return Convert.ToDouble(sDouble);
            else
                return System.DBNull.Value;
        }

        public static object StringNP(string str)
        {
            if (str.Length > 0)
                return str;
            else
                return System.DBNull.Value;
        }
    }
    /// <summary>
    /// 系統公用轉換 object Null
    /// </summary>
    public static class NullValue
    {
        public static object DateTimeNP(string sDate)
        {
            if (sDate.Length > 0 && Check.IsDateTime(sDate))
                return Convert.ToDateTime(sDate);
            else
                return null;
        }

        public static object IntegerNP(string sInt)
        {
            if (sInt.Length > 0 && Check.IsInteger(sInt))
                return Convert.ToInt32(sInt);
            else
                return null;
        }

        public static object DoubleNP(string sDouble)
        {
            if (sDouble.Length > 0 && Check.IsNumeric(sDouble))
                return Convert.ToDouble(sDouble);
            else
                return null;
        }

        public static object DecimalNP(string sDecimal)
        {
            if (sDecimal.Length > 0 && Check.IsNumeric(sDecimal))
                return Convert.ToDecimal(sDecimal);
            else
                return null;
        }

        public static string StringNP(string str)
        {
            if (str.Length > 0)
                return str;
            else
                return null;
        }
    }

    public class Login : System.Web.SessionState.IReadOnlySessionState
    {
        public static bool CheckSession()
        {
            return (System.Web.HttpContext.Current.Session["uid"] != null);
        }

        //public static void GetUserPower(UserInfo oUser)
        //{
        //    if (oUser.User != null)
        //    {
        //        oUser.UserRole = (from lsU in gObj.SysRoleUser
        //                          join lsR in gObj.SysRole on lsU.RoleID equals lsR.ID
        //                          where lsU.UserID == oUser.User.ID
        //                          select lsR).ToList();

        //        if (oUser.UserRole == null || oUser.UserRole.Count() == 0)
        //            oUser.UserRole = (from lsR in gObj.SysRole where lsR.IsDefault == true select lsR).ToList();
        //    }
        //}

        public static List<UIMenuMain> GetUserMenu(UserInfo oUser)
        {
            return (from lsP in (from lsU in oUser.Roles
                                 join lsR in cyc.Global.SysRoleProg.List on lsU equals lsR.RoleID
                                 join lsP in cyc.Global.SysProg.List.Where(p => p.Enabled) on lsR.ProgID equals lsP.ID
                                 select lsP).GroupBy(g => g.ID).Select(s => s.First()).GroupBy(g => g.DirID)
                    join lsD in cyc.Global.SysDir.List on lsP.Key equals lsD.ID
                    select new UIMenuMain { Name = lsD.Name, Seq = lsD.Seq, Items = lsP.Select(s => new UIMenuItem { ID = s.ID, Name = s.Name, Dir = s.Folder, Seq = s.Seq }).OrderBy(o => o.Seq).ToList() }).OrderBy(o => o.Seq).ToList();
        }

        public static cyc.SysUser GetUser(string sID, string sPW)
        {
            using (var oDB = new cyc.DB.SqlDapperConn())
            {
                return oDB.QueryOne<cyc.SysUser>("select * from SysUser where Code=@Code and Password=@PW", new { Code = sID.Trim(), PW = cyc.Comm.Login.CryptoPWD(sPW.Trim()) });
            }
        }

        public static cyc.UserInfo GetUserInfo(cyc.SysUser user)
        {
            cyc.UserInfo oUser = new cyc.UserInfo()
            {
                User = (cyc.SysUser)user.Clone(),
                Dept = (cyc.SysDept)cyc.Global.SysDept.List.FirstOrDefault(p => p.ID == user.DeptID).Clone(),
                Roles = (from lsR in cyc.Global.SysRole.List.Where(p => p.Enabled)
                         join lsU in cyc.Global.SysRoleUser.List on lsR.ID equals lsU.RoleID into UU
                         from lsU in UU.DefaultIfEmpty()
                         where (lsU.UserID == user.ID || lsR.IsDefault)
                         select lsR.ID).Distinct().ToList()
                //UserRole = (from lsR in pin.gObj.SysRole
                //            join lsU in pin.gObj.SysRoleUser on lsR.ID equals lsU.RoleID into UU
                //            from lsU in UU.DefaultIfEmpty()
                //            where lsU.UserID == user.ID || lsR.IsDefault
                //            select lsR).Distinct().ToList()
            };
            return oUser;
        }

        public static string CryptoPWD(string sPWD)
        {
            return Convert.ToBase64String(new SHA256CryptoServiceProvider().ComputeHash(Encoding.Default.GetBytes(sPWD)));
        }

        public static bool CheckUserProg(UserInfo oUser, int iApp)
        {
            var mProg = (from lsR in cyc.Global.SysRoleProg.List
                         join lsU in oUser.Roles on lsR.RoleID equals lsU
                         where lsR.ProgID == iApp
                         select lsR).FirstOrDefault(p => p.isAllSub == true);
            return mProg != null;
        }
    }

    public class NPOI
    {
        public static IWorkbook GetWorkbook(ref System.Web.UI.WebControls.FileUpload oFileUpload)
        {
            IWorkbook workbook = null;
            if (oFileUpload.HasFile)
            {
                string sFileExt = System.IO.Path.GetExtension(oFileUpload.FileName).ToLower();
                if (sFileExt == ".xls")
                    workbook = new HSSFWorkbook(oFileUpload.FileContent);
                else if (sFileExt == ".xlsx")
                    workbook = new XSSFWorkbook(oFileUpload.FileContent);
            }
            return workbook;
        }

        public static IWorkbook GetWorkbook(string sFile)
        {
            IWorkbook workbook = null;
            if (System.IO.File.Exists(sFile))
            {
                string sFileExt = System.IO.Path.GetExtension(sFile).ToLower();
                using (System.IO.FileStream fs = System.IO.File.Open(sFile, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.ReadWrite))
                {
                    switch (sFileExt)
                    {
                        case ".xlsx":
                        case ".xlsm":
                        case ".xltx":
                        case ".xltm":
                            workbook = new XSSFWorkbook(fs);
                            break;
                        case ".xls":
                        case ".xlt":
                            workbook = new HSSFWorkbook(fs);
                            break;
                        default:
                            break;
                    }
                    //if (sFileExt == ".xls")
                    //    workbook = new HSSFWorkbook(fs);
                    //else if (sFileExt == ".xlsx")
                    //    workbook = new XSSFWorkbook(fs);

                    fs.Close();
                }
            }
            return workbook;
        }

        public static string GetCellValue(ICell cell)
        {
            string str = string.Empty;
            switch (cell.CellType)
            {
                case CellType.Numeric:  // 數值格式
                    if (DateUtil.IsCellDateFormatted(cell))
                    {   // 日期格式
                        str = cell.DateCellValue.ToString();
                    }
                    else
                    {   // 數值格式
                        str = cell.NumericCellValue.ToString();
                    }
                    break;
                case CellType.String:   // 字串格式
                    str = cell.StringCellValue;
                    break;
                //case CellType.Formula:  // 公式格式
                //    var formulaValue = formulaEvaluator.Evaluate(cell);
                //    if (formulaValue.CellType == CellType.String) str = formulaValue.StringValue.ToString();          // 執行公式後的值為字串型態
                //    else if (formulaValue.CellType == CellType.Numeric) str = formulaValue.NumberValue.ToString();    // 執行公式後的值為數字型態
                //    break;
                default:
                    break;
            }
            return str;
        }

        public static bool CheckColumnMap(IRow row, string[] strColumns)
        {
            bool bCheck = false;
            if (row.Cells.Count >= strColumns.Length)
            {
                bCheck = true;
                for (int idx = 0; idx < strColumns.Length; idx++)
                {
                    if (row.Cells[idx].StringCellValue.Trim() != strColumns[idx])
                    {
                        bCheck = false;
                        break;
                    }
                }
            }
            return bCheck;
        }
    }

    public static class Shared
    {
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
        public static bool EqualsObject(this object obj1, object obj2)
        {
            var targetArray = getObjectByte(obj1);
            var expectedArray = getObjectByte(obj2);
            var equals = expectedArray.SequenceEqual(targetArray);
            return equals;
        }
        private static byte[] getObjectByte(object model)
        {
            using (System.IO.MemoryStream memory = new System.IO.MemoryStream())
            {
                System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(model.GetType());
                xs.Serialize(memory, model);
                var array = memory.ToArray();
                return array;
            }
        }
        public static string DateTimeWithZone(DateTime? dt)
        {
            if (dt != null)
                return string.Format("{0} Taipei Time", ((DateTime)dt).ToString("yyyy/MM/dd HH:mm"));
            return "";
        }
    }
}
