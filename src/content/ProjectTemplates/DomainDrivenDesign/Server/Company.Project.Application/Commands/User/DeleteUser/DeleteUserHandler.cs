using Company.Project.Application.Core;
using Company.Project.Domain.Entities;
using Company.Project.Domain.Persistence;
using Company.Project.Shared.Cqrs;

namespace Company.Project.Application.Commands;

internal class DeleteUserHandler : RequestHandler<DeleteUserCommand, Result>
{
    private readonly IUnitOfWork _uow;

    public DeleteUserHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }
    
    protected override async Task<Result> HandleValidated(
        DeleteUserCommand req, CancellationToken ct)
    {
        var user = await _uow.Repository<User>()
            .GetAsync(i => i.Id == req.Id);

        if (user is null)
        {
            return Result.Failure($"Could not find a user with ID {req.Id}");
        }
        
        _uow.Repository<User>().Delete(user);
        await _uow.SaveChangesAsync(ct);
        return Result.Success();
    }
}
