using Domain.Mappers.Profile;
using Domain.Utils;
using LayeredAPI.Domain.Interfaces.Repositories;
using LayeredAPI.Domain.Interfaces.Services;
using LayeredAPI.Domain.Models.Request;
using LayeredAPI.Domain.Models.Response;

namespace LayeredAPI.Application.Services;

public class ProfileService : IProfileService
{
    
    private readonly IProfileRepository _profileRepository;
    private readonly IUserRepository _userRepository;
    private readonly ProfileMapper _mapper;

    public ProfileService(IProfileRepository profileRepository, IUserRepository userRepository, ProfileMapper mapper)
    {
        _mapper = mapper;
        _profileRepository = profileRepository;
        _userRepository = userRepository;
    }
    
    
    public async Task<ProfileResponse> GetProfile(int id)
    {
        var userProfile = await _profileRepository.GetUserProfile(id);
        return _mapper.MapUserProfile(userProfile);
    }

    public async Task<ProfileResponse> EditProfile(int id, EditProfileRequest editProfileRequest)
    {
        var user = await _userRepository.GetUser(id);

        if (user == null)
        {
            throw new InvalidOperationException("User not found.");
        }

        user.Email = editProfileRequest.Email;
        user.FirstName = editProfileRequest.FirstName;
        user.LastName = editProfileRequest.LastName;


        if (!string.IsNullOrEmpty(editProfileRequest.ImagePath))
        {

            var imageRequest = new ImageRequest()
            {
                Base64Image = editProfileRequest.ImagePath,
                Extension = "png",
            };
            
            string imagePath = FileUploadHelper.ImageUpload(imageRequest, "profile");
            user.ImagePath = imagePath;
        }
       

        await _userRepository.UpdateUserAsync(user);

        return _mapper.MapUserProfile(user);
    }
}