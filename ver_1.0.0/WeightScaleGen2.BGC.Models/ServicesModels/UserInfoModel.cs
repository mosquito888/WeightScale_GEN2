namespace WeightScaleGen2.BGC.Models.ServicesModels
{
    public class UserInfoModel
    {
        public string comp_code { get; set; }
        public string plant_code { get; set; }
        public string short_code { get; set; }
        public string emp_code { get; set; }
        public string dept_code { get; set; }
        public int user_id { get; set; }
        public string name { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public long role_id { get; set; }
        public bool is_super_role { get; set; }
    }
}
