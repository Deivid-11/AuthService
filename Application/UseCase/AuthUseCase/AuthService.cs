using Application.Interfaces.AuthInterface;
using Application.Models.UserModel;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCase.AuthUseCase
{
    public class AuthService : IAuthService
    {
        private readonly IAuthCommand _authCommand;
        private readonly IAuthQuery _authQuery;
        public AuthService(IAuthCommand authCommand, IAuthQuery authQuery)
        {
            _authCommand = authCommand;
            _authQuery = authQuery;
        }
        public async Task<UserDTO> GetCurrentUser(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                throw new ArgumentException("El ID del usuario no puede ser vacío");
            }
            return await _authQuery.GetById(userId);
        }

        public async Task<UserDTO> Login(LoginUserDTO loginUserDTO)
        {
            if (loginUserDTO == null)
            {
                throw new ArgumentException("El usuario no puede ser nulo");
            }
            return await _authQuery.Get(loginUserDTO);
        }

        public async Task<UserDTO> Register(UserDTO user)
        {
            if (user == null)
            {
                throw new ArgumentException("the user cannot be null");
            }
            return await _authCommand.Insert(user);
        }
    }
}
