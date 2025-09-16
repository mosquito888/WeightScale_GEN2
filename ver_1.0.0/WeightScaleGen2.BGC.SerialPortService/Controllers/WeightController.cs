using System.Web.Http;
using System.Web.Http.Cors;

namespace WeightScaleGen2.BGC.SerialPortService.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class WeightController : ApiController
    {
        [HttpGet]
        [Route("api/runservice")]
        public IHttpActionResult RunService()
        {
            return Ok("OK");
        }

        [HttpGet]
        [Route("api/weight")]
        public IHttpActionResult GetWeight()
        {
            return Ok(Program.latestWeight);
        }
    }
}
