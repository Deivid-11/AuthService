using Application.Interfaces.UserInterfaces;
using Application.Models.UserModels;
using Domain.Entities;

namespace Application.UseCase.UserUseCase
{
    public class UserService : IUserService
    {
        private readonly IUserCommand _userCommand;
        private readonly IUserQuery _userQuery;
        public UserService(IUserCommand userCommand, IUserQuery userQuery)
        {
            _userCommand = userCommand;
            _userQuery = userQuery;
        }

        public async Task<UserResponseDTO> ChangePassword(Guid userId, string newPassword)
        {
            User user = await _userQuery.GetUser(userId);
            user.Password = newPassword;
            return await _userCommand.UpdateUser(user);
        }
        public async Task<UserResponseDTO> DeleteUser(Guid id)
        {
            User user = await _userQuery.GetUser(id);
            if (user == null)
            {
                throw new ArgumentException("User doesn´t exist");
            }
            return await _userCommand.DeleteUser(user);
        }

        public async Task<bool> ExistUser(string email)
        {
            if (email == null)
            {
                throw new ArgumentNullException("email");
            }
            return await _userQuery.ExistUser(email);
        }

        public async Task<List<UserResponseDTO>> GetAllUsers()
        {
            List<UserResponseDTO> users = await _userQuery.GetAllUsers();
            return users;
        }

        public async Task<UserResponseDTO> Login(UserLoginDTO login)
        {
            if ((login == null) || (login.Email == null) || (login.Password == null))
            {
                throw new ArgumentException("Bad Request");
            }
            User user = await _userQuery.Login(login.Email, login.Password);
            return new UserResponseDTO
            {
                Id = user.Id,
                Name = user.Name,
                LastName = user.LastName,
                Email = user.Email,
                Password = user.Password,
                Phone = user.Phone,
                RoleId = user.RoleId,
                RoleName = user.Role.Name
            };
        }

        public async Task<UserResponseDTO> RegisterUser(UserRequestDTO user)
        {
            if (_userQuery.ExistUser(user.Email).Result == true)
            {
                throw new ArgumentException("User already exist");
            }
            if ((user == null) || (user.Name == null) || (user.Email == null) || (user.Password == null)
                || (user.Phone == null) || (user.RoleId < 1) || (user.RoleId > 3) || (user.RoleId == null))
            {
                throw new ArgumentException("Bad Request");
            }
            User registerUser = new User
            {
                Name = user.Name,
                LastName = user.LastName,
                Email = user.Email,
                Password = user.Password,
                Phone = user.Phone,
                RoleId = user.RoleId
            };
            return await _userCommand.InsertUser(registerUser);
        }

        public Task<UserResponseDTO> UpdateUser(UserRequestDTO user)
        {
            throw new NotImplementedException();
        }
        
    }
}
