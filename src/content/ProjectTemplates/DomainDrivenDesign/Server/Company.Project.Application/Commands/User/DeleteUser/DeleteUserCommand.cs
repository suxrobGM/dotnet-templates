using Company.Project.Shared.Cqrs;
using MediatR;

namespace Company.Project.Application.Commands;

public class DeleteUserCommand : IRequest<Result>
{
    public string? Id { get; set; }
}
