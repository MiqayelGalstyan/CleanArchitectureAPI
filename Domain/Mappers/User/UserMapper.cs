using LayeredAPI.Domain.Models.Request;
using Riok.Mapperly.Abstractions;
using LayeredAPI.Domain.Models.Entities;
using LayeredAPI.Domain.Models.Response;

namespace LayeredAPI.Domain.Mappers;

[Mapper]
public partial class UserMapper
{
    public partial User MapRegisteredUser(RegisterUserRequest request);

    public UserResponse MapUser(User user)
    {
        return new UserResponse
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            ImagePath = user.ImagePath,
            Id = user.Id,
            RoleId = user.RoleId,
            RoleName = user.Role.Name,
        };
    }
    public partial User MapUserByAdmin(RegisterUserByAdminRequest request);
    public partial RegisterUserResponse MapRegisterUserResponse(User user);

    public TokenResponse MapTokenResponse(string accessToken, string refreshToken)
    {
        return new TokenResponse
        {
            Token = accessToken,
            RefreshToken = refreshToken,
        };
    }
}