using System.Collections.Generic;

namespace WeightScaleGen2.BGC.Models.ViewModels.Home
{
    public class ResultHomeViewModel
    {
        public List<VMRESULT_FILE_SYSTEM> ls_file_system { get; set; }
    }
    public class VMRESULT_FILE_SYSTEM
    {
        public VMRESULT_FILE_SYSTEM() { }
        public string type { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public string path { get; set; }
    }
}
