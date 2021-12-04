using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BoschCartaoDigitalBackEnd.Controllers.v1.AreaOperacional
{
    [Authorize(Roles = "ENTREGA")]
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class AreaOperacionalController : ControllerBase
    {
        public AreaOperacionalController()
        {
        }
        
    }
}