using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VortexCombat.Domain.Entities;
using VortexCombat.Infrastructure.Data;

namespace server.Controllers
{
    [ApiController]
    [Route("nomis/exercise")]
    [Authorize(Roles = "PrimaryMaster")]
    public class ExerciseController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ExerciseController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateExercises([FromBody] List<Exercise> exercises)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Exercises.AddRange(exercises);
            await _context.SaveChangesAsync();

            return Ok(new { count = exercises.Count });
        }

        [HttpGet]
        [Authorize(Roles = "PrimaryMaster,Student")]
        public async Task<ActionResult<IEnumerable<Exercise>>> GetExercises()
        {
            var exercises = await _context.Exercises.ToListAsync();
            return Ok(exercises);
        }
    }
}