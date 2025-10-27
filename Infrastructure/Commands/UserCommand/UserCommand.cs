using Application.Interfaces.UserInterfaces;
using Application.Models.UserModels;
using Domain.Entities;
using Infrastructure.Persistence;

namespace Infrastructure.Commands.UserCommand
{
    public class UserCommand : IUserCommand
    {
        private readonly AppDbContext _context;
        public UserCommand(AppDbContext context)
        {
            _context = context;
        }

        public async Task<UserResponseDTO> DeleteUser(User user)
        {
            _context.Remove(user);
            await _context.SaveChangesAsync();
            return new UserResponseDTO
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Password = user.Password,
                Phone = user.Phone,
                RoleId = user.RoleId,
                RoleName = user.Role.Name
            };
        }

        public async Task<UserResponseDTO> InsertUser(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            UserResponseDTO userResponse = new UserResponseDTO
            {
                Id = user.Id,
                Name = user.Name,
                LastName = user.LastName,
                Email = user.Email,
                Password = user.Password,
                Phone = user.Phone,
                RoleId = user.RoleId
            };
            return userResponse;
        }

        public async Task<UserResponseDTO> UpdateUser(User user)
        {
            _context.Update(user);
            await _context.SaveChangesAsync();
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
    }
}
