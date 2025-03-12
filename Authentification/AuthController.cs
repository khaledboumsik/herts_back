using Microsoft.AspNetCore.Mvc;

namespace Invoicer.Authentification
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly TokenService _tokenService;

        public AuthController(TokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            if (request.Username == "Ameni Boumsik" && request.Password == "97476848aA") // Change these
            {
                var token = _tokenService.GenerateToken(request.Username);
                return Ok(new { token });
            }

            return Unauthorized(new { message = "Invalid credentials" });
        }
    }

    public record LoginRequest(string Username, string Password);
}
