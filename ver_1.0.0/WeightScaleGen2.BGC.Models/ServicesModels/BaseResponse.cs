using System.Collections.Generic;

namespace WeightScaleGen2.BGC.Models.ServicesModels
{
    public class BaseResponse
    {
        private List<string> _message = new List<string>();
        public List<string> message
        {
            get
            {
                return _message;
            }
            set
            {
                if (value == null)
                    _message = new List<string>();
                else
                    _message = value;
            }
        }
        public bool isCompleted { get; set; }
    }
}
