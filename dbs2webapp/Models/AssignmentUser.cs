namespace dbs2webapp.Models
{
    public class AssignmentUser
    {
        public int Id { get; set; }

        public int AssignmentId { get; set; }

        public int UserId { get; set; }

        public virtual Assignment Assignment { get; set; } = null!;

        public virtual User User { get; set; } = null!;
    }
}
