using Application.Models.UserModels;
using Domain.Entities;

namespace Application.Interfaces.UserInterfaces
{
    public interface IUserCommand
    {
        Task<UserResponseDTO> InsertUser(User user);
        Task<UserResponseDTO> UpdateUser(User user);
        Task<UserResponseDTO> DeleteUser(User user);
    }
}
