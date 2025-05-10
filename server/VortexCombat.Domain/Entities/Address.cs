using System.Text.Json.Serialization;

namespace VortexCombat.Domain.Entities
{
    public class Address
    {
        public string? Street { get; set; }

        public string? Number { get; set; }

        public string? Floor { get; set; }

        public string? City { get; set; }

        public string? ZipCode { get; set; }
    }
}