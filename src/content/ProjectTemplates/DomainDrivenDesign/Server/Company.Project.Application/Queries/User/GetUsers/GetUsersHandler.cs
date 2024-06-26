using Company.Project.Application.Core;
using Company.Project.Domain.Entities;
using Company.Project.Domain.Persistence;
using Company.Project.Domain.Specifications;
using Company.Project.Mappings;
using Company.Project.Mappings;
using Company.Project.Core.Cqrs;
using Company.Project.Shared.Models;

namespace Company.Project.Application.Queries;

internal class GetUsersHandler : RequestHandler<GetUsersQuery, PagedResult<UserDto>>
{
    private readonly IUnitOfWork _uow;

    public GetUsersHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }
    
    protected override async Task<PagedResult<UserDto>> HandleValidated(
        GetUsersQuery req, CancellationToken ct)
    {
        var totalItems = await _uow.Repository<User>().CountAsync();

        var users = _uow.Repository<User>()
            .ApplySpecification(new GetUsersPaged(req.OrderBy, req.Page, req.PageSize))
            .Select(i => i.ToDto())
            .ToArray();
        
        return PagedResult<UserDto>.Success(users, totalItems, req.PageSize);
    }
}
