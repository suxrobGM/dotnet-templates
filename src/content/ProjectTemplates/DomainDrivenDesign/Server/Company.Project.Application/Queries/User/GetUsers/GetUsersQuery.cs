using Company.Project.Shared.Cqrs;
using Company.Project.Shared.Models;
using MediatR;

namespace Company.Project.Application.Queries;

public class GetUsersQuery : PagedQuery, IRequest<PagedResult<UserDto>>
{
}
