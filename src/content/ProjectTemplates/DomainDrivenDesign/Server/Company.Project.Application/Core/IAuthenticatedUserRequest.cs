using MediatR;

namespace Company.Project.Application.Core;

internal interface IAuthenticatedUserRequest<out TResult> : IRequest<TResult>
{
    /// <summary>
    /// Authenticated user ID
    /// </summary>
    string? AuthUserId { get; set; }
}
