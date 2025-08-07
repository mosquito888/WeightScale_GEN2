using AutoMapper;
using System;
using WeightScaleGen2.BGC.Models.DBModels;

namespace WeightScaleGen2.BGC.Models.ViewModels.Department
{
    [AutoMap(typeof(DepartmentData), ReverseMap = true)]
    public class ResultGetDeptViewModel
    {
        public string dept_code { get; set; }
        public string comp_code { get; set; }
        public string plant_code { get; set; }
        public string short_code { get; set; }
        public string name_th { get; set; }
        public string name_en { get; set; }
        public string created_by { get; set; }
        public DateTime created_date { get; set; }
        public string modified_by { get; set; }
        public DateTime? modified_date { get; set; }
        public bool is_active { get; set; }
        public bool is_deleted { get; set; }
        public bool is_all { get; set; }
    }

    public class ResultGetDeptInfoViewModel
    {
        public string mode { get; set; }
        public string dept_code { get; set; }
        public string comp_code { get; set; }
        public string plant_code { get; set; }
        public string short_code { get; set; }
        public string name_th { get; set; }
        public string name_en { get; set; }
        public string created_by { get; set; }
        public DateTime created_date { get; set; }
        public string modified_by { get; set; }
        public DateTime? modified_date { get; set; }
        public bool is_active { get; set; }
        public bool is_deleted { get; set; }
        public bool is_all { get; set; }
    }
}
