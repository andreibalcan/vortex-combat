using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VortexCombat.Infrastructure.Identity;

public class ApplicationUser : IdentityUser
{
    // Ponte para o agregado de domínio User
    public Guid? DomainUserId { get; set; }
}