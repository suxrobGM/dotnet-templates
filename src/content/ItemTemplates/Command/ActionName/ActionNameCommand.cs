using %NAMESPACE%.Shared.Cqrs;
using MediatR;

namespace %NAMESPACE%.Commands;

public class ActionNameCommand : IRequest<Result>
{
    public string? Id { get; set; }
}
