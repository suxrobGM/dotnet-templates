using FluentValidation;

namespace Company.Project.Application.Queries;

internal class GetUserValidator : AbstractValidator<GetUserQuery>
{
    public GetUserValidator()
    {
        RuleFor(i => i.Id).NotEmpty();
    }
}
