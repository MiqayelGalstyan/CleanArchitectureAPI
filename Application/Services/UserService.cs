using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Domain.Utils;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Security.Cryptography;
using LayeredAPI.Domain.Interfaces.Repositories;
using LayeredAPI.Domain.Interfaces.Services;
using LayeredAPI.Domain.Models.Entities;
using LayeredAPI.Domain.Models.Request;
using LayeredAPI.Domain.Models.Response;
using LayeredAPI.Domain.Mappers;

namespace LayeredAPI.Application.Services;

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

    public async Task<TokenResponse> Login(LoginRequest loginRequest, string secretKey)
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

        var token = GenerateAccessToken(user, secretKey, loginRequest.isRemembered);
        var refreshToken = GenerateRefreshToken();

        var accessToken = "Bearer " + token;

        return _mapper.MapTokenResponse(accessToken, refreshToken);
    }

    public async Task<TokenResponse> RefreshToken(string token, string secretKey)
    {
        JwtSecurityTokenHandler handler = new();
        JwtSecurityToken jwtSecurityToken = handler.ReadJwtToken(token.Replace("Bearer ", string.Empty));
        var userId = jwtSecurityToken.Claims.Select(x => x.Value).FirstOrDefault();

        if (string.IsNullOrEmpty(userId))
        {
            throw new InvalidOperationException("Invalid refresh token.");
        }


        bool isTokenExpired = jwtSecurityToken.ValidTo < DateTime.UtcNow;

        var user = await _userRepository.GetUser(int.Parse(userId));

        string newAccessToken = "";
        string newRefreshToken = "";

        if (isTokenExpired)
        {
            var generatedToken = GenerateAccessToken(user, secretKey, false);
            newAccessToken = "Bearer " + generatedToken;
            newRefreshToken = GenerateRefreshToken();
        }


        return _mapper.MapTokenResponse(newAccessToken, newRefreshToken);
    }


    public async Task<RegisterUserResponse> RegisterAsync(RegisterUserRequest request)
    {
        PasswordUtility.CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

        var user = _mapper.MapRegisteredUser(request);
        var role = await _roleRepository.GetRoleByName("User");

        user.PasswordHash = passwordHash;
        user.PasswordSalt = passwordSalt;
        user.RoleId = role.Id;

        await _userRepository.AddUserAsync(user);

        return _mapper.MapRegisterUserResponse(user);
    }

    public async Task<PagedResult<UserResponse>> GetUsers(GetUsersRequest getUsersRequest)
    {
        var users = await _userRepository.GetUsers(getUsersRequest);
        var userResponses = users.Select(user => _mapper.MapUser(user)).ToList();

        var totalUsers = await _userRepository.GetUsersCount(getUsersRequest.SearchQuery);

        return new PagedResult<UserResponse>
        {
            Items = userResponses,
            TotalCount = totalUsers,
            Page = getUsersRequest.Page,
            Limit = getUsersRequest.Limit
        };
    }

    public async Task<UserResponse> GetUser(int id)
    {
        var user = await _userRepository.GetUser(id);
        return _mapper.MapUser(user);
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


    private string GenerateAccessToken(User user, string secret, bool isRemembered)
    {
        if (user.Role == null)
        {
            throw new InvalidOperationException("User role not found.");
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var tokenExpiresTime = isRemembered == false ? DateTime.UtcNow.AddHours(2) : DateTime.UtcNow.AddHours(48);

        List<Claim> claims = new()
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Role, user.Role.Name),
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = tokenExpiresTime,
            SigningCredentials = credentials,
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }


    private static string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}