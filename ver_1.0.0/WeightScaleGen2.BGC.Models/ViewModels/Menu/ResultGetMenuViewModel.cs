using System.Collections.Generic;

namespace WeightScaleGen2.BGC.Models.ViewModels.Menu
{
    public class ResultGetMenuViewModel
    {
        public long menu_definition { get; set; }
        public long menu_id { get; set; }
        public int menu_level { get; set; }
        public int? parent_menu_id { get; set; }
        public string menu_name { get; set; }
        public string display_name { get; set; }
        public string icon { get; set; }
        public string url_controller { get; set; }
        public string url { get; set; }
        public bool is_display { get; set; }
        public int order_by { get; set; }
        public List<ResultGetMenuSectionViewModel> section { get; set; }
        public ResultGetMenuViewModel()
        {
            section = new List<ResultGetMenuSectionViewModel>();
        }
    }
    public class ResultGetMenuSectionViewModel
    {
        public string role_item_id { get; set; }
        public string menu_section_name { get; set; }
        public string menu_section_name_display { get; set; }
        public int section_no { get; set; }
        public bool is_action { get; set; }
        public bool is_display { get; set; }
    }

}
