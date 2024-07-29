using Domain.Mappers.Profile;
using LayeredAPI.Domain.Interfaces.Repositories;
using LayeredAPI.Domain.Interfaces.Services;
using LayeredAPI.Domain.Models.Response;

namespace LayeredAPI.Infrastructure.Services;

public class ProfileService : IProfileService
{
    
    private readonly IProfileRepository _profileRepository;
    private readonly ProfileMapper _mapper;

    public ProfileService(IProfileRepository profileRepository, ProfileMapper mapper)
    {
        _mapper = mapper;
        _profileRepository = profileRepository;
    }
    
    
    public async Task<ProfileResponse> GetProfile(int id)
    {
        var userProfile = await _profileRepository.GetUserProfile(id);
        return _mapper.MapUserProfile(userProfile);
    }
}