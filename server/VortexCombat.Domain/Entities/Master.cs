using System.Text.Json.Serialization;

namespace VortexCombat.Domain.Entities
{
    public class Master
    {
        public int Id { get; set; }
        public bool HasTrainerCertificate { get; set; }

        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        [JsonIgnore] public List<WorkoutMaster> WorkoutMasters { get; set; } = new();
    }
}