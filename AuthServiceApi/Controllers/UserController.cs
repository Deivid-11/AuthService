using Application.Interfaces.UserInterfaces;
using Application.Models.UserModels;
using Microsoft.AspNetCore.Mvc;

namespace AuthServiceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly 

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // POST: api/user/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRequestDTO user)
        {
            if (await _userService.ExistUser(user.Email))
            {
                throw new ArgumentException("Existe usuario asociado al mail ingresado");
            }
            var result = await _userService.RegisterUser(user);
            return new JsonResult(result);
        }

        // POST: api/user/login
        [HttpGet("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO login)
        {
            var result = await _userService.Login(login);
            return new JsonResult(result);
        }

        //Get: api/users
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var result = await _userService.GetAllUsers();
            return new JsonResult(result);
        }

        [HttpPatch("change-password")]
        public async Task<IActionResult> ChangePassword(Guid userId, string newPassword)
        {
            var result = await _userService.ChangePassword(userId, newPassword);
            return new JsonResult(result);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUser([FromQuery] Guid id)
        {
            var result = await _userService.DeleteUser(id);
            return new JsonResult(result);
        }

    }
}

