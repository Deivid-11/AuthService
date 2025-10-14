using Application.Models.UserModel;

namespace Application.Interfaces.AuthInterface
{
    public interface IAuthQuery
    {
        Task<UserResponseDTO> Get(LoginUserDTO login);
        Task<UserResponseDTO> GetById(Guid id);
    }
}
