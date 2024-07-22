using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LayeredAPI.Domain.Models.Entities;

public class Permission
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public int RoleId { get; set; }
    [ForeignKey("RoleId")]
    public Role Role { get; set; }
}