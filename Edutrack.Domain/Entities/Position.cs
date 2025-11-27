namespace EduTrack.Domain.Entities {
    public class Position
    {
        public int PositionId { get; set; }
        public string PositionName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string? ModifiedBy { get; set; }

        // Navegación opcional: empleados que tienen esta posición
        public ICollection<Employees> Employees { get; set; } = new List<Employees>();
    }
}