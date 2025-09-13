using VortexCombat.Domain.Common;

namespace VortexCombat.Domain.Entities
{
    public class Master
    {
        public int Id { get; set; }
        public UserId UserId { get; set; }
        public User? User { get; set; }
        public bool HasTrainerCertificate { get; set; }
        public List<WorkoutMaster> WorkoutMasters { get; set; } = new();
    }
}