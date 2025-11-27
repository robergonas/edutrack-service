using MediatR;

namespace EduTrack.Application.Authentication.Commands {
    public class CreatePositionCommand : IRequest<PositionResponseDto>
    {
        public CreatePositionDto Dto { get; set; } = new();
        public string CurrentUserName { get; set; } = "system";
    }
    public class UpdatePositionCommand : IRequest<PositionResponseDto>
    {
        public UpdatePositionDto Dto { get; set; } = new();
        public string CurrentUserName { get; set; } = "system";
    }
    public class DeletePositionCommand : IRequest<bool>
    {
        public int PositionId { get; set; }
        public string CurrentUserName { get; set; } = "system";
    }
    public class GetPositionByIdQuery : IRequest<PositionResponseDto?>
    {
        public int PositionId { get; set; }
    }
    public class ListPositionsQuery : IRequest<IReadOnlyList<PositionResponseDto>> { }
    public class ListPositionSelectQuery : IRequest<IReadOnlyList<PositionSelectDto>> { }

}