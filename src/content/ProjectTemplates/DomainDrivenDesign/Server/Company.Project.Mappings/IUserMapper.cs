using Company.Project.Domain.Entities;
using Company.Project.Shared.Models;
using Mapster;

namespace Company.Project.Mappings;

[Mapper]
public interface IUserMapper
{
    UserDto ToDto(User entity);
    User ToEntity(UserDto dto);
}
