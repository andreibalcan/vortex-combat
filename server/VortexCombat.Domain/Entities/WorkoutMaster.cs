using VortexCombat.Shared.Enums;

namespace VortexCombat.Domain.Entities
{
    public class WorkoutMaster
    {
        public int WorkoutId { get; set; }
        public Workout Workout { get; set; }

        public int MasterId { get; set; }
        public Master Master { get; set; }
        
        public EAttendanceStatus Status { get; set; } = EAttendanceStatus.None;
    }
}