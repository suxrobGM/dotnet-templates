using FluentValidation;

namespace Company.Project.Application.Queries;

internal class GetUsersValidator : AbstractValidator<GetUsersQuery>
{
    public GetUsersValidator()
    {
        RuleFor(i => i.Page).GreaterThanOrEqualTo(0);
        RuleFor(i => i.PageSize).GreaterThanOrEqualTo(1);
    }
}
