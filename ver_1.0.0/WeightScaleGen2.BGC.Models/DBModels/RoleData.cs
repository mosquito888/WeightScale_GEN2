using System;
using WeightScaleGen2.BGC.Models.ServicesModels;

namespace WeightScaleGen2.BGC.Models.DBModels
{
    public class RoleData
    {
        public long role_id { get; set; }
        public string role_name { get; set; }
        public string role_desc { get; set; }
        public string created_by { get; set; }
        public DateTime created_date { get; set; }
        public string modified_by { get; set; }
        public DateTime modified_date { get; set; }
        public bool is_active { get; set; }
        public bool is_deleted { get; set; }
        public bool is_super_role { get; set; }
        #region [ADD ON]
        public int total_record { get; set; }
        #endregion [ADD ON]
    }
    public class RoleDataResponse : BaseResponse
    {
        public RoleData role { get; set; }
    }
}
