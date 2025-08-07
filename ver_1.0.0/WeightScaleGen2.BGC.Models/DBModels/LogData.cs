using System;
using WeightScaleGen2.BGC.Models.ServicesModels;

namespace WeightScaleGen2.BGC.Models.DBModels
{
    public class LogData
    {
        public long log_id { get; set; }
        public string log_level { get; set; }
        public string log_type { get; set; }
        public int log_error_code { get; set; }
        public DateTime log_date { get; set; }
        public string log_message { get; set; }
        public string log_inner_exception { get; set; }
        public string log_exception_message { get; set; }
        public string log_additional_Info { get; set; }
        public string log_caller_member_name { get; set; }
        public string log_stack_trace { get; set; }
        public string log_caller_file_path { get; set; }
        public string log_source_line_number { get; set; }
        public string log_user { get; set; }
        public string log_ip_address { get; set; }
        #region [ADD ON]
        public int total_record { get; set; }
        public string role_name { get; set; }
        #endregion [ADD ON]
    }

    public class LogDataResponse : BaseResponse
    {
        public LogData log { get; set; }
    }

    public class LogLevelData
    {
        public string level_code { get; set; }
        public string level_value { get; set; }
        public string level_desc_th { get; set; }
        public string level_desc_en { get; set; }
    }

    public class LogLevelDataResponse : BaseResponse
    {
        public LogLevelData log_level { get; set; }
    }
}
