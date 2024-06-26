using Company.Project.Application.Core;
using Company.Project.Domain.Entities;
using Company.Project.Domain.Identity;
using Company.Project.Domain.Persistence;
using Company.Project.Core.Cqrs;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Company.Project.Application.Behaviours;

internal sealed class SetAuthenticatedUserBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IAuthenticatedUserRequest<TResponse>
    where TResponse : IResult, new()
{
    private readonly AuthenticatedUserData _authenticatedUserData;
    private readonly IUnitOfWork _uow;
    private readonly ILogger<SetAuthenticatedUserBehaviour<TRequest, TResponse>> _logger;

    public SetAuthenticatedUserBehaviour(
        AuthenticatedUserData authenticatedUserData,
        IUnitOfWork uow,
        ILogger<SetAuthenticatedUserBehaviour<TRequest, TResponse>> logger)
    {
        _authenticatedUserData = authenticatedUserData;
        _uow = uow;
        _logger = logger;
    }
    
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var userExists = await _uow.Repository<User>()
            .AnyAsync(i => i.Id == request.AuthUserId || i.Email == request.AuthUserId || i.UserName == request.AuthUserId);

        if (!userExists)
        {
            var errorMsg = string.IsNullOrEmpty(request.AuthUserId) ? "Authenticated user ID is null" : "Invalid authenticated user ID";
            _logger.LogError("Unauthorized operation: {ErrorMessage}\nRequest: {Request}", 
                errorMsg, typeof(TRequest).Name);
            
            return new TResponse
            {
                Error = $"Unauthorized operation: {errorMsg}." 
            };
        }
            
        _authenticatedUserData.UserId = request.AuthUserId;
        return await next();
    }
}
