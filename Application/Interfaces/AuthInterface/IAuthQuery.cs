using Application.Models.UserModel;

namespace Application.Interfaces.AuthInterface
{
    public interface IAuthQuery
    {
        Task<UserDTO> Get(LoginUserDTO login);
        Task<UserDTO> GetById(Guid id);
    }
}
