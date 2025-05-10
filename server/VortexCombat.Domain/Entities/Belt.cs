using System.Text.Json.Serialization;

namespace VortexCombat.Domain.Entities
{
    public class Belt
    {
        public EBeltColor Color { get; set; }

        public int Degrees { get; set; }
    }
}