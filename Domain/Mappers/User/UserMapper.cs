using LayeredAPI.Domain.Models.Request;
using Riok.Mapperly.Abstractions;
using LayeredAPI.Domain.Models.Entities;
using LayeredAPI.Domain.Models.Response;

namespace LayeredAPI.Domain.Mappers;

[Mapper]
    public partial class UserMapper
    {
        public partial User MapUser(RegisterUserRequest request);
        public partial User MapUserByAdmin(RegisterUserByAdminRequest request);
        public partial RegisterUserResponse MapRegisterUserResponse(User user);
        public LoginResponse MapLoginResponse(User user, string token)
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
