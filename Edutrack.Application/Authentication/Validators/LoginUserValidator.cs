using EduTrack.Application.Authentication.Commands;
using FluentValidation;

public class LoginUserValidator : AbstractValidator<LoginUserCommand>
{
    public LoginUserValidator()
    {
        RuleFor(x => x.Username).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Password).NotEmpty().MaximumLength(255);
        //RuleFor(x => x.RememberMe).MaximumLength(255);
    }
}
