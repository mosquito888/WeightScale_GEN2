namespace WeightScaleGen2.BGC.Models.ServicesModels
{
    public class MessageReport
    {
        public bool is_success { get; set; }
        public string message { get; set; }
        public MessageReport()
        {

        }
        public MessageReport(bool isSuccess, string message)
        {
            this.is_success = isSuccess;
            this.message = message;
        }
    }
}
