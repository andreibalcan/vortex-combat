using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VortexCombat.Domain.Common;
using VortexCombat.Domain.Entities;

namespace VortexCombat.Domain.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetByIdAsync(UserId id);
        //Task<User?> GetByEmailAsync(string email);
        //Task AddAsync(User user);
        //Task<bool> ExistsByEmailAsync(string email);
    }
}