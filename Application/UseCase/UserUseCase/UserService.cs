using Application.Interfaces.UserInterface;
using Application.Models.AuthModels.Login;
using Application.Models.AuthModels.Register;
using Application.Models.UserModels;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;

namespace Application.UseCase.UserUseCase
{
    public class UserService : IUserService
    {
        private readonly IUserCommand _userCommand;
        private readonly IUserQuery _userQuery;
        private readonly IConfiguration _configuration;
        public UserService(IUserCommand userCommand, IUserQuery userQuery, IConfiguration configuration)
        {
            _userCommand = userCommand;
            _userQuery = userQuery;
            _configuration = configuration;
        }
        public async Task<UserResponseDTO> DeleteUser(Guid userId)
        {
            await _userCommand.DeleteUser(userId);
            User user = await _userQuery.GetById(userId);
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

        public async Task<List<UserResponseDTO>> GetAllUsers()
        {
            List<User> users = await _userQuery.GetAllUsers();
            return users.Select(user => new UserResponseDTO
            {
                Id = user.Id,
                Name = user.Name,
                LastName = user.LastName,
                Email = user.Email,
                Password = user.Password,
                Phone = user.Phone,
                RoleId = user.RoleId,
                RoleName = user.Role.Name
            }).ToList();
        }

        public async Task<UserResponseDTO> GetUser(Guid userId)
        {
            User user = await _userQuery.GetById(userId);
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

        public async Task<string> LoginUser(LoginDTO request)
        {
            if (request.Email == null || request.Password == null)
            {
                throw new ArgumentException("Bad Request");
            }
            User user = await _userQuery.GetByEmail(request.Email);
            if (user == null)
            {
                throw new ArgumentException("Bad Request");
            }
            var hashedInput = encriptarSHA256(request.Password);

            if (user.Password != hashedInput)
                throw new ArgumentException("Bad Request");

            string token = GenerateJwtToken(new LoginResponseDTO
            {
                Id = user.Id,
                Email = user.Email,
                RoleName = user.Role.Name
            });

            return token;

        }

        public async Task<UserResponseDTO> RegisterUser(RegisterDTO request)
        {
            if (request.Email == null || request.Password == null || request.Name == null || request.Phone == null)
            {
                throw new ArgumentException("Bad Request");
            }
            User user = await _userCommand.InsertUser(new User
            {
                Name = request.Name,
                LastName = request.LastName,
                Email = request.Email,
                Password = encriptarSHA256(request.Password),
                Phone = request.Phone,
                RoleId = 1
            });
            return new UserResponseDTO
            {
                Id = user.Id,
                Name = user.Name,
                LastName = user.LastName,
                Email = user.Email,
                Password = user.Password,
                Phone = user.Phone,
                RoleId = user.RoleId
            };
        }


        public async Task<UserResponseDTO> UpdateUser(User userDto)
        {
            User user = await _userCommand.UpdateUser(new User
            {
                Id = userDto.Id,
                Name = userDto.Name,
                LastName = userDto.LastName,
                Email = userDto.Email,
                Password = encriptarSHA256(userDto.Password),
                Phone = userDto.Phone,
                RoleId = userDto.RoleId
            });
            return new UserResponseDTO
            {
                Id = user.Id,
                Name = user.Name,
                LastName = user.LastName,
                Email = user.Email,
                Password = user.Password,
                Phone = user.Phone,
                RoleId = user.RoleId
            };
        }

        private string encriptarSHA256(string texto)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(texto));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }

        private string GenerateJwtToken(LoginResponseDTO user)
        {
            var userClaims = new[]
            {
                new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.NameIdentifier, user.Id.ToString()),
                new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Email, user.Email),
                new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Role, user.RoleName),
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            var jwtConfig = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                claims: userClaims,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(jwtConfig);
        }
    }
}
