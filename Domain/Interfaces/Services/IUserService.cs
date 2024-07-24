using LayeredAPI.Domain.Models.Entities;
using LayeredAPI.Domain.Models.Request;
using LayeredAPI.Domain.Models.Response;

namespace LayeredAPI.Domain.Interfaces.Services;

public interface IUserService
{
    public  Task<LoginResponse> Login(LoginRequest loginRequest, string secretKey);
    public Task<RegisterUserResponse> RegisterAsync(RegisterUserRequest request);
    
    Task<List<UserResponse>> GetUsers();
    
    public Task<RegisterUserResponse> RegisterByAdminAsync(RegisterUserByAdminRequest request);
}