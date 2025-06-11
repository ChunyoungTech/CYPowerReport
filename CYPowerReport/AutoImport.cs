using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using cyc.DB;
using Dapper;
using System.Collections.Generic;
using System.Globalization;

namespace WebApp
{
    public class AutoImport : IDisposable //: cyc.Page.BasePageDB
    {
        // 設定每一輪工作執行完畢之後要間隔幾分鐘再執行下一輪工作.
        const int LoopIntervalInMinutes = 1000 * 60 * 1;

        private cyc.ExeResult oResult = new cyc.ExeResult();
        private cyc.DB.SqlDapperConn _DB = null;
        private cyc.DB.SqlDapperConn dDB { get { if (_DB == null) { _DB = new SqlDapperConn(oResult); } return _DB; } }

        private bool _stopping = false;

        public AutoImport()
        {

        }

        public void Dispose()
        {
            if (_DB != null) { _DB.Dispose(); }
        }

        public void Run()
        {
            var aThread = new Thread(TaskLoop);
            aThread.IsBackground = true;
            aThread.Priority = ThreadPriority.BelowNormal;  // 避免此背景工作拖慢 ASP.NET 處理 HTTP 請求.
            //TODO 
            aThread.Start();
        }

        public void Stop()
        {
            _stopping = true;
        }

        private void TaskLoop()
        {
            dDB.Execute("EXEC sp_WriteGlobalLog '背景執行啟動 TaskLoop on thread ID:" + Thread.CurrentThread.ManagedThreadId.ToString() + "'");

            while (!_stopping)
            {

                int hh = DateTime.Now.Hour;
                int mm = DateTime.Now.Minute;

                try
                {
                    //Billion();
                    //if (hh == 0 && mm == 5) bDB.oConn.Execute("EXEC sp_DailyCaseMaterial"); 使用SQL Job執行
                    //if (mm == 3 || mm == 18 || mm == 33|| mm == 48) bDB.oConn.Execute("EXEC sp_DailyDataImport"); 使用SQL Job執行
                    if (mm == 6 || mm == 21 || mm == 33 || mm == 51) Bigsum();
                    if (mm == 9 || mm == 24 || mm == 39 || mm == 54) Huawei();
                    if (mm == 12 || mm == 27 || mm == 33 || mm == 57) Billion();
                    //Bigsum();
                    //Huawei();
                    //Billion();
                }
                catch (Exception ex)
                {
                    // 發生意外時只記在 log 裡，不拋出 exception，以確保迴圈持續執行.
                    dDB.Execute("EXEC sp_WriteGlobalLog '" + ex.StackTrace + "'");
                }
                finally
                {
                    //寫入log確保程式持續
                    dDB.Execute("EXEC sp_WriteGlobalLog '背景執行工作中'");

                    // 每一輪工作完成後的延遲.
                    System.Threading.Thread.Sleep(LoopIntervalInMinutes);
                }
            }
        }

        private void Billion()
        {
            int rowCount = 0;
            //取得檔案存放路徑
            string FilePath = System.Configuration.ConfigurationManager.AppSettings["FTP_BILLION"];

            //取得路徑下所有檔案名稱
            DirectoryInfo di = new DirectoryInfo(FilePath);

            foreach (var fi in di.GetFiles())
            {
                if ((fi.Name).IndexOf("CASE_") != -1) //如果檔名包含CASE_才執行
                {
                    //檔案路徑
                    string CASE_file = FilePath + fi.Name;
                    string Done_file = FilePath + "Done\\" + fi.Name;

                    //依正規式取出檔案代表的日期
                    MatchCollection Matches = Regex.Matches((fi.Name), @"(\d+)");

                    //檔案日期格式
                    string FileDate = (Matches[0].ToString()).Substring(0, 8);

                    DateTime dtDate;

                    if (!(DateTime.TryParse(FileDate.Substring(0, 4) + "-" + FileDate.Substring(4, 2) + "-" + FileDate.Substring(6, 2), out dtDate)))
                    {
                        continue;
                    }

                    //DB日期格式
                    string DataDate = DateTime.ParseExact(FileDate, "yyyyMMdd", null).ToString("yyyy/MM/dd");

                    //取得案場資料對應
                    DataTable cbd = new DataTable("CaseBaseData");
                    cbd.Columns.Add("CBD_SEQ_ID", System.Type.GetType("System.Int32"));
                    cbd.Columns.Add("CBD_Case_Code", System.Type.GetType("System.String"));
                    cbd.Columns.Add("CBD_KW", System.Type.GetType("System.Double"));

                    //var res = cyc.Global.gDB.oConn.Query("select CBD_SEQ_ID,CBD_Case_Code,CBD_KW from CaseBaseData where CBD_Case_Code is not null and CBD_Case_Code !='' and CBD_Equipment_Brand='盛齊'");
                    var res = dDB.QueryList<dynamic>("select CBD_SEQ_ID,CBD_Case_Code,CBD_KW from CaseBaseData where CBD_Case_Code is not null and CBD_Case_Code !='' and CBD_Equipment_Brand='盛齊'");
                    foreach (var rec in res)
                    {
                        var d = rec as IDictionary<string, object>;
                        cbd.Rows.Add(d.Values.ToArray()[0], d.Values.ToArray()[1], d.Values.ToArray()[2]);//CBD_SEQ_ID,CBD_Case_Code,CBD_KW
                    }

                    //取得 CASE CSV內資料
                    DataTable CaseData = new DataTable("Case");
                    CaseData.Columns.Add("CASE_NAME", System.Type.GetType("System.String"));
                    CaseData.Columns.Add("CASE_MANEGER", System.Type.GetType("System.String"));
                    CaseData.Columns.Add("UPLOAD_AMT", System.Type.GetType("System.Double"));
                    CaseData.Columns.Add("UPLOAD_RATE", System.Type.GetType("System.Double"));
                    CaseData.Columns.Add("UPLOAD_PR", System.Type.GetType("System.Double"));
                    CaseData.Columns.Add("DATA_DATE", System.Type.GetType("System.DateTime"));

                    try
                    {
                        StreamReader s_CASE = new StreamReader(CASE_file, System.Text.Encoding.Default);
                        string AllData = s_CASE.ReadToEnd();
                        string[] rows = AllData.Split("\n".ToCharArray());
                        foreach (string r in rows)
                        {
                            rowCount++;
                            string r2 = r.Replace("\"", "");
                            r2 = r2.Replace("\r", "");
                            if (r2.IndexOf("案場名稱") == -1 && r2.Length > 10)
                            {
                                string[] items = r2.Split(",".ToCharArray());
                                items[5] = DateTime.ParseExact(items[5], "yyyy/M/d", null).ToString("yyyy/MM/dd");
                                if (items.Length > 2 && items[5] == DataDate)
                                {
                                    if (items[2] == "null") items[2] = "0";
                                    if (items[3] == "null") items[3] = "0";
                                    if (items[4] == "null") items[4] = "0";
                                    CaseData.Rows.Add(items);
                                }
                            }
                        }
                        s_CASE.Close();
                    }
                    catch (Exception ex)
                    {
                        dDB.Execute(" EXEC sp_WriteGlobalLog '" + fi.Name + " 在第 " + rowCount + " 行出錯，請檢查檔案" + ex.StackTrace + "'");
                        continue;
                    }



                    //將案場資料及CSV資料join後上傳CaseUpload
                    try
                    {
                        var Case = from t in cbd.AsEnumerable()
                                   join t2 in CaseData.AsEnumerable()
                                           on t.Field<string>("CBD_Case_Code") equals t2.Field<string>("CASE_NAME")
                                   group t by new { f1 = t.Field<Int32>("CBD_SEQ_ID"), f2 = t2.Field<DateTime>("DATA_DATE"), f3 = t2.Field<Double>("UPLOAD_AMT"), f4 = t2.Field<Double>("UPLOAD_RATE"), f5 = t2.Field<Double>("UPLOAD_PR") } into m
                                   select new
                                   {
                                       CBD_SEQ_ID = m.Key.f1,
                                       UPLOAD_AMT = m.Key.f3,
                                       UPLOAD_RATE = m.Key.f4,
                                       UPLOAD_PR = m.Key.f5,
                                       DATA_DATE = (m.Key.f2).ToString("yyyy/MM/dd"),
                                       TheYear = ((m.Key.f2).Year).ToString(),
                                       TheMonth = ((m.Key.f2).Month).ToString().PadLeft(2, '0'),
                                       TheWeek = (GetWeekOfYear(m.Key.f2)).ToString().PadLeft(2, '0')
                                   };

                        if (Case.ToList().Count > 0)
                        {
                            Case.ToList().ForEach(q =>
                            {
                                string x = "DELETE CaseUpload WHERE DATA_DATE='" + q.DATA_DATE + "' AND CBD_SEQ_ID='" + q.CBD_SEQ_ID + "'";
                                dDB.Execute(x);
                                string y = "INSERT INTO CaseUpload(CBD_SEQ_ID,UPLOAD_AMT,UPLOAD_RATE,UPLOAD_PR,DATA_DATE,CU_Year,CU_Month,CU_Week) VALUES(" + q.CBD_SEQ_ID + ",'" + q.UPLOAD_AMT + "','" + q.UPLOAD_RATE + "'," + ((q.UPLOAD_PR > 100) ? 100 : q.UPLOAD_PR) + ",'" + q.DATA_DATE + "','" + q.TheYear + "','" + q.TheMonth + "','" + q.TheWeek + "')";
                                dDB.Execute(y);
                            });
                        }
                    }
                    catch (InvalidCastException ex)
                    {
                        dDB.Execute("EXEC sp_WriteGlobalLog '" + fi.Name + ex.StackTrace + "'");
                        continue;
                    }

                    //將檔案移動到Done資料夾
                    try
                    {
                        if (System.IO.File.Exists(Done_file)) System.IO.File.Delete(Done_file);
                        File.Move(CASE_file, Done_file);
                    }
                    catch (InvalidCastException ex)
                    {
                        dDB.Execute("EXEC sp_WriteGlobalLog '" + fi.Name + ex.StackTrace + "'");
                        continue;
                    }

                }
                else if ((fi.Name).IndexOf("INV_") != -1) //如果檔名包含INV_才執行
                {

                    //檔案路徑

                    string INV_file = FilePath + fi.Name;
                    string Done_file = FilePath + "Done\\" + fi.Name;
                    try
                    {
                        //依正規式取出檔案代表的日期
                        MatchCollection Matches = Regex.Matches((fi.Name), @"(\d+)");

                        //檔案日期格式
                        string FileDate = (Matches[0].ToString()).Substring(0, 8);

                        DateTime dtDate;

                        if (!(DateTime.TryParse(FileDate.Substring(0, 4) + "-" + FileDate.Substring(4, 2) + "-" + FileDate.Substring(6, 2), out dtDate)))
                        {
                            continue;
                        }

                        //DB日期格式
                        string DataDate = DateTime.ParseExact(FileDate, "yyyyMMdd", null).ToString("yyyy/MM/dd");

                        //取得案場資料對應
                        DataTable cbd = new DataTable("CaseBaseData");
                        cbd.Columns.Add("CBD_SEQ_ID", System.Type.GetType("System.Int32"));
                        cbd.Columns.Add("CBD_Case_Code", System.Type.GetType("System.String"));
                        cbd.Columns.Add("CBD_KW", System.Type.GetType("System.Double"));

                        var res = dDB.QueryList<dynamic>("select CBD_SEQ_ID,CBD_Case_Code,CBD_KW from CaseBaseData where CBD_Case_Code is not null and CBD_Case_Code !='' and CBD_Equipment_Brand='盛齊'");
                        foreach (dynamic rec in res)
                        {
                            var d = rec as IDictionary<string, object>;
                            cbd.Rows.Add(d.Values.ToArray()[0], d.Values.ToArray()[1], d.Values.ToArray()[2]);//CBD_SEQ_ID,CBD_Case_Code,CBD_KW
                        }

                        //取得 CASE CSV內資料
                        DataTable InvData = new DataTable("Inv");
                        InvData.Columns.Add("CASE_NAME", System.Type.GetType("System.String"));
                        InvData.Columns.Add("CASE_MANEGER", System.Type.GetType("System.String"));
                        InvData.Columns.Add("DATA_DATE", System.Type.GetType("System.DateTime"));
                        InvData.Columns.Add("INV_NAME", System.Type.GetType("System.String"));
                        InvData.Columns.Add("INV_AMT", System.Type.GetType("System.Double"));
                        InvData.Columns.Add("INV_ESH", System.Type.GetType("System.Double"));

                        StreamReader s_INV = new StreamReader(INV_file, System.Text.Encoding.Default);

                        string AllData = s_INV.ReadToEnd();
                        string[] rows = AllData.Split("\n".ToCharArray());

                        foreach (string r in rows)
                        {
                            rowCount++;
                            string r2 = r.Replace("\"", "");
                            r2 = r2.Replace("\r", "");
                            if (r2.IndexOf("CASE_NAME") == -1 && r2.Length > 10)
                            {
                                string[] items = r2.Split(",".ToCharArray());
                                items[2] = DateTime.ParseExact(items[2], "yyyy/M/d", null).ToString("yyyy/MM/dd");
                                if (items.Length > 2 && items[2] == DataDate)
                                {
                                    if (items[4] == "null") items[4] = "0";
                                    if (items[5] == "null") items[5] = "0";
                                    InvData.Rows.Add(items);
                                }
                            }
                        }

                        s_INV.Close();

                        //將案場資料及CSV資料join後上傳CaseUpload
                        var Inv = from t in cbd.AsEnumerable()
                                  join t2 in InvData.AsEnumerable()
                                          on t.Field<string>("CBD_Case_Code") equals t2.Field<string>("CASE_NAME")
                                  group t by new { f1 = t.Field<Int32>("CBD_SEQ_ID"), f2 = t2.Field<DateTime>("DATA_DATE"), f3 = t2.Field<String>("INV_NAME"), f4 = t2.Field<Double>("INV_AMT"), f5 = t2.Field<Double>("INV_ESH") } into m
                                  select new
                                  {
                                      CBD_SEQ_ID = m.Key.f1,
                                      DATA_DATE = (m.Key.f2).ToString("yyyy/MM/dd"),
                                      INV_NAME = m.Key.f3,
                                      INV_AMT = m.Key.f4,
                                      INV_ESH = m.Key.f5,
                                      TheYear = ((m.Key.f2).Year).ToString(),
                                      TheMonth = ((m.Key.f2).Month).ToString().PadLeft(2, '0'),
                                      TheWeek = (GetWeekOfYear(m.Key.f2)).ToString().PadLeft(2, '0')
                                  };


                        if (Inv.ToList().Count > 0)
                        {
                            Inv.ToList().ForEach(q =>
                            {
                                string x = "DELETE InvUpload WHERE DATA_DATE='" + q.DATA_DATE + "' AND CBD_SEQ_ID='" + q.CBD_SEQ_ID + "' AND INV_NAME='" + q.INV_NAME + "'";
                                dDB.Execute(x);
                                string y = "INSERT INTO InvUpload (CBD_SEQ_ID,INV_NAME,DATA_DATE,INV_AMT,INV_RATE,INV_PR,IU_Year,IU_Month,IU_Week) VALUES(" + q.CBD_SEQ_ID + ",'" + q.INV_NAME + "','" + q.DATA_DATE + "'," + q.INV_AMT + "," + q.INV_ESH + ",0,'" + q.TheYear + "','" + q.TheMonth + "','" + q.TheWeek + "')";
                                dDB.Execute(y);
                            });
                        }

                        //將檔案移動到Done資料夾
                        if (System.IO.File.Exists(Done_file)) System.IO.File.Delete(Done_file);
                        File.Move(INV_file, Done_file);
                    }
                    catch (Exception ex)
                    {
                        dDB.Execute(" EXEC sp_WriteGlobalLog '" + fi.Name + " 在第 " + rowCount + " 行出錯，請檢查檔案" + ex.StackTrace + "'");
                        continue;
                    }

                }
            }
        }

        private void Bigsum()
        {
            int rowCount = 0;
            string curFileName = "";
            try
            {

                //取得檔案存放路徑
                string FilePath = System.Configuration.ConfigurationManager.AppSettings["FTP_BIGSUN"];

                //取得路徑下所有檔案名稱
                DirectoryInfo di = new DirectoryInfo(FilePath);

                foreach (var fi in di.GetFiles())
                {
                    if ((fi.Name).IndexOf("DailyPower") != -1) //如果檔名包含DailyPower才執行
                    {
                        curFileName = fi.Name;
                        //依正規式取出檔案代表的日期
                        MatchCollection Matches = Regex.Matches((fi.Name), @"(\d+)");

                        //檔案日期格式
                        string FileDate = Matches[0].ToString();

                        //DB日期格式
                        string DataDate = DateTime.ParseExact(FileDate, "yyyyMMdd", null).ToString("yyyy/MM/dd");

                        //來源路徑
                        string Sunshine_file = FilePath + "YUXUAN_Sunshine_" + FileDate + ".csv";
                        string DailyPower_file = FilePath + "YUXUAN_DailyPower_" + FileDate + ".csv";

                        //成功後移動路徑
                        string destSunshine_file_Done = FilePath + "Done\\" + "YUXUAN_Sunshine_" + FileDate + ".csv";
                        string destDailyPower_file_Done = FilePath + "Done\\" + "YUXUAN_DailyPower_" + FileDate + ".csv";

                        //失敗時移動路徑
                        string destSunshine_file_Fail = FilePath + "Fail\\" + "YUXUAN_Sunshine_" + FileDate + ".csv";
                        string destDailyPower_file_Fail = FilePath + "Fail\\" + "YUXUAN_DailyPower_" + FileDate + ".csv";

                        //檢查檔案
                        if (!File.Exists(Sunshine_file) || !File.Exists(DailyPower_file))
                        {
                            String d = $"EXEC sp_WriteGlobalLog \"檔案不存在 {DailyPower_file}\"";
                            dDB.Execute(d);
                            continue;
                        }

                        //取得案場資料對應
                        DataTable cbd = new DataTable("CaseBaseData");
                        cbd.Columns.Add("CBD_SEQ_ID", System.Type.GetType("System.Int32"));
                        cbd.Columns.Add("CBD_Case_Code", System.Type.GetType("System.String"));
                        cbd.Columns.Add("CBD_KW", System.Type.GetType("System.Double"));

                        var res = dDB.QueryList<dynamic>("select CBD_SEQ_ID,CBD_Case_Code,CBD_KW from CaseBaseData where CBD_Case_Code is not null and CBD_Case_Code !='' and CBD_Equipment_Brand='Big Sun'");
                        foreach (dynamic rec in res)
                        {
                            var d = rec as IDictionary<string, object>;
                            cbd.Rows.Add(d.Values.ToArray()[0], d.Values.ToArray()[1], d.Values.ToArray()[2]);//CBD_SEQ_ID,CBD_Case_Code,CBD_KW
                        }

                        //取得 Sunshine CSV內資料
                        DataTable Sunshine = new DataTable("Sunshine");
                        Sunshine.Columns.Add("CBD_Case_Code", System.Type.GetType("System.String"));
                        Sunshine.Columns.Add("date", System.Type.GetType("System.DateTime"));
                        Sunshine.Columns.Add("Avg_Sunshine", System.Type.GetType("System.Double"));
                        Sunshine.Columns.Add("TotalESH", System.Type.GetType("System.Double"));
                        StreamReader s_Sunshine = new StreamReader(Sunshine_file, System.Text.Encoding.Default);

                        string AllData = s_Sunshine.ReadToEnd();
                        string[] rows = AllData.Split("\n".ToCharArray());

                        foreach (string r in rows)
                        {
                            rowCount++;
                            if (r.IndexOf("site") == -1)
                            {
                                string[] items = r.Split(",".ToCharArray());
                                if (items.Length > 2 && items[1] == DataDate) Sunshine.Rows.Add(items);
                            }
                        }

                        s_Sunshine.Close();

                        //取得 DailyPower CSV內資料
                        DataTable DailyPower = new DataTable("DailyPower");
                        DailyPower.Columns.Add("CBD_Case_Code", System.Type.GetType("System.String"));
                        DailyPower.Columns.Add("INV_NAME", System.Type.GetType("System.String"));
                        DailyPower.Columns.Add("INV_AMT", System.Type.GetType("System.Double"));
                        DailyPower.Columns.Add("date", System.Type.GetType("System.DateTime"));
                        DailyPower.Columns.Add("Avg_Sunshine", System.Type.GetType("System.Double"));
                        DailyPower.Columns.Add("TotalESH", System.Type.GetType("System.Double"));
                        StreamReader s_DailyPower = new StreamReader(DailyPower_file, System.Text.Encoding.Default);
                        int day = DateTime.Parse(DataDate).Day;

                        string AllData2 = s_DailyPower.ReadToEnd();
                        string[] rows2 = AllData2.Split("\n".ToCharArray());

                        foreach (string r in rows2)
                        {
                            if (r.IndexOf("site") == -1 && r.Length > 10)
                            {
                                string[] col = r.Split(",".ToCharArray());
                                DailyPower.Rows.Add(col[0], col[1], col[day + 1], DataDate, 0, 0);
                            }

                        }

                        s_DailyPower.Close();

                        //將Sunshine資料合併到DailyPower
                        foreach (DataRow drD in DailyPower.Rows)
                        {
                            foreach (DataRow drS in Sunshine.Rows)
                            {
                                if (drD["CBD_Case_Code"].Equals(drS["CBD_Case_Code"]))
                                {
                                    drD["Avg_Sunshine"] = drS["Avg_Sunshine"];
                                    drD["TotalESH"] = drS["TotalESH"];
                                }
                            }
                        }


                        //將案場資料及CSV資料join後上傳CaseUpload
                        var Case = from t in cbd.AsEnumerable()
                                   join t2 in DailyPower.AsEnumerable()
                                           on t.Field<string>("CBD_Case_Code") equals t2.Field<string>("CBD_Case_Code")
                                   group new { f6 = t2.Field<double>("INV_AMT") } by new { f1 = t.Field<string>("CBD_Case_Code"), f2 = t.Field<Int32>("CBD_SEQ_ID"), f3 = t2.Field<DateTime>("date"), f4 = t.Field<double>("CBD_KW"), f5 = t2.Field<double>("TotalESH") } into m
                                   select new
                                   {
                                       site = m.Key.f1,
                                       CBD_SEQ_ID = m.Key.f2,
                                       UPLOAD_AMT = Math.Round(m.Sum(n => n.f6), 2),
                                       UPLOAD_RATE = (m.Key.f4 == 0) ? 0 : Math.Round(((m.Sum(n => n.f6)) / (m.Key.f4)), 2),
                                       UPLOAD_PR = (m.Key.f4 == 0 || m.Key.f5 == 0) ? 0 : Math.Round(((m.Sum(n => n.f6)) / (m.Key.f4) / (m.Key.f5)) * 100, 2),
                                       DATA_DATE = (m.Key.f3).ToString("yyyy/MM/dd"),
                                       TheYear = ((m.Key.f3).Year).ToString(),
                                       TheMonth = ((m.Key.f3).Month).ToString().PadLeft(2, '0'),
                                       TheWeek = (GetWeekOfYear(m.Key.f3)).ToString().PadLeft(2, '0')
                                   };


                        if (Case.ToList().Count > 0)
                        {
                            Case.ToList().ForEach(q =>
                            {
                                string x = "DELETE CaseUpload WHERE DATA_DATE='" + q.DATA_DATE + "' AND CBD_SEQ_ID='" + q.CBD_SEQ_ID + "'";
                                dDB.Execute(x);
                                string y = "INSERT INTO CaseUpload(CBD_SEQ_ID,UPLOAD_AMT,UPLOAD_RATE,UPLOAD_PR,DATA_DATE,CU_Year,CU_Month,CU_Week) VALUES(" + q.CBD_SEQ_ID + ",'" + q.UPLOAD_AMT + "','" + q.UPLOAD_RATE + "'," + ((q.UPLOAD_PR > 100) ? 100 : q.UPLOAD_PR) + ",'" + q.DATA_DATE + "','" + q.TheYear + "','" + q.TheMonth + "','" + q.TheWeek + "')";
                                dDB.Execute(y);
                            });
                        }

                        //將案場資料及CSV資料join後上傳InvUpload
                        var Inv = from t in cbd.AsEnumerable()
                                  join t2 in DailyPower.AsEnumerable()
                                          on t.Field<string>("CBD_Case_Code") equals t2.Field<string>("CBD_Case_Code")
                                  //join t3 in Sunshine.AsEnumerable()
                                  //        on t.Field<string>("CBD_Case_Code") equals t3.Field<string>("CBD_Case_Code")
                                  select new
                                  {
                                      site = t.Field<string>("CBD_Case_Code"),
                                      CBD_SEQ_ID = t.Field<Int32>("CBD_SEQ_ID"),
                                      INV_NAME = t2.Field<string>("INV_NAME"),
                                      DATA_DATE = (t2.Field<DateTime>("date")).ToString("yyyy/MM/dd"),
                                      INV_AMT = t2.Field<double>("INV_AMT"),
                                      INV_RATE = 0,
                                      INV_PR = 0,
                                      IU_Year = ((t2.Field<DateTime>("date")).Year).ToString(),
                                      IU_Month = ((t2.Field<DateTime>("date")).Month).ToString().PadLeft(2, '0'),
                                      IU_Week = (GetWeekOfYear(t2.Field<DateTime>("date"))).ToString().PadLeft(2, '0')
                                  };

                        if (Inv.ToList().Count > 0)
                        {
                            Inv.ToList().ForEach(q =>
                            {
                                string x = "DELETE InvUpload WHERE DATA_DATE='" + q.DATA_DATE + "' AND CBD_SEQ_ID='" + q.CBD_SEQ_ID + "' AND INV_NAME='" + q.INV_NAME + "'";
                                dDB.Execute(x);
                                string y = "INSERT INTO InvUpload (CBD_SEQ_ID,INV_NAME,DATA_DATE,INV_AMT,INV_RATE,INV_PR,IU_Year,IU_Month,IU_Week) VALUES(" + q.CBD_SEQ_ID + ",'" + q.INV_NAME + "','" + q.DATA_DATE + "'," + q.INV_AMT + ",0,0,'" + q.IU_Year + "','" + q.IU_Month + "','" + q.IU_Week + "')";
                                dDB.Execute(y);
                            });
                        }

                        //將檔案移動到Done資料夾
                        if (System.IO.File.Exists(destSunshine_file_Done)) System.IO.File.Delete(destSunshine_file_Done);
                        File.Move(Sunshine_file, destSunshine_file_Done);
                        if (System.IO.File.Exists(destDailyPower_file_Done)) System.IO.File.Delete(destDailyPower_file_Done);
                        File.Move(DailyPower_file, destDailyPower_file_Done);

                    }
                }
            }
            catch (Exception ex)
            {
                dDB.Execute(" EXEC sp_WriteGlobalLog '" + curFileName + " 在第 " + rowCount + " 行出錯，請檢查檔案" + ex.StackTrace + "'");

            }
        }

        private void Huawei()
        {
            int rowCount = 0;
            string curFileName = "";
            try
            {

                //取得檔案存放路徑
                string FilePath = System.Configuration.ConfigurationManager.AppSettings["FTP_HUAWEI"];

                DataTable cbd = new DataTable("CaseBaseData");
                cbd.Columns.Add("CBD_SEQ_ID", System.Type.GetType("System.Int32"));
                cbd.Columns.Add("CBD_Case_Code", System.Type.GetType("System.String"));
                cbd.Columns.Add("CBD_KW", System.Type.GetType("System.Double"));

                DataTable dt = new DataTable("tmp");
                dt.Columns.Add("INV_Name", System.Type.GetType("System.String"));
                dt.Columns.Add("INV_Case_Code", System.Type.GetType("System.String"));
                dt.Columns.Add("date", System.Type.GetType("System.DateTime"));
                dt.Columns.Add("INV_KW", System.Type.GetType("System.Double"));

                //取得案場資料對應
                var res = dDB.QueryList<dynamic>("select CBD_SEQ_ID,CBD_Case_Code,CBD_KW from CaseBaseData where CBD_Equipment_Brand='華為' and CBD_Case_Code is not null and CBD_Case_Code !=''");

                //依取得案場代碼跑迴圈讀取CSV資料
                foreach (dynamic rec in res)
                {
                    var d = rec as IDictionary<string, object>;
                    string FolderName = d.Values.ToArray()[1].ToString();//資料夾名稱=案場代碼

                    //如果資料夾不存在，則略過此迴圈
                    if (!Directory.Exists(FilePath + FolderName))
                    {
                        continue;
                    }

                    //取得路徑下所有檔案名稱
                    DirectoryInfo di = new DirectoryInfo(FilePath + FolderName);

                    foreach (var fi in di.GetFiles())
                    {
                        if ((fi.Name).IndexOf("min") != -1) //如果檔名包含min才執行
                        {
                            string file = FilePath + FolderName + "\\" + fi.Name;

                            string destfile = FilePath + "Done\\" + FolderName + "\\" + fi.Name;

                            //依正規式取出檔案代表的日期
                            MatchCollection MatchesDate = Regex.Matches((fi.Name), @"(\d+)");

                            //檔案日期格式
                            string FileDate = MatchesDate[0].ToString();

                            //DB日期格式
                            string DataDate = DateTime.ParseExact("20" + FileDate, "yyyyMMdd", null).ToString("yyyy/MM/dd");


                            cbd.Rows.Add(d.Values.ToArray()[0], d.Values.ToArray()[1], d.Values.ToArray()[2]);//CBD_SEQ_ID,CBD_Case_Code,CBD_KW

                            //檢查檔案
                            if (!File.Exists(file))
                            {
                                dDB.Execute("EXEC sp_WriteGlobalLog '華為案場" + d.Values.ToArray()[1] + "CSV檔案不存在'");
                            }
                            else
                            {

                                StreamReader s = new StreamReader(file, System.Text.Encoding.Default);
                                string AllData = s.ReadToEnd();
                                string[] rows = AllData.Split("\n".ToCharArray());
                                string ds = "";
                                int f = 0;
                                foreach (string r in rows)
                                {
                                    rowCount++;
                                    if (r.IndexOf("#") != -1)
                                    {

                                        string Pattern = @"INV(\d+)";

                                        if (Regex.IsMatch(r, Pattern))   // 是否有符合的字串
                                        {
                                            MatchCollection Matches = Regex.Matches(r, Pattern);

                                            ds = Matches[0].ToString();
                                        }
                                        else
                                        {
                                            string[] items = r.Split(";".ToCharArray());
                                            f = Array.IndexOf(items, "Eac");
                                        }
                                    }
                                    else if (r != "" && f != -1)
                                    {
                                        //decimal ii = 0;
                                        string[] items = r.Split(";".ToCharArray());
                                        if (items.Length > f)
                                        {
                                            dt.Rows.Add(ds, d.Values.ToArray()[1], DataDate, items[f]);
                                        }

                                    }

                                }

                                s.Close();


                                //將案場資料及CSV資料join後上傳CaseUpload
                                var Huawei_Case = from t in dt.AsEnumerable()
                                                  join t2 in cbd.AsEnumerable()
                                                          on t.Field<string>("INV_Case_Code") equals t2.Field<string>("CBD_Case_Code")
                                                  group t by new { f1 = t2.Field<Int32>("CBD_SEQ_ID"), f2 = t.Field<DateTime>("date"), f3 = t2.Field<double>("CBD_KW") } into m
                                                  select new
                                                  {
                                                      CBD_SEQ_ID = m.Key.f1,
                                                      INV_KW = m.Sum(n => n.Field<double>("INV_KW")),
                                                      CBD_KW = (m.Key.f3),
                                                      TheDate = (m.Key.f2).ToString("yyyy/MM/dd"),
                                                      TheYear = ((m.Key.f2).Year).ToString(),
                                                      TheMonth = ((m.Key.f2).Month).ToString().PadLeft(2, '0'),
                                                      TheWeek = (GetWeekOfYear(m.Key.f2)).ToString().PadLeft(2, '0')
                                                  };

                                if (Huawei_Case.ToList().Count > 0)
                                {
                                    Huawei_Case.ToList().ForEach(q =>
                                    {
                                        string x = "DELETE CaseUpload WHERE CBD_SEQ_ID=" + q.CBD_SEQ_ID + " AND DATA_DATE='" + q.TheDate + "'";
                                        dDB.Execute(x);
                                        string y = "INSERT INTO CaseUpload(CBD_SEQ_ID,UPLOAD_AMT,UPLOAD_RATE,UPLOAD_PR,DATA_DATE,CU_Year,CU_Month,CU_Week) VALUES(" + q.CBD_SEQ_ID + "," + q.INV_KW + "," + q.INV_KW / q.CBD_KW + ",0,'" + q.TheDate + "','" + q.TheYear + "','" + q.TheMonth + "','" + q.TheWeek + "')";
                                        dDB.Execute(y);
                                    });
                                }

                                //將案場資料及CSV資料join後上傳InvUpload
                                var Huawei_INV = from t in dt.AsEnumerable()
                                                 join t2 in cbd.AsEnumerable()
                                                         on t.Field<string>("INV_Case_Code") equals t2.Field<string>("CBD_Case_Code")
                                                 group t by new { f1 = t2.Field<Int32>("CBD_SEQ_ID"), f2 = t.Field<string>("INV_Name"), f3 = t.Field<DateTime>("date") } into m
                                                 select new
                                                 {
                                                     CBD_SEQ_ID = m.Key.f1,
                                                     INV_Name = m.Key.f2,
                                                     INV_KW = m.Sum(n => n.Field<double>("INV_KW")),
                                                     TheDate = (m.Key.f3).ToString("yyyy/MM/dd"),
                                                     TheYear = ((m.Key.f3).Year).ToString(),
                                                     TheMonth = ((m.Key.f3).Month).ToString().PadLeft(2, '0'),
                                                     TheWeek = (GetWeekOfYear(m.Key.f3)).ToString().PadLeft(2, '0')
                                                 };

                                if (Huawei_INV.ToList().Count > 0)
                                {

                                    Huawei_INV.ToList().ForEach(q =>
                                    {
                                        string x = "DELETE InvUpload WHERE CBD_SEQ_ID=" + q.CBD_SEQ_ID + " AND INV_NAME='" + q.INV_Name + "' AND DATA_DATE='" + q.TheDate + "'";
                                        dDB.Execute(x);
                                        string y = "INSERT INTO InvUpload (CBD_SEQ_ID,INV_NAME,DATA_DATE,INV_AMT,INV_RATE,INV_PR,IU_Year,IU_Month,IU_Week) VALUES(" + q.CBD_SEQ_ID + ",'" + q.INV_Name + "','" + q.TheDate + "'," + q.INV_KW + ",0,0,'" + q.TheYear + "','" + q.TheMonth + "','" + q.TheWeek + "')";
                                        dDB.Execute(y);
                                    });

                                }

                                //將檔案移動到Done資料夾
                                string P = Path.GetDirectoryName(destfile);

                                if (Directory.Exists(P))
                                {
                                    if (System.IO.File.Exists(destfile)) System.IO.File.Delete(destfile);
                                    File.Move(file, destfile);
                                }
                                else
                                {
                                    Directory.CreateDirectory(P);
                                    if (System.IO.File.Exists(destfile)) System.IO.File.Delete(destfile);
                                    File.Move(file, destfile);
                                }


                            }
                            cbd.Clear();
                            dt.Clear();
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                dDB.Execute(" EXEC sp_WriteGlobalLog '" + curFileName + " 在第 " + rowCount + " 行出錯，請檢查檔案" + ex.StackTrace + "'");
            }
        }

        //取日期週數
        private int GetWeekOfYear(DateTime dt)
        {
            GregorianCalendar gc = new GregorianCalendar();
            return gc.GetWeekOfYear(dt, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
        }


    }
}