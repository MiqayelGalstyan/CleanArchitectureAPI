using LayeredAPI.Domain.Models.Request;
using Riok.Mapperly.Abstractions;
using LayeredAPI.Domain.Models.Entities;
using LayeredAPI.Domain.Models.Response;

namespace LayeredAPI.Infrastructure.Mappers;

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
