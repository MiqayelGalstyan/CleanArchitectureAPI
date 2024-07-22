using LayeredAPI.Domain.Models.Entities;
using LayeredAPI.Domain.Models.Response;
using Riok.Mapperly.Abstractions;

namespace LayeredAPI.Domain.Mappers;

[Mapper]
public partial class RoleMapper
{
    public partial RoleResponse MapRole(Role role);
}