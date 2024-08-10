using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace LayeredAPI.Domain.Models.Request;

public class EditProfileRequest
{
    [JsonProperty("email")]
    [Required(AllowEmptyStrings = false), MaxLength(50)]
    [RegularExpression(pattern: @"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Invalid email format.")]
    public string Email { get; set; }

    [JsonProperty("firstName")]
    public string FirstName { get; set; }
    
    [JsonProperty("lastName")]
    public string LastName { get; set; }
    
    [JsonProperty("imagePath")]
    public string ImagePath { get; set; }
}