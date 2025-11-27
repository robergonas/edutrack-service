using EduTrack.Application.Authentication.Commands;
using FluentValidation;

public class CreateEmployeeValidator : AbstractValidator<CreateEmployeeCommand>
{
    public CreateEmployeeValidator()
    {
        RuleFor(x => x.Dto.FullName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Dto.DepartmentId).GreaterThan(0);
        RuleFor(x => x.Dto.PositionId).GreaterThan(0);
        RuleFor(x => x.Dto.HireDate).NotEmpty();
    }
}
public class UpdateEmployeeValidator : AbstractValidator<UpdateEmployeeCommand>
{
    public UpdateEmployeeValidator()
    {
        RuleFor(x => x.Dto.EmployeeId).GreaterThan(0);
        RuleFor(x => x.Dto.FullName).MaximumLength(100).When(x => x.Dto.FullName != null);
        RuleFor(x => x.Dto.DepartmentId).GreaterThan(0).When(x => x.Dto.DepartmentId.HasValue);
        RuleFor(x => x.Dto.PositionId).GreaterThan(0).When(x => x.Dto.PositionId.HasValue);
    }
}