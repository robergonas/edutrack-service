using EduTrack.Application.Authentication.Commands;
using FluentValidation;

public class CreatePositionValidator : AbstractValidator<CreatePositionCommand>
{
    public CreatePositionValidator()
    {
        RuleFor(x => x.Dto.PositionName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Dto.Description).MaximumLength(255);
    }
}
public class UpdatePositionValidator : AbstractValidator<UpdatePositionCommand>
{
    public UpdatePositionValidator()
    {
        RuleFor(x => x.Dto.PositionId).GreaterThan(0);
        RuleFor(x => x.Dto.PositionName).MaximumLength(50).When(x => x.Dto.PositionName != null);
        RuleFor(x => x.Dto.Description).MaximumLength(255).When(x => x.Dto.Description != null);
    }
}