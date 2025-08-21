using System.Collections.Generic;

namespace WeightScaleGen2.BGC.Models.ViewModels.Role
{
    public class ParamUpdateRoleItemViewModel
    {
        public IEnumerable<UpdateRoleItemSection> role_item { get; set; }
        public ParamUpdateRoleItemViewModel()
        {
            role_item = new List<UpdateRoleItemSection>();
        }
    }
    public class UpdateRoleItemSection
    {
        public string role_item_id { get; set; }
        public bool is_display { get; set; }
        public bool is_action { get; set; }
    }
}
