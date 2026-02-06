using CustomerSupport.API.DTOs;

namespace CustomerSupport.API.Services
{
    public interface IUserService
    {
        Task<List<UserDto>> GetAdminUsersAsync();
        Task<UserDto?> GetUserByIdAsync(int userId);
    }
}