using LayeredAPI.Domain.Models.Response;

namespace LayeredAPI.Domain.Interfaces.Services;

public interface IProfileService
{
    public Task<ProfileResponse> GetProfile(int id);
}