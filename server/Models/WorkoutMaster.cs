namespace server.Models
{
    public class WorkoutMaster
    {
        public int WorkoutId { get; set; }
        public Workout Workout { get; set; }

        public int MasterId { get; set; }
        public Master Master { get; set; }
    }
}