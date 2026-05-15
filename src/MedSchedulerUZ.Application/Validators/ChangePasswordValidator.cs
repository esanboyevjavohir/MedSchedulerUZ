using FluentValidation;
using MedSchedulerUZ.Application.Models.User;

namespace MedSchedulerUZ.Application.Validators
{
    public class ChangePasswordValidator : AbstractValidator<ChangePasswordModel>
    {
        public ChangePasswordValidator()
        {
            RuleFor(x => x.OldPassword)
                .NotEmpty().WithMessage("Eski parol bo'sh bo'lmasligi kerak");

            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("Yangi parol bo'sh bo'lmasligi kerak")
                .MinimumLength(8).WithMessage("Parol kamida 8 ta belgidan iborat bo'lishi kerak")
                .Matches("[A-Z]").WithMessage("Parol kamida bitta katta harf bo'lishi kerak")
                .Matches("[a-z]").WithMessage("Parol kamida bitta kichik harf bo'lishi kerak")
                .Matches("[0-9]").WithMessage("Parol kamida bitta raqam bo'lishi kerak")
                .Matches("[^a-zA-Z0-9]").WithMessage("Parol kamida bitta maxsus belgi bo'lishi kerak");
        }
    }
}
