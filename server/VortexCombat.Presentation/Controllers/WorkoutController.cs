using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VortexCombat.Application.DTOs;
using VortexCombat.Domain.Entities;
using VortexCombat.Domain.Interfaces;
using VortexCombat.Shared.Enums;

namespace VortexCombat.Presentation.Controllers
{
    [ApiController]
    public class WorkoutController : ControllerBase
    {
        private readonly IWorkoutRepository _workoutRepository;
        private readonly IStudentRepository _studentRepository;

        public WorkoutController(IWorkoutRepository workoutRepository, IStudentRepository studentRepository)
        {
            _workoutRepository = workoutRepository;
            _studentRepository = studentRepository;
        }

        [HttpGet]
        [Route("api/workouts")]
        [Authorize(Roles = "PrimaryMaster,Student")]
        public async Task<ActionResult<IEnumerable<object>>> GetWorkouts()
        {
            var workouts = await _workoutRepository.GetAllWithDetailsAsync();
            var shaped = workouts.Select(w => new
            {
                w.Id,
                w.Description,
                w.StartDate,
                w.EndDate,
                w.Room,
                Students = w.WorkoutStudents.Select(ws => new { Id = ws.Student.Id, Name = ws.Student.ApplicationUser.Name }).ToList(),
                Masters = w.WorkoutMasters.Select(wm => new { Id = wm.Master.Id, Name = wm.Master.ApplicationUser.Name }).ToList(),
                Exercises = w.WorkoutExercises.Select(we => new { Id = we.Exercise.Id, Name = we.Exercise.Name }).ToList()
            })
            .ToList();

            return Ok(shaped);
        }

        [HttpPost]
        [Route("nomis/workouts/schedule-workout")]
        [Authorize(Roles = "PrimaryMaster")]
        public async Task<ActionResult<ScheduleWorkoutDTO>> CreateWorkout([FromBody] ScheduleWorkoutDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var workout = new Workout
            {
                Description = dto.Description,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Room = dto.Room
            };

            await _workoutRepository.AddAsync(workout);
            await _workoutRepository.SaveChangesAsync();

            foreach (var exerciseId in dto.Exercises)
            {
                // hydrate via navigation collection (tracked by context inside repo)
                workout.WorkoutExercises.Add(new WorkoutExercise
                {
                    WorkoutId = workout.Id,
                    ExerciseId = exerciseId
                });
            }

            await _workoutRepository.SaveChangesAsync();

            var responseDto = new WorkoutDTO
            {
                Id = workout.Id,
                Description = workout.Description,
                StartDate = workout.StartDate,
                EndDate = workout.EndDate,
                Room = workout.Room
            };

            return CreatedAtAction(nameof(GetWorkouts), new { id = workout.Id }, responseDto);
        }

        [HttpPut]
        [Route("nomis/workouts/update-workout/{id}")]
        [Authorize(Roles = "PrimaryMaster")]
        public async Task<IActionResult> UpdateWorkout(int id, [FromBody] ScheduleWorkoutDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var workout = await _workoutRepository.GetByIdAsync(id);
            if (workout == null) return NotFound("Workout not found");

            workout.Description = dto.Description;
            workout.Room = dto.Room;
            workout.StartDate = dto.StartDate;
            workout.EndDate = dto.EndDate;

            _workoutRepository.Update(workout);
            await _workoutRepository.SaveChangesAsync();

            return Ok(new
            {
                workout.Id,
                workout.Description,
                workout.StartDate,
                workout.EndDate,
                workout.Room
            });
        }

        [HttpPost]
        [Route("/nomis/workouts/register-attendance")]
        [Authorize(Roles = "PrimaryMaster")]
        public async Task<IActionResult> SubmitAttendance([FromBody] SubmitAttendanceRequest request)
        {
            var workout = await _workoutRepository.GetByIdAsync(request.WorkoutId);
            if (workout == null) return NotFound("Workout not found");

            await _workoutRepository.MarkAttendanceAsync(request.WorkoutId, request.StudentIds, request.MasterIds);
            return Ok(new { message = "Attendance submitted successfully" });
        }

        [HttpPost]
        [Route("/nomis/workouts/enroll-workout")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> EnrollInWorkout([FromBody] EnrollInWorkoutRequest request)
        {
            var studentId = await GetAuthenticatedStudentIdAsync();

            var workout = await _workoutRepository.GetByIdAsync(request.WorkoutId);
            if (workout == null) return NotFound("Workout not found");

            if (await _workoutRepository.IsStudentEnrolledAsync(workout.Id, studentId))
                return BadRequest("You are already enrolled in this workout.");

            if (await _workoutRepository.StudentHasTimeConflictAsync(studentId, workout.StartDate, workout.EndDate))
                return BadRequest("You are already enrolled in another workout during this time.");

            await _workoutRepository.EnrollStudentAsync(workout.Id, studentId, EAttendanceStatus.Enrolled);

            var student = await _studentRepository.GetByIdWithUserAsync(studentId);
            return Ok(new { id = student!.Id, name = student.ApplicationUser.Name });
        }

        [HttpDelete]
        [Route("nomis/workouts/delete-workout/{id}")]
        [Authorize(Roles = "PrimaryMaster")]
        public async Task<IActionResult> DeleteWorkout(int id)
        {
            var workout = await _workoutRepository.GetByIdAsync(id);
            if (workout == null) return NotFound("Workout not found");

            _workoutRepository.Remove(workout);
            await _workoutRepository.SaveChangesAsync();

            return Ok(new { message = "Workout deleted successfully" });
        }

        public class SubmitAttendanceRequest
        {
            public int WorkoutId { get; set; }
            public List<int> StudentIds { get; set; } = new();
            public List<int> MasterIds { get; set; } = new();
        }

        public class EnrollInWorkoutRequest
        {
            public int WorkoutId { get; set; }
        }

        private async Task<int> GetAuthenticatedStudentIdAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                throw new UnauthorizedAccessException("User ID not found in token");

            var student = await _studentRepository.GetByApplicationUserIdAsync(userId);
            if (student == null)
                throw new UnauthorizedAccessException("Student not found");

            return student.Id;
        }
    }
}
