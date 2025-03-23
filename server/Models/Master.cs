using System.Text.Json.Serialization;

namespace server.Models
{
    public class Master
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public string Belt { get; set; }
        
        [JsonIgnore]
        public List<WorkoutMaster> WorkoutMasters { get; set; } = new();
    }
}