using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Invoicer.Authentification
{
    [Route("api/secure")]
    [ApiController]
    [Authorize]
    public class SecureController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetSecretData()
        {
            return Ok(new { message = "You have accessed a protected route!" });
        }
    }
}
