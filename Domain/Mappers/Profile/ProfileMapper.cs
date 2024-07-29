using LayeredAPI.Domain.Models.Entities;
using LayeredAPI.Domain.Models.Response;
using Riok.Mapperly.Abstractions;

namespace Domain.Mappers.Profile;

[Mapper]
public partial class ProfileMapper
{
    public ProfileResponse MapUserProfile(User user)
    {
        return new ProfileResponse
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            ImagePath = user.ImagePath,
            Id = user.Id,
            RoleId = user.RoleId,
        };
    }
}