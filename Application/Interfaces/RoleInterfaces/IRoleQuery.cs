using Application.Models.RoleModels;

namespace Application.Interfaces.RoleInterfaces
{
    public interface IRoleQuery
    {
        Task<RoleResponseDTO> GetRoleById(int roleId);
        Task<List<RoleResponseDTO>> GetAll();
        Task<bool> RoleExists(int roleId);
    }
}
