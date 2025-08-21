using System;
using WeightScaleGen2.BGC.Models.ServicesModels;

namespace WeightScaleGen2.BGC.Models.DBModels
{
    public class UserData
    {
        public long user_id { get; set; }
        public long role_id { get; set; }
        public string emp_code { get; set; }
        public string name { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string created_by { get; set; }
        public DateTime created_date { get; set; }
        public string modified_by { get; set; }
        public DateTime modified_date { get; set; }
        public bool is_active { get; set; }
        public bool is_deleted { get; set; }
        #region [ADD ON]
        public int total_record { get; set; }
        public string role_name { get; set; }
        #endregion [ADD ON]
    }
    public class UserDataResponse : BaseResponse
    {
        public UserData user { get; set; }
    }
}
