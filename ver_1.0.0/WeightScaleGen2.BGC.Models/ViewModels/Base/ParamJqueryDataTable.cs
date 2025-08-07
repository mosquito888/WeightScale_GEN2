using System;
using System.Collections.Generic;

namespace WeightScaleGen2.BGC.Models.ViewModels.Base
{
    public class ParamJqueryDataTable
    {
        public ParamJqueryDataTable()
        {
            this.order = new List<ParamJqueryDataTableOrder>();
            this.columns = new List<ParamJqueryDataTableColumn>();
        }
        public int draw { get; set; }
        public int start { get; set; }
        public int length { get; set; }
        public IDictionary<string, object> active_filters { get; set; }
        public IDictionary<string, ValueTuple<object, object>> active_range_filters { get; set; }
        public ParamJqueryDataTableSearch search { get; set; }
        public List<ParamJqueryDataTableOrder> order { get; set; }
        public List<ParamJqueryDataTableColumn> columns { get; set; }

    }

    public class ParamJqueryDataTableSearch
    {
        public string value { get; set; }
        public bool regex { get; set; }
    }

    public enum ParamJqueryDataTable_DIR
    {
        asc, desc
    }

    public class ParamJqueryDataTableOrder
    {
        public int column { get; set; }
        public ParamJqueryDataTable_DIR dir { get; set; }
    }

    public class ParamJqueryDataTableColumn
    {
        public string data { get; set; }
        public string name { get; set; }
        public Boolean orderable { get; set; }
    }

}
