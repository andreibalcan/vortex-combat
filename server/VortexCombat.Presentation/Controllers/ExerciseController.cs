using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VortexCombat.Domain.Entities;
using VortexCombat.Domain.Interfaces;

namespace VortexCombat.Presentation.Controllers
{
    [ApiController]
    [Route("nomis/exercise")]
    public class ExerciseController : ControllerBase
    {
        private readonly IExerciseRepository _exerciseRepository;

        public ExerciseController(IExerciseRepository exerciseRepository)
        {
            _exerciseRepository = exerciseRepository;
        }

        [HttpPost]
        [Authorize(Roles = "PrimaryMaster")]
        public async Task<IActionResult> CreateExercises([FromBody] List<Exercise> exercises)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _exerciseRepository.AddRangeAsync(exercises);
            await _exerciseRepository.SaveChangesAsync();
            return Ok(new { count = exercises.Count });
        }

        [HttpGet]
        [Authorize(Roles = "PrimaryMaster,Student")]
        public async Task<ActionResult<IEnumerable<Exercise>>> GetExercises([FromQuery] string? id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                var allExercises = await _exerciseRepository.GetAllAsync();
                return Ok(allExercises);
            }

            var parsedIds = id.Split(',')
                .Select(idStr => int.TryParse(idStr, out var id) ? id : (int?)null)
                .Where(id => id.HasValue)
                .Select(id => id!.Value)
                .ToList();

            if (!parsedIds.Any())
                return BadRequest("Invalid ids format. Use: ?id=1,2,3");

            if (parsedIds.Count == 1)
            {
                var exercise = await _exerciseRepository.GetByIdAsync(parsedIds[0]);
                if (exercise == null) return NotFound();
                return Ok(new List<Exercise> { exercise });
            }
            else
            {
                var exercises = await _exerciseRepository.GetByIdsAsync(parsedIds);
                return Ok(exercises);
            }
        }
    }
}