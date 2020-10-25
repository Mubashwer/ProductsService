using Microsoft.AspNetCore.Mvc;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace Products.API.Controllers
{
    [Route("")]
    [ApiController]
    public class DefaultController : ControllerBase
    {
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public IActionResult Index() => Redirect("~/swagger");
        
        [HttpGet("ping")]
        [ProducesResponseType(Status200OK)]
        public IActionResult Ping() => Ok();
    }
}