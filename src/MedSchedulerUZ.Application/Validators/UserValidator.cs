using FluentValidation;
using MedSchedulerUZ.Core.Entities;

namespace MedSchedulerUZ.Application.Validators
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(user => user)
                .NotEmpty();
        }
    }
}
