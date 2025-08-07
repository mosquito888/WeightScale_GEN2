using AutoMapper;
using System;
using WeightScaleGen2.BGC.Models.DBModels;

namespace WeightScaleGen2.BGC.Models.ViewModels.Company
{
    [AutoMap(typeof(CompanyData), ReverseMap = true)]
    public class ResultGetCompViewModel
    {
        public string comp_code { get; set; }
        public string name_th_line1 { get; set; }
        public string name_th_line2 { get; set; }
        public string name_en_line1 { get; set; }
        public string name_en_line2 { get; set; }
        public string addr_th_line1 { get; set; }
        public string addr_th_line2 { get; set; }
        public string addr_en_line1 { get; set; }
        public string addr_en_line2 { get; set; }
        public string phone { get; set; }
        public string province_code { get; set; }
        public string head_report { get; set; }
        public string approve_name { get; set; }
        public bool? edit_after_print { get; set; }
        public string created_by { get; set; }
        public DateTime created_date { get; set; }
        public string modified_by { get; set; }
        public DateTime? modified_date { get; set; }
        public bool is_active { get; set; }
        public bool is_deleted { get; set; }
    }

    public class ResultGetCompInfoViewModel
    {
        public string mode { get; set; }
        public string comp_code { get; set; }
        public string name_th_line1 { get; set; }
        public string name_th_line2 { get; set; }
        public string name_en_line1 { get; set; }
        public string name_en_line2 { get; set; }
        public string addr_th_line1 { get; set; }
        public string addr_th_line2 { get; set; }
        public string addr_en_line1 { get; set; }
        public string addr_en_line2 { get; set; }
        public string phone { get; set; }
        public string province_code { get; set; }
        public string head_report { get; set; }
        public string approve_name { get; set; }
        public bool? edit_after_print { get; set; }
        public string created_by { get; set; }
        public DateTime created_date { get; set; }
        public string modified_by { get; set; }
        public DateTime? modified_date { get; set; }
        public bool is_active { get; set; }
        public bool is_deleted { get; set; }
    }
}
