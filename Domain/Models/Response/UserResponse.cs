namespace LayeredAPI.Domain.Models.Response;

public class UserResponse
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string ImagePath { get; set; }
    public int RoleId { get; set; }
    public string RoleName { get; set; }
}