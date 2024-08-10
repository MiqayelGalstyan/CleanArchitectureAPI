using LayeredAPI.Domain.Models.Request;
using LayeredAPI.Domain.Models.Response;

namespace LayeredAPI.Domain.Interfaces.Services;

public interface IProfileService
{
    public Task<ProfileResponse> GetProfile(int id);

    public Task<ProfileResponse> EditProfile(int id, EditProfileRequest editProfileRequest);
}