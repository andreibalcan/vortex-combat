using VortexCombat.Shared.Enums;

namespace VortexCombat.Domain.Entities
{
    public class WorkoutStudent
    {
        public int WorkoutId { get; set; }
        public Workout Workout { get; set; }

        public int StudentId { get; set; }
        public Student Student { get; set; }
        
        public EAttendanceStatus Status { get; set; } = EAttendanceStatus.None;
    }
}