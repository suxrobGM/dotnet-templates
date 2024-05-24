using FluentValidation;

namespace %NAMESPACE%.Queries;

internal class ActionNameValidator : AbstractValidator<ActionNameQuery>
{
    public ActionNameValidator()
    {
        RuleFor(i => i.Id).NotEmpty();
    }
}
