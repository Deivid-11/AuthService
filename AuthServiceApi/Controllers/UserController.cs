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

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // POST: api/user/register
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRequestDTO user)
        {
            if (await _userService.ExistUser(user.Email))
            {
                throw new ArgumentException("Existe usuario asociado al mail ingresado");
            }
            var result = await _userService.RegisterUser(user);
            return new JsonResult(result);
        }

        // Get: api/user/login
        [HttpGet("login")]
        public async Task<IActionResult> Login(UserLoginDTO login)
        {
            var result = await _userService.Login(login);
            return new JsonResult(result);
        }

        //Get: api/user/all
        [HttpGet("all")]
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
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var result = await _userService.DeleteUser(id);
            return new JsonResult(result);
        }

    }
}

