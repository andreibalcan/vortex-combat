using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VortexCombat.Domain.Common;

namespace VortexCombat.Domain.Entities
{
    public class User
    {
        public UserId Id { get; set; }
        public string Name { get; set; }

        public Address? Address { get; set; }

        public string Nif { get; set; }

        public EGender EGender { get; set; }

        public DateTime Birthday { get; set; }

        public Belt Belt { get; set; }

        public double Height { get; set; }

        public double Weight { get; set; }
    }
}