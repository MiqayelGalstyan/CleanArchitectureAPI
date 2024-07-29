using LayeredAPI.Domain.Models.Request;
using LayeredAPI.Domain.Models.Response;

namespace LayeredAPI.Domain.Interfaces.Services;

public interface IUserService
{
    public  Task<TokenResponse> Login(LoginRequest loginRequest, string secretKey);

    public Task<TokenResponse> RefreshToken(string token, string secretKey);
    public Task<RegisterUserResponse> RegisterAsync(RegisterUserRequest request);
    
    Task<List<UserResponse>> GetUsers();
    
    public Task<UserResponse> GetUser(int id);
    
    public Task<RegisterUserResponse> RegisterByAdminAsync(RegisterUserByAdminRequest request);
}