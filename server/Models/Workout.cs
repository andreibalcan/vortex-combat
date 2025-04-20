using System.ComponentModel.DataAnnotations.Schema;

namespace server.Models
{
    public class Workout
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public int Duration { get; set; }
        public string Room { get; set; }
        public List<WorkoutMaster> WorkoutMasters { get; set; } = new();
        public List<WorkoutStudent> WorkoutStudents { get; set; } = new();
    }
}