using LayeredAPI.Domain.Models.Request;
using LayeredAPI.Domain.Models.Response;

namespace LayeredAPI.Domain.Interfaces.Services;

public interface IUserService
{
    public  Task<TokenResponse> Login(LoginRequest loginRequest, string secretKey);

    public Task<TokenResponse> RefreshToken(string token, string secretKey);
    public Task<RegisterUserResponse> RegisterAsync(RegisterUserRequest request);
    
    Task<PagedResult<UserResponse>> GetUsers(GetUsersRequest getUsersRequest);
    
    public Task<UserResponse> GetUser(int id);

    public Task<bool> DeleteUser(int id);
    
    public Task<RegisterUserResponse> RegisterByAdminAsync(RegisterUserByAdminRequest request);
}