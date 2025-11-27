using EduTrack.Application.Authentication.Commands;
using FluentValidation;
public class CreateTeacherValidator : AbstractValidator<CreateTeacherCommand>
{
    public CreateTeacherValidator()
    {
        RuleFor(x => x.Dto.EmployeeId).GreaterThan(0);
        RuleFor(x => x.Dto.HireDate).NotEmpty();
        RuleFor(x => x.Dto.Specialty).MaximumLength(100);
        RuleFor(x => x.Dto.Degree).MaximumLength(100);
    }
}
public class UpdateTeacherValidator : AbstractValidator<UpdateTeacherCommand>
{
    public UpdateTeacherValidator()
    {
        RuleFor(x => x.Dto.TeacherId).GreaterThan(0);
        RuleFor(x => x.Dto.Specialty).MaximumLength(100)
            .When(x => x.Dto.Specialty != null);
        RuleFor(x => x.Dto.Degree).MaximumLength(100)
            .When(x => x.Dto.Degree != null);
    }
}