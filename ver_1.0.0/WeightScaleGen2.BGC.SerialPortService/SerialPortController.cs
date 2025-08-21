using System.Web.Http;

namespace WeightScaleGen2.BGC.SerialPortService
{
    public class SerialPortController : ApiController
    {
        // GET api/SerialPort/GetWeight?serialType=A
        [HttpGet]
        public IHttpActionResult GetWeight(string serialType)
        {
            SerialPortService svc = new SerialPortService();
            var res = svc.ConnectSerialPort(serialType).Result;
            return Ok(res);
        }

        // GET api/SerialPort/RunService
        [HttpGet]
        public IHttpActionResult RunService()
        {
            SerialPortService svc = new SerialPortService();
            var res = svc.RunService().Result;
            return Ok(res);
        }
    }
}
