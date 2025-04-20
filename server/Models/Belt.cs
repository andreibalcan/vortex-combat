using System.Text.Json.Serialization;

namespace server.Models
{
    public class Belt
    {
        public BeltColor Color { get; set; }

        public int Degrees { get; set; }
    }
}