using %NAMESPACE%.Application.Core;
using %NAMESPACE%.Domain.Entities;
using %NAMESPACE%.Domain.Persistence;
using %NAMESPACE%.Shared.Cqrs;

namespace %NAMESPACE%.Queries;

internal class ActionNameHandler : RequestHandler<ActionNameQuery, Result>
{
    private readonly IUnitOfWork _uow;

    public ActionNameHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }
    
    protected override Task<Result> HandleValidated(
        ActionNameQuery req, CancellationToken ct)
    {
        return Task.FromResult(Result.Success());
    }
}
