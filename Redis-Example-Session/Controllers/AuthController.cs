using Microsoft.AspNetCore.Mvc;
using Redis_Example_Session.Models;
using Redis_Example_Session.Models.Dtos;
using System.Text.Json;

namespace Redis_Example_Session.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly TokenService _tokenService;
        private readonly RedisService _redisService;
        private readonly IConfiguration _config;
        public AuthController(TokenService tokenService, RedisService redisService, IConfiguration config)
        {
            _tokenService = tokenService;
            _redisService = redisService;
            _config = config;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (request.Username != "admin" || request.Password != "1234")
                return Unauthorized();

            var user = new User
            {
                Id = 1,
                Username = request.Username,
                Role = "Admin"
            };

            var token = _tokenService.GenerateToken(user);

            var session = new SessionData
            {
                UserId = user.Id,
                Username = user.Username,
                Role = user.Role,
                IPAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                LoginTime = DateTime.UtcNow
            };

            var db = _redisService.Database;
            var key = $"session:{token}";
            var value = JsonSerializer.Serialize(session);

            var expireMinutes = int.Parse(_config["Jwt:ExpireMinutes"]!);

            await db.StringSetAsync(key, value, TimeSpan.FromMinutes(expireMinutes));

            return Ok(new { token });
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetSessionFromToken()
        {
            var authHeader = Request.Headers["Authorization"].ToString();

            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
                return Unauthorized("Bearer token required");

            var token = authHeader["Bearer ".Length..].Trim();
            var key = $"session:{token}";

            var db = _redisService.Database;
            var sessionJson = await db.StringGetAsync(key);

            if (string.IsNullOrEmpty(sessionJson))
                return Unauthorized("Session expired or invalid");

            var session = JsonSerializer.Deserialize<SessionData>(sessionJson!);
            return Ok(session);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var authHeader = Request.Headers["Authorization"].ToString();

            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
                return Unauthorized("Bearer token required");

            var token = authHeader["Bearer ".Length..].Trim();
            var key = $"session:{token}";

            var db = _redisService.Database;
            var deleted = await db.KeyDeleteAsync(key);

            if (!deleted)
                return NotFound("Session already expired or not found");

            return Ok("Logged out successfully");
        }
    }
}
