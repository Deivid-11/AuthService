using Application.Interfaces.UserInterface;
using Application.Models.AuthModels.Login;
using Application.Models.AuthModels.Register;
using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthServiceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost("register")]
        //[Authorize(Roles = "User")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO request)
        {
            var result = await _userService.RegisterUser(request);
            return new JsonResult(result);
        }

        [HttpPost("login")]
        //[Authorize(Roles = "User")]
        public async Task<IActionResult> Login([FromBody] LoginDTO request)
        {
            var result = await _userService.LoginUser(request);
            return new JsonResult(result);
        }

        [HttpGet("users")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> GetAllUsers()
        {
            var result = await _userService.GetAllUsers();
            return new JsonResult(result);
        }
    }
}
