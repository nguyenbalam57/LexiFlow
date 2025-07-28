using FluentValidation;
using LexiFlow.API.DTOs.Auth;

namespace LexiFlow.API.Validators.AuthValidators
{
    /// <summary>
    /// Validator cho RegisterRequest
    /// </summary>
    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username is required")
                .Length(3, 50).WithMessage("Username must be between 3 and 50 characters")
                .Matches("^[a-zA-Z0-9_-]*$").WithMessage("Username can only contain letters, numbers, underscores and hyphens");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .Length(6, 100).WithMessage("Password must be between 6 and 100 characters")
                .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter")
                .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter")
                .Matches("[0-9]").WithMessage("Password must contain at least one number");

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithMessage("Confirm password is required")
                .Equal(x => x.Password).WithMessage("Passwords do not match");

            RuleFor(x => x.Email)
                .EmailAddress().When(x => !string.IsNullOrEmpty(x.Email)).WithMessage("Invalid email address")
                .Length(0, 255).WithMessage("Email cannot exceed 255 characters");

            RuleFor(x => x.FirstName)
                .Length(0, 100).WithMessage("First name cannot exceed 100 characters");

            RuleFor(x => x.LastName)
                .Length(0, 100).WithMessage("Last name cannot exceed 100 characters");
        }
    }
}
