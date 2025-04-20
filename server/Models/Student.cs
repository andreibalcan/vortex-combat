using System.Text.Json.Serialization;

namespace server.Models
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