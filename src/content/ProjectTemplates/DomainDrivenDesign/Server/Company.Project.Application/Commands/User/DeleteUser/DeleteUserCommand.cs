using Company.Project.Core.Cqrs;
using MediatR;

namespace Company.Project.Application.Commands;

public class DeleteUserCommand : IRequest<Result>
{
    public string? Id { get; set; }
}
