using CustomerSupport.API.DTOs;

namespace CustomerSupport.API.Services
{
    public interface IAuthService
    {
        Task<LoginResponse> LoginAsync(LoginRequest request);
        string GenerateJwtToken(UserDto user);
        Task<UserDto?> GetUserByIdAsync(int userId);
    }
}