using Microsoft.AspNetCore.Mvc;
using AuthService.Models;
using AuthService.Domain;

namespace AuthService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IUserService userService, ITokenService tokenService, ILogger<AuthController> logger)
        {
            _userService = userService;
            _tokenService = tokenService;
            _logger = logger;

            _logger.LogInformation("AuthController создан");
        }

        [HttpGet("status")]
        public IActionResult Status()
        {
            return Ok(new
            {
                Service = "AuthService",
                Status = "Running",
                Timestamp = DateTime.UtcNow
            });
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var user = _userService.Authenticate(request.Username, request.Password);
            if (user == null)
            {
                return Unauthorized(new GenericResponse
                {
                    Success = false,
                    Error = "Invalid credentials"
                });
            }

            var token = _tokenService.GenerateToken(user.Username);
            return Ok(new LoginResponse { Token = token });
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterRequest request)
        {
            var success = _userService.Register(request.Username, request.Password);

            if (!success)
            {
                return BadRequest(new GenericResponse
                {
                    Success = false,
                    Error = "User already exists"
                });
            }

            return Ok(new GenericResponse
            {
                Success = true
            });
        }
    }
}
