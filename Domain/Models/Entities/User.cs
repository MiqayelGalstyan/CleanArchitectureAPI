using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LayeredAPI.Domain.Models.Entities;

public class User
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Email { get; set; }
    
    public string Phone { get; set; }
    
    [Required(AllowEmptyStrings = false), MaxLength(200)]
    public string FirstName { get; set; }

    [Required(AllowEmptyStrings = false), MaxLength(200)]
    public string LastName { get; set; }
    
    public string ImagePath { get; set; }
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime LastUpdate { get; set; }
}