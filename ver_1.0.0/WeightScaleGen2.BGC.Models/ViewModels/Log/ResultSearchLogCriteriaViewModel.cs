using System.Collections.Generic;
using WeightScaleGen2.BGC.Models.ViewModels.Base;

namespace WeightScaleGen2.BGC.Models.ViewModels.Log
{
    public class ResultSearchLogCriteriaViewModel
    {
        public IEnumerable<BaseDLLViewModel> level_item { get; set; }
        public ResultSearchLogCriteriaViewModel()
        {
            this.level_item = new List<BaseDLLViewModel>();
        }
    }
}
