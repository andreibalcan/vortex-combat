using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VortexCombat.Application.Mappings;
using VortexCombat.Domain.Entities;
using VortexCombat.Domain.Interfaces;

namespace VortexCombat.Presentation.Controllers
{
    [Route("api/students")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IExerciseRepository _exerciseRepository;

        public StudentController(IStudentRepository studentRepository, IExerciseRepository exerciseRepository)
        {
            _studentRepository = studentRepository;
            _exerciseRepository = exerciseRepository;
        }

        [Authorize(Roles = "PrimaryMaster")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
        {
            var students = await _studentRepository.GetAllWithUserAsync();
            var formattedDto = students.Select(s => s.ToExtendedStudentDto()).ToList();

            return Ok(formattedDto);
        }

        [HttpGet("nomis/students/progress/{studentId}")]
        [Authorize(Roles = "PrimaryMaster,Student")]
        public async Task<IActionResult> GetStudentProgress(int studentId)
        {
            var student = await _studentRepository.GetByIdWithUserAsync(studentId);
            if (student is null) return NotFound("Student not found");

            var user = student.User;
            if (user?.Belt is null) return BadRequest("Student does not have a belt assigned");

            var currentBelt = user.Belt;

            var requiredExercises = await _exerciseRepository.GetByBeltAsync(currentBelt);
            var completedExercises = await _studentRepository.GetCompletedExercisesForBeltAsync(student.Id, currentBelt);
            var attendedWorkouts = await _studentRepository.GetAttendedWorkoutsAsync(student.Id);

            var nextBelt = CalculateNextBelt(currentBelt);

            var progressDto = student.ToProgressDto(nextBelt, completedExercises, 
                requiredExercises.Except(completedExercises).ToList(), attendedWorkouts);

            return Ok(progressDto);
        }

        private static Belt CalculateNextBelt(Belt currentBelt)
        {
            const int maxDegrees = 4;
            var nextDegrees = currentBelt.Degrees + 1;
            var nextColor = currentBelt.Color;

            if (nextDegrees > maxDegrees)
            {
                nextDegrees = 0;
                nextColor = (EBeltColor)(((int)currentBelt.Color) + 1);
            }

            return new Belt { Color = nextColor, Degrees = nextDegrees };
        }
    }
}
