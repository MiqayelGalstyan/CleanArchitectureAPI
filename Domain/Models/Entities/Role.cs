using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LayeredAPI.Domain.Models.Entities;

public class Role
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [Required(AllowEmptyStrings = false), MaxLength(300)]
    public string Name { get; set; }

    [Required(AllowEmptyStrings = false), MaxLength(300)]
    public string DisplayName { get; set; }
    
    public bool IsDeleted { get; set; }
    
    public ICollection<User> Users { get; set; }
    public ICollection<Permission> Permissions { get; set; }
    
}