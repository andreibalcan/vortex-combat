namespace VortexCombat.Application.Actions.Nomis
{
    public sealed class RegisterAttendanceRequest
    {
        public int WorkoutId { get; set; }
        public List<int> StudentIds { get; set; } = new();
        public List<int> MasterIds { get; set; } = new();
    }

    public sealed class EnrollWorkoutRequest
    {
        public int WorkoutId { get; set; }
        public int StudentId { get; set; } // resolved from token by controller
    }

    public sealed class UpdateWorkoutRequest
    {
        public int Id { get; set; }
        public string Description { get; set; } = "";
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Room { get; set; } = "";
        public List<int> Exercises { get; set; } = new();
    }

    public sealed class ScheduleWorkoutRequest
    {
        public string Description { get; set; } = "";
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Room { get; set; } = "";
        public List<int> Exercises { get; set; } = new();
    }
}