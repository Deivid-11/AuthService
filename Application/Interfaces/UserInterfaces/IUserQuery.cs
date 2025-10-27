using Application.Models.UserModels;
using Domain.Entities;

namespace Application.Interfaces.UserInterfaces
{
    public interface IUserQuery
    {
        Task<User> GetUser(Guid Id);
        Task<User> Login(string email, string password);
        Task<List<UserResponseDTO>> GetAllUsers();
        Task<bool> ExistUser(string email);
    }
}
