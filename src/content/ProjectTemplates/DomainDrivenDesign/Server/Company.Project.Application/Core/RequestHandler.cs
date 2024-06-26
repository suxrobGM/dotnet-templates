using Company.Project.Core.Cqrs;
using Company.Project.Application.Extensions;
using MediatR;

namespace Company.Project.Application.Core;

internal abstract class RequestHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : IResult, new()
{
    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken)
    {
        try
        {
            return await HandleValidated(request, cancellationToken);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Thrown an exception when handling the request {0}, exception: {1}", typeof(TRequest).Name, ex);
            return new TResponse { Error = ex.GetAllMessages() };
        }
    }

    protected abstract Task<TResponse> HandleValidated(TRequest req, CancellationToken ct);
}
