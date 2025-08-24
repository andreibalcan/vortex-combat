using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VortexCombat.Domain.Entities;
using VortexCombat.Domain.Interfaces;

namespace VortexCombat.Presentation.Controllers
{
    [ApiController]
    [Route("nomis/exercise")]
    [Authorize(Roles = "PrimaryMaster")]
    public class ExerciseController : ControllerBase
    {
        private readonly IExerciseRepository _exerciseRepository;

        public ExerciseController(IExerciseRepository exerciseRepository)
        {
            _exerciseRepository = exerciseRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateExercises([FromBody] List<Exercise> exercises)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _exerciseRepository.AddRangeAsync(exercises);
            await _exerciseRepository.SaveChangesAsync();
            return Ok(new { count = exercises.Count });
        }

        [HttpGet]
        [Authorize(Roles = "PrimaryMaster,Student")]
        public async Task<ActionResult<IEnumerable<Exercise>>> GetExercises()
        {
            var exercises = await _exerciseRepository.GetAllAsync();
            return Ok(exercises);
        }
    }
}