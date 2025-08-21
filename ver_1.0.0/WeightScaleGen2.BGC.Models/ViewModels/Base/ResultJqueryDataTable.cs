using System.Collections.Generic;

namespace WeightScaleGen2.BGC.Models.ViewModels.Base
{
    public class ResultJqueryDataTable<T>
    {
        public ResultJqueryDataTable()
        {
            this.data = new List<T>();
        }

        public int draw { get; set; }
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
        public IEnumerable<T> data { get; set; }
        public string status { get; set; }
        public string message { get; set; }
    }
}
