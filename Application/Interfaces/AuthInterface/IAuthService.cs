using Application.Models.UserModel;
using Domain.Entities;

namespace Application.Interfaces.AuthInterface
{
    public interface IAuthService
    {
        Task<UserResponseDTO> Register(UserRequestDTO registerUserDTO);
        Task<UserResponseDTO> Login(LoginUserDTO loginUserDTO);
        Task<UserResponseDTO> GetCurrentUser(Guid userId);
    }
}
