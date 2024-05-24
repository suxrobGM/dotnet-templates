using Company.Project.Application.Core;
using Company.Project.Domain.Entities;
using Company.Project.Domain.Persistence;
using Company.Project.Shared.Cqrs;

namespace Company.Project.Application.Commands;

internal class CreateUserHandler : RequestHandler<CreateUserCommand, Result>
{
    private readonly IUnitOfWork _uow;

    public CreateUserHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }
    
    protected override async Task<Result> HandleValidated(
        CreateUserCommand req, CancellationToken ct)
    {
        var newUser = User.Create(req.Email!, req.PhoneNumber!);
        
        
        await _uow.Repository<User>().AddAsync(newUser);
        await _uow.SaveChangesAsync(ct);
        return Result.Success();
    }
}
