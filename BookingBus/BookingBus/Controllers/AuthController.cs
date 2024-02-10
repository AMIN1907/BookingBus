using BookingBus.models.auth_model;
using BookingBus.Repository.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System.Threading.Tasks;

/*
 eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJCdXNCb29raW5nM0AxIiwianRpIjoiNzQwMmIzYWEtMWMwYi00ZmNjLWIzYmEtODAyMjg2NDdhY2Q2IiwiZW1haWwiOiJCdXNCb29raW5nM0AxIiwidWlkIjoiMzY2MTA3NWItM2JiZC00MTZjLTgyMjctNjFkMWM3ZDQ4ZDVkIiwicm9sZXMiOlsiQWRtaW4iLCJVc2VyIl0sImV4cCI6MTcxMDE2MjQyMywiaXNzIjoiU2VjdXJlQXBpIiwiYXVkIjoiU2VjdXJlQXBpVXNlciJ9.21LqKp6fZ-XIzYzVZbDlyJQl13Q4-Z7AS6oBlFdY-sI
 */

namespace BookingBus.Controllers

{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.RegisterAsync(model);

            if (!result.IsAuthenticated)
                return BadRequest(result.Message);

            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> login([FromBody] TokenRequestModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.GetTokenAsync(model);

            if (!result.IsAuthenticated)
                return BadRequest(result.Message);

            return Ok(result);
        }

        [HttpPost("addrole")]
        public async Task<IActionResult> AddRoleAsync([FromBody] AddRoleModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.AddRoleAsync(model);

            if (!string.IsNullOrEmpty(result))
                return BadRequest(result);

            return Ok(model);
        }
    }


}