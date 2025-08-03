using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VortexCombat.Application.DTOs;
using VortexCombat.Infrastructure.Data;
using VortexCombat.Domain.Entities;
using VortexCombat.Shared.Enums;

namespace server.Controllers
{
    [Route("api/students")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public StudentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "PrimaryMaster")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
        {
            var students = await _context.Students
                .Include(s => s.ApplicationUser)
                .ToListAsync();
            return Ok(students);
        }

        [HttpGet("nomis/students/progress/{studentId}")]
        [Authorize(Roles = "PrimaryMaster,Student")]
        public async Task<IActionResult> GetStudentProgress(int studentId)
        {
            var student = await _context.Students
                .Include(s => s.ApplicationUser)
                .FirstOrDefaultAsync(s => s.Id == studentId);

            if (student is null)
                return NotFound("Student not found");

            var user = student.ApplicationUser;

            if (user.Belt is null)
                return BadRequest("Student does not have a belt assigned");

            var currentBelt = user.Belt;

            // Get all exercises required for this belt
            var requiredExercises = await _context.Exercises
                .Where(e => e.Grade.Color == currentBelt.Color && e.Grade.Degrees == currentBelt.Degrees)
                .ToListAsync();

            var requiredExerciseIds = requiredExercises.Select(e => e.Id).ToHashSet();

            // Get exercises completed by student via workouts
            var completedExercises = await _context.StudentWorkoutExercise
                .Where(swe => swe.StudentId == studentId && requiredExerciseIds.Contains(swe.ExerciseId))
                .Select(swe => swe.Exercise)
                .Distinct()
                .ToListAsync();

            // Get workouts the student attended
            var attendedWorkouts = await _context.WorkoutStudents
                .Where(ws => ws.StudentId == studentId && ws.Status == EAttendanceStatus.Attended)
                .Select(ws => ws.Workout)
                .ToListAsync();

            var completedCount = completedExercises.Count;
            var totalRequired = requiredExercises.Count;

            var progress = totalRequired > 0
                ? Math.Min(1.0, (double)completedCount / totalRequired) * 100
                : 0;

            var nextBelt = CalculateNextBelt(currentBelt);

            var completedExerciseIds = completedExercises.Select(e => e.Id).ToHashSet();

            var remainingExercises = requiredExercises
                .Where(e => !completedExerciseIds.Contains(e.Id))
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
                NextBelt = nextBelt,
                ProgressPercentage = progress,
                CompletedExercises = completedExercises
                    .Select(e => new SimplifiedExerciseDTO { Id = e.Id, Name = e.Name })
                    .ToList(),

                AttendedWorkouts = attendedWorkouts
                    .Select(w => new WorkoutDTO { Id = w.Id, StartDate = w.StartDate, EndDate = w.EndDate })
                    .ToList(),
                RemainingExercises = remainingExercises,
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

            return new Belt
            {
                Color = nextColor,
                Degrees = nextDegrees
            };
        }
    }
}