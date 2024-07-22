namespace LayeredAPI.Domain.Models.Request;

public class RegisterUserByAdminRequest:BaseRegisterUserRequest
{
    public int RoleId { get; set; }
}