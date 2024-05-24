using Company.Project.Domain.Entities;
using Company.Project.Shared.Models;

namespace Company.Project.Mappings;

public static class UserExt
{
    private static readonly UserMapper Mapper = new();
    
    public static UserDto ToDto(this User entity)
    {
        return Mapper.ToDto(entity);
    }
    
    public static User ToEntity(this UserDto dto)
    {
        return Mapper.ToEntity(dto);
    }
}
