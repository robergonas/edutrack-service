namespace EduTrack.Domain.Entities
{
    public class Permissions
    {
        public int Permissionid { get; set; }
        public string Module { get; set; }
        public string Action { get; set; }
        public string Description { get; set; }
    }
}
