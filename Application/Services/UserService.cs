using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Domain.Utils;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using LayeredAPI.Domain.Interfaces.Repositories;
using LayeredAPI.Domain.Interfaces.Services;
using LayeredAPI.Domain.Models.Entities;
using LayeredAPI.Domain.Models.Request;
using LayeredAPI.Domain.Models.Response;
using LayeredAPI.Domain.Mappers;

namespace LayeredAPI.Infrastructure.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly UserMapper _mapper;

    public UserService(IUserRepository userRepository, IRoleRepository roleRepository, UserMapper mapper)
    {
        _mapper = mapper;
        _userRepository = userRepository;
        _roleRepository = roleRepository;
    }

    public async Task<LoginResponse> Login(LoginRequest loginRequest, string secretKey)
    {
        var user = await _userRepository.GetUserByEmailAsync(loginRequest.Email);
        
        
        if (user == null)
        {
            throw new InvalidOperationException("Invalid email or password.");
        }
   
        
        var isPasswordVerified =
            PasswordUtility.VerifyPasswordHash(loginRequest.Password, user.PasswordHash, user.PasswordSalt);
        
        if (!isPasswordVerified)
        {
            throw new InvalidOperationException("Password not verified");
        }

        var token = GenerateToken(user, secretKey);

        return _mapper.MapLoginResponse(user, token);
    }

    public async Task<RegisterUserResponse> RegisterAsync(RegisterUserRequest request)
    {
        PasswordUtility.CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

        var user = _mapper.MapUser(request);
        var role = await _roleRepository.GetRoleByName("User");

        user.PasswordHash = passwordHash;
        user.PasswordSalt = passwordSalt;
        user.RoleId = role.Id;

        await _userRepository.AddUserAsync(user);

        return _mapper.MapRegisterUserResponse(user);
    }
    
    public async Task<RegisterUserResponse> RegisterByAdminAsync(RegisterUserByAdminRequest request)
    {
        PasswordUtility.CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

        var user = _mapper.MapUserByAdmin(request);

        user.PasswordHash = passwordHash;
        user.PasswordSalt = passwordSalt;

        await _userRepository.AddUserAsync(user);

        return _mapper.MapRegisterUserResponse(user);
    }
    

    private string GenerateToken(User user, string secretKey)
    {
        
        if (user.Role == null)
        {
            throw new InvalidOperationException("User role is not loaded.");
        }
        
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Role, user.Role.Name),
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