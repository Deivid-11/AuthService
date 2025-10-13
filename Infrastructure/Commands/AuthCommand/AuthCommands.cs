using Application.Interfaces.AuthInterface;
using Application.Models.UserModel;
using Domain.Entities;
using Infrastructure.Persistence;

namespace Infrastructure.Commands.AuthCommand
{
    public class AuthCommands : IAuthCommand
    {
        private readonly AppDbContext _context;
        public AuthCommands(AppDbContext context)
        {
            _context = context;
        }

        public async Task<UserDTO> Insert(UserDTO user)
        {
            var userInsert = new User
            {
                Id = Guid.NewGuid(),
                Name = user.Name,
                LastName = user.LastName,
                Email = user.Email,
                Password = user.Password,
                Role = user.Role,
            };
            _context.Users.Add(userInsert);
            await _context.SaveChangesAsync();
            return await Task.FromResult(user);
        }

    }
}
