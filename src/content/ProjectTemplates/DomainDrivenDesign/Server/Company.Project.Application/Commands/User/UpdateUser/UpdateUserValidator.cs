using FluentValidation;

namespace Company.Project.Application.Commands;

internal class UpdateUserValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserValidator()
    {
        RuleFor(i => i.Id)
            .NotEmpty();
    }
}
