using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cyc.Data
{
    #region 類別定義
    /// <summary>
    /// Tag設定
    /// </summary>
    public class TagData
    {
        public int ID { get; set; }
        public string Tag_Name { get; set; }
        public string Tag_Desc { get; set; }
        public string Unit { get; set; }
        public string Tag_Type { get; set; }
        public decimal? HiHi_Limit { get; set; }
        public decimal? Hi_Limit { get; set; }
        public decimal? Lo_Limit { get; set; }
        public decimal? LoLo_Limit { get; set; }
        public int User { get; set; }
        public DateTime DT { get; set; }
        public string opc_name { get; set; }
    }

    /// <summary>
    /// 部門使用Tag
    /// </summary>
    public class DeptTag
    {
        public int ID { get; set; }
        public int dept_id { get; set; }
        public int tag_data_id { get; set; }
        public string Tag_Name { get; set; }
        public string Tag_Desc { get; set; }
        public int User { get; set; }
        public DateTime DT { get; set; }
    }

    /// <summary>
    /// 部門使用Tag警報
    /// </summary>
    public class DeptAlarmTag : DeptTag
    {
        public decimal? HiHi { get; set; }
        public decimal? Hi { get; set; }
        public decimal? Lo { get; set; }
        public decimal? LoLo { get; set; }

        public bool? All_Enable { get; set; }
        public bool? HiHi_Enable { get; set; }
        public bool? Hi_Enable { get; set; }
        public bool? Lo_Enable { get; set; }
        public bool? LoLo_Enable { get; set; }
        public int? MAppGroupId { get; set; }
    }

    /// <summary>
    /// 報表設定
    /// </summary>
    public class ReportData
    {
        public int ID { get; set; }
        public int dept_id { get; set; }
        public string report_name { get; set; }
        public TimeSpan? auto_create_time { get; set; }
        public TimeSpan? auto_create_time2 { get; set; }
        public string save_pate { get; set; }
        public string report_desc { get; set; }
        public string upload_file_name { get; set; }
        public char stop_flag { get; set; }
        public int report_type { get; set; }
        public int User { get; set; }
        public DateTime DT { get; set; }
    }

    /// <summary>
    /// 報表Tag
    /// </summary>
    public class ReportTag
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int report_data_id { get; set; }
        public int tag_data_id { get; set; }
        public int sort { get; set; }
    }

    /// <summary>
    /// 報表Tag時間點
    /// </summary>
    public class ReportTime
    {
        public int ID { get; set; }
        public int report_data_id { get; set; }
        public TimeSpan value_time { get; set; }
        public int sort { get; set; }
    }

    /// <summary>
    /// 報表執行紀錄
    /// </summary>
    public class ReportExecLog
    {
        public int seq_id { get; set; }
        public int report_data_id { get; set; }
        public DateTime exec_time { get; set; }
        public string exec_status { get; set; }
        public string file_name { get; set; }
        public int dept_id { get; set; }
        public string report_name { get; set; }
    }

    /// <summary>
    /// 記錄值
    /// </summary>
    public class TagValue
    {
        public int ID { get; set; }
        public int tag_id { get; set; }
        public string tag_value { get; set; }
        public DateTime value_datetime { get; set; }
    }

    public class TagExtValue
    {
        public int tag_id { get; set; }
        public string tag_value { get; set; }
        public string tag_value_type { get; set; }
        public DateTime value_datetime { get; set; }
    }
    #endregion
}
