using LayeredAPI.Domain.Models.Request;
using Riok.Mapperly.Abstractions;
using LayeredAPI.Domain.Models.Entities.User;
using LayeredAPI.Domain.Models.Response.RegisterUserResponse;
using LayeredAPI.Domain.Models.Response.LoginResponse;

namespace LayeredAPI.Infrastructure.Mappers.UserMapper;

[Mapper]
    public partial class UserMapper
    {
        public partial User ToUser(RegisterUserRequest request);
        public partial RegisterUserResponse ToRegisterUserResponse(User user);
        
        public LoginResponse ToLoginResponse(User user, string token)
        {
            return new LoginResponse
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                ImagePath = user.ImagePath,
                Token = token,
            };
        }
    }
