using WeightScaleGen2.BGC.Models.ServicesModels;

namespace WeightScaleGen2.BGC.Models.DBModels
{
    public class FileData
    {
        public long file_id { get; set; }
        public string file_name { get; set; }
        public string file_ext { get; set; }
        public byte[] file_base6 { get; set; }
    }

    public class FileDataResponse : BaseResponse
    {
        public FileData fileData { get; set; }
    }
}
