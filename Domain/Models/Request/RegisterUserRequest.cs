using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace LayeredAPI.Domain.Models.Request;

public class RegisterUserRequest
{
    public string Phone { get; set; }
    
    [Required(AllowEmptyStrings = false), MaxLength(200)]
    public string FirstName { get; set; }

    [Required(AllowEmptyStrings = false), MaxLength(200)]
    public string LastName { get; set; }
    
    public string ImagePath { get; set; }
    
    [JsonProperty("email")]
    [Required(AllowEmptyStrings = false), MaxLength(50)]
    [RegularExpression(pattern: @"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Invalid email format.")]
    public string Email { get; set; }

    [JsonProperty("password")]
    [Required(AllowEmptyStrings = false), MaxLength(200)]
    public string Password { get; set; }
    
}