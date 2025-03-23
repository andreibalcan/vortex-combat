using System.Text.Json.Serialization;

namespace server.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public string Belt { get; set; }
        public double Height { get; set; }
        public double Weight { get; set; }
        public DateTime EnrollDate { get; set; }
        
        [JsonIgnore]
        public List<WorkoutStudent> WorkoutStudents { get; set; } = new();
    }
}