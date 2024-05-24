using %NAMESPACE%.Shared.Cqrs;
using MediatR;

namespace %NAMESPACE%.Queries;

public class ActionNameQuery : IRequest<Result>
{
    public string? Id { get; set; }
}
