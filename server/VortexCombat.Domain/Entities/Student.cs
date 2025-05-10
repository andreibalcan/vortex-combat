using System.Text.Json.Serialization;

namespace VortexCombat.Domain.Entities
{
    public class Student
    {
        public int Id { get; set; }

        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public DateTime EnrollDate { get; set; }

        [JsonIgnore] public List<WorkoutStudent> WorkoutStudents { get; set; } = new();
    }
}