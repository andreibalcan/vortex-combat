namespace server.Models
{
    public class WorkoutStudent
    {
        public int WorkoutId { get; set; }
        public Workout Workout { get; set; }

        public int StudentId { get; set; }
        public Student Student { get; set; }
    }
}