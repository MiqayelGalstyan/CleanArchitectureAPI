using LayeredAPI.Domain.Models.Request;
using LayeredAPI.Domain.Models.Response;

namespace LayeredAPI.Domain.Interfaces.Services;

public interface IUserService
{
    public  Task<LoginResponse> Login(LoginRequest loginRequest, string secretKey);
    public Task<RegisterUserResponse> RegisterAsync(RegisterUserRequest request);
}