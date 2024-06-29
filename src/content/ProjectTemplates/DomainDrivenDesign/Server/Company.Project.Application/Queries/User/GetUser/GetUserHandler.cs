using Company.Project.Application.Core;
using Company.Project.Domain.Entities;
using Company.Project.Domain.Persistence;
using Company.Project.Mappings;
using Company.Project.Core.Cqrs;
using Company.Project.Shared.Models;

namespace Company.Project.Application.Queries;

internal class GetUserHandler : RequestHandler<GetUserQuery, Result<UserDto>>
{
    private readonly IUnitOfWork _uow;

    public GetUserHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }
    
    protected override async Task<Result<UserDto>> HandleValidated(
        GetUserQuery req, CancellationToken ct)
    {
        var userEntity = await _uow.Repository<User>().GetAsync(i => i.Id == req.Id);

        if (userEntity is null)
        {
            return Result<UserDto>.Fail($"Could not find a user with ID {req.Id}");
        }

        var userDto = userEntity.ToDto();
        return Result<UserDto>.Succeed(userDto);
    }
}
