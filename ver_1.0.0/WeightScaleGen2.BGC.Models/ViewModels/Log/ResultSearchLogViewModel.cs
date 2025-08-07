using System;

namespace WeightScaleGen2.BGC.Models.ViewModels.Log
{
    public class ResultSearchLogViewModel
    {
        public string level { get; set; }
        public DateTime log_date { get; set; }
        public string username { get; set; }
        public string message { get; set; }
        public string exception_message { get; set; }
        public string log_caller_file_path { get; set; }
        public string log_source_line_number { get; set; }
        public int total_record { get; set; }
    }
}
