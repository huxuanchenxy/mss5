using Microsoft.AspNetCore.Mvc;

namespace MSS.API.Core.V1.Controllers
{

    public class HealthResult
    {

        public string status { get; set; }

    }

    [Route("api/v1/mro")]
    [ApiController]
    public class HealthController : ControllerBase
    {

        public HealthController()
        {
        }

        //[HttpGet("health.json")]
        //public System.Net.Http.HttpResponseMessage GetAll()
        //{
        //    string json = "{\"status\":\"up\"}";
        //    return new HttpResponseMessage { Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json") };
        //}
        [HttpGet("health.json")]
        public ActionResult GetAll()
        {
            HealthResult hr = new HealthResult();
            hr.status = "UP";
            //string json = "{\"status\":\"up\"}";
            return Ok(hr);
        }

    }
}