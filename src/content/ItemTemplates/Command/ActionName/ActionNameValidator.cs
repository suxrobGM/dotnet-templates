using FluentValidation;

namespace %NAMESPACE%.Commands;

internal class ActionNameValidator : AbstractValidator<ActionNameCommand>
{
    public ActionNameValidator()
    {
        RuleFor(i => i.Id).NotEmpty();
    }
}
