using VortexCombat.Domain.Common;

namespace VortexCombat.Domain.Entities
{
    public class Student
    {
        public int Id { get; set; }
        public UserId UserId { get; set; }
        public User? User { get; set; }
        public DateTime EnrollDate { get; set; }
        public List<WorkoutStudent> WorkoutStudents { get; set; } = new();
    }
}