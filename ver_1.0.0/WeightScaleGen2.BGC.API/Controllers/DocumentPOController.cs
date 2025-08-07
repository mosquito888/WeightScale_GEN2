using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WeightScaleGen2.BGC.API.APIServices;
using WeightScaleGen2.BGC.Models.ViewModels.DocumentPO;

namespace WeightScaleGen2.BGC.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class DocumentPOController : Controller
    {
        private readonly ILogger<DocumentPOController> _logger;
        private readonly DocumentPOAPIService _documentPOAPIService;

        public DocumentPOController(ILogger<DocumentPOController> logger, DocumentPOAPIService documentPOAPIService)
        {
            _logger = logger;
            _documentPOAPIService = documentPOAPIService;
        }

        [HttpGet("GetSearchListDocumentPO")]
        public ActionResult GetSearchListDocumentPO(ParamSearchDocumentPOViewModel param)
        {
            var res = _documentPOAPIService.GetSearchListDocumentPO(param).Result;
            return Ok(res);
        }

        [HttpGet("GetDocumentPOInfo")]
        public ActionResult GetDocumentPOInfo(string purchase_number)
        {
            var res = _documentPOAPIService.GetDocumentPOInfo(purchase_number).Result;
            return Ok(res);
        }

    }
}
