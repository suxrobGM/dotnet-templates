using Company.Project.Core.Cqrs;
using MediatR;

namespace Company.Project.Application.Commands;

public class CreateUserCommand : IRequest<Result>
{
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
}
