using System;
using WeightScaleGen2.BGC.Models.ServicesModels;

namespace WeightScaleGen2.BGC.Models.DBModels
{
    public class MenuData
    {
        public long menu_id { get; set; }
        public int menu_level { get; set; }
        public int parent_menu_id { get; set; }
        public int list_no { get; set; }
        public string menu_name { get; set; }
        public string display_name { get; set; }
        public long menu_definition { get; set; }
        public string icon { get; set; }
        public string url_controller { get; set; }
        public string url { get; set; }
        public long created_by { get; set; }
        public DateTime created_date { get; set; }
        public long modified_by { get; set; }
        public DateTime modified_date { get; set; }
        public bool is_active { get; set; }
        public bool is_deleted { get; set; }
        #region [ADD ON]
        public int role_item_id { get; set; }
        public string menu_section_name { get; set; }
        public string menu_section_name_display { get; set; }
        public int section_no { get; set; }
        public bool is_action { get; set; }
        public bool is_display { get; set; }
        #endregion [ADD ON]
    }

    public class MenuDataResponse : BaseResponse
    {
        public MenuData menu { get; set; }
    }
}
