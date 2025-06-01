using System.ComponentModel.DataAnnotations.Schema;

namespace VortexCombat.Domain.Entities
{
    public class Workout
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Room { get; set; }
        public List<WorkoutMaster> WorkoutMasters { get; set; } = new();
        public List<WorkoutStudent> WorkoutStudents { get; set; } = new();
    }
}