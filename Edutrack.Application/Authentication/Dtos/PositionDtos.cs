public class CreatePositionDto
{
    public string PositionName { get; set; } = string.Empty;
    public string? Description { get; set; }
}
public class UpdatePositionDto
{
    public int PositionId { get; set; }
    public string? PositionName { get; set; }
    public string? Description { get; set; }
}
public class PositionResponseDto
{
    public int PositionId { get; set; }
    public string PositionName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
}
public class PositionSelectDto
{
    public int PositionId { get; set; }
    public string PositionName { get; set; } = string.Empty;
}