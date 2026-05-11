using FluentValidation;
using MedSchedulerUZ.Application.Models.User;
using MedSchedulerUZ.DataAccess.Persistence;
using System.Text.RegularExpressions;

namespace MedSchedulerUZ.Application.Validators
{
    public class CreateUserValidator : AbstractValidator<CreateUserModel>
    {
        public readonly DatabaseContext _dbContext;
        public CreateUserValidator(DatabaseContext dbContext)
        {
            _dbContext = dbContext;

            RuleFor(u => u.FullName)
            .Must(l => !string.IsNullOrWhiteSpace(l));

            RuleFor(u => u.Email)
            .Must(EmailIsUnique)
            .WithMessage("Email address is already in use");

            RuleFor(x => x.FullName)
           .NotEmpty().WithMessage("Name cannot be empty")
           .Length(2, 50).WithMessage("Name must be between 2 and 50 characters");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email cannot be empty")
                .EmailAddress().WithMessage("Invalid email format")
                .MaximumLength(100).WithMessage("Email must not exceed 100 characters");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("PhoneNumber cannot be empty")
                .MinimumLength(13).WithMessage("PhoneNumber must be at least 13 characters long")
                .Matches(@"^\+").WithMessage("PhoneNumber must start with '+'")
                .Matches(@"^\+\d+$").WithMessage("PhoneNumber must contain only digits after '+'");
        }

        private bool EmailIsUnique(string email)
        {
            bool emailExists = _dbContext.Users.Any(u => u.Email == email);
            return !emailExists;
        }

        public static bool IsValidPhoneNumber(string phoneNumber)
        {
            var phoneNumberRegex = new Regex(@"^998\d{9}$", RegexOptions.Compiled);

            if (string.IsNullOrWhiteSpace(phoneNumber))
            {
                return false;
            }

            return phoneNumberRegex.IsMatch(phoneNumber);
        }
    }
}
