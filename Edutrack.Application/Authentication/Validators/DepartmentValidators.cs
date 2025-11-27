using EduTrack.Application.Authentication.Commands;
using FluentValidation;
public class CreateDepartmentValidator : AbstractValidator<CreateDepartmentCommand>
{
    public CreateDepartmentValidator()
    {
        RuleFor(x => x.Dto.DepartmentName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Dto.Description).MaximumLength(255);
    }
}
public class UpdateDepartmentValidator : AbstractValidator<UpdateDepartmentCommand>
{
    public UpdateDepartmentValidator()
    {
        RuleFor(x => x.Dto.DepartmentId).GreaterThan(0);
        RuleFor(x => x.Dto.DepartmentName).MaximumLength(50)
            .When(x => x.Dto.DepartmentName != null);
        RuleFor(x => x.Dto.Description).MaximumLength(255)
            .When(x => x.Dto.Description != null);
    }
}