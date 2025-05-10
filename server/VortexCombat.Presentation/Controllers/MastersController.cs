using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VortexCombat.Domain.Entities;
using VortexCombat.Infrastructure.Data;

namespace server.Controllers
{
    [Route("api/masters")]
    [ApiController]
    [Authorize(Roles = "PrimaryMaster")]
    public class MastersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MastersController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Master>>> GetMasters()
        {
            var masters = await _context.Masters.ToListAsync();
            return Ok(masters);
        }
    }
}