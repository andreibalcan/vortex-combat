using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VortexCombat.Application.DTOs;
using VortexCombat.Domain.Entities;
using VortexCombat.Domain.Interfaces;
using VortexCombat.Shared.Enums;

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
            return Ok(students);
        }

        [HttpGet("nomis/students/progress/{studentId}")]
        [Authorize(Roles = "PrimaryMaster,Student")]
        public async Task<IActionResult> GetStudentProgress(int studentId)
        {
            var student = await _studentRepository.GetByIdWithUserAsync(studentId);
            if (student is null) return NotFound("Student not found");

            var user = student.ApplicationUser;
            if (user?.Belt is null) return BadRequest("Student does not have a belt assigned");

            var currentBelt = user.Belt;

            // Required and completed exercises for current belt
            var requiredExercises = await _exerciseRepository.GetByBeltAsync(currentBelt);
            var completedExercises = await _studentRepository.GetCompletedExercisesForBeltAsync(student.Id, currentBelt);

            var completedCount = completedExercises.Count;
            var totalRequired = requiredExercises.Count;
            var progress = totalRequired > 0 ? Math.Min(1.0, (double)completedCount / totalRequired) * 100 : 0;

            var attendedWorkouts = await _studentRepository.GetAttendedWorkoutsAsync(student.Id);

            var completedIds = completedExercises.Select(e => e.Id).ToHashSet();
            var remainingExercises = requiredExercises
                .Where(e => !completedIds.Contains(e.Id))
                .Select(e => new SimplifiedExerciseDTO { Id = e.Id, Name = e.Name })
                .ToList();

            var progressDto = new StudentProgressDTO
            {
                Id = student.Id,
                ApplicationUserId = user.Id,
                Name = user.Name,
                EGender = user.EGender,
                Birthday = user.Birthday,
                EnrollDate = student.EnrollDate,
                Height = user.Height,
                Weight = user.Weight,
                CurrentBelt = currentBelt,
                NextBelt = CalculateNextBelt(currentBelt),
                ProgressPercentage = progress,
                CompletedExercises = completedExercises
                    .Select(e => new SimplifiedExerciseDTO { Id = e.Id, Name = e.Name })
                    .ToList(),
                AttendedWorkouts = attendedWorkouts
                    .Select(w => new WorkoutDTO { Id = w.Id, StartDate = w.StartDate, EndDate = w.EndDate })
                    .ToList(),
                RemainingExercises = remainingExercises
            };

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
