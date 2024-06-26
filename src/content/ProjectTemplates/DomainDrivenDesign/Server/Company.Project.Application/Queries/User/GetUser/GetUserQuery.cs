using Company.Project.Core.Cqrs;
using Company.Project.Shared.Models;
using MediatR;

namespace Company.Project.Application.Queries;

public class GetUserQuery : IRequest<Result<UserDto>>
{
    public required string Id { get; set; }
}
