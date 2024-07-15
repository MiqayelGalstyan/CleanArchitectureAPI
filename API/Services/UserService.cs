using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Domain.Utils;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Domain.Interfaces.Repositories.UserRepository;
using LayeredAPI.Domain.Models.Entities.User;
using LayeredAPI.Domain.Models.Request;
using LayeredAPI.Domain.Models.Response;
using LayeredAPI.Domain.Models.Response.LoginResponse;
using LayeredAPI.Domain.Models.Response.RegisterUserResponse;
using LayeredAPI.Infrastructure.Mappers.UserMapper;

namespace LayeredAPI.Application.Services.UserService;

public class UserService
{
    private readonly IUserRepository _userRepository;
    private readonly UserMapper _mapper;

    public UserService(IUserRepository userRepository,UserMapper mapper)
    {
        _mapper = mapper;
        _userRepository = userRepository;
    }

    public async Task<LoginResponse> Login(LoginRequest loginRequest, string secretKey)
    {
        var user = await _userRepository.GetUserByEmailAsync(loginRequest.Email);
        var isPasswordVerified =
            !PasswordUtility.VerifyPasswordHash(loginRequest.Password, user.PasswordHash, user.PasswordSalt);

        if (user == null)
        {
            throw new InvalidOperationException("Invalid email or password.");
        }

        if (isPasswordVerified)
        {
            var token = GenerateToken(user, secretKey);
            return _mapper.ToLoginResponse(user, token);
        }

        throw new InvalidOperationException("Something went wrong");
    }

    public async Task<RegisterUserResponse> RegisterAsync(RegisterUserRequest request)
    {
        PasswordUtility.CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
       
        var user = _mapper.ToUser(request);
        user.PasswordHash = passwordHash;
        user.PasswordSalt = passwordSalt;
        
        await _userRepository.AddUserAsync(user);

        return _mapper.ToRegisterUserResponse(user);
    }

    private string GenerateToken(User user, string secretKey)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        };
        var token = new JwtSecurityToken(
            issuer: null,
            audience: null,
            claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: credentials);


        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}