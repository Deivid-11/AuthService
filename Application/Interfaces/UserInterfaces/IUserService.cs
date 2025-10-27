using Application.Models.UserModels;

namespace Application.Interfaces.UserInterfaces
{
    public interface IUserService
    {
        Task<UserResponseDTO> Login(UserLoginDTO login);
        Task<List<UserResponseDTO>> GetAllUsers();
        Task<UserResponseDTO> RegisterUser(UserRequestDTO user);
        Task<UserResponseDTO> ChangePassword(Guid userId, string newPassword);
        Task<UserResponseDTO> DeleteUser(Guid id);
        Task<bool> ExistUser(string email);
    }
}
