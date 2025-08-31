using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VortexCombat.Application.Mappings;
using VortexCombat.Application.Actions.Nomis;
using VortexCombat.Application.DTOs.Workout;
using VortexCombat.Domain.Entities;
using VortexCombat.Domain.Interfaces;

namespace VortexCombat.Presentation.Controllers
{
    [ApiController]
    public class WorkoutsController : ControllerBase
    {
        private readonly IWorkoutRepository _workoutRepo;
        private readonly IStudentRepository _studentRepo;

        private readonly INomisAction<ScheduleWorkoutRequest, Workout> _scheduleWorkout;
        private readonly INomisAction<UpdateWorkoutRequest, Workout?> _updateWorkout;
        private readonly INomisAction<RegisterAttendanceRequest, bool> _registerAttendance;
        private readonly INomisAction<EnrollWorkoutRequest, (int studentId, string studentName)> _enrollWorkout;
        private readonly INomisAction<int, bool> _deleteWorkout;

        public WorkoutsController(
            IWorkoutRepository workoutRepo,
            IStudentRepository studentRepo,
            INomisAction<ScheduleWorkoutRequest, Workout> scheduleWorkout,
            INomisAction<UpdateWorkoutRequest, Workout?> updateWorkout,
            INomisAction<RegisterAttendanceRequest, bool> registerAttendance,
            INomisAction<EnrollWorkoutRequest, (int studentId, string studentName)> enrollWorkout,
            INomisAction<int, bool> deleteWorkout)
        {
            _workoutRepo = workoutRepo;
            _studentRepo = studentRepo;
            _scheduleWorkout = scheduleWorkout;
            _updateWorkout = updateWorkout;
            _registerAttendance = registerAttendance;
            _enrollWorkout = enrollWorkout;
            _deleteWorkout = deleteWorkout;
        }

        [HttpGet]
        [Route("api/workouts")]
        [Authorize(Roles = "PrimaryMaster,Student")]
        public async Task<ActionResult<IEnumerable<WorkoutDto>>> GetWorkouts()
        {
            var workouts = await _workoutRepo.GetAllWithDetailsAsync();
            return Ok(workouts.Select(w => w.ToWorkoutDto()));
        }

        [HttpPost]
        [Route("nomis/workouts/schedule-workout")]
        [Authorize(Roles = "PrimaryMaster")]
        public async Task<ActionResult<WorkoutDto>> CreateWorkout([FromBody] ScheduleWorkoutDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var req = new ScheduleWorkoutRequest
            {
                Description = dto.Description,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Room = dto.Room,
                Exercises = dto.Exercises
            };

            var (ok, error) = await _scheduleWorkout.CanExecuteAsync(req);
            if (!ok) return BadRequest(error);

            var w = await _scheduleWorkout.ExecuteAsync(req);
            var detailedWorkout = await _workoutRepo.GetByIdWithDetailsAsync(w.Id);
            return CreatedAtAction(nameof(GetWorkouts), new { id = w.Id }, detailedWorkout!.ToWorkoutDto());
        }

        [HttpPut]
        [Route("nomis/workouts/update-workout/{id}")]
        [Authorize(Roles = "PrimaryMaster")]
        public async Task<IActionResult> UpdateWorkout(int id, [FromBody] ScheduleWorkoutDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var req = new UpdateWorkoutRequest
            {
                Id = id,
                Description = dto.Description,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Room = dto.Room,
                Exercises = dto.Exercises
            };

            var (ok, error) = await _updateWorkout.CanExecuteAsync(req);
            if (!ok) return NotFound(error);

            await _updateWorkout.ExecuteAsync(req);

            var updatedWorkout = await _workoutRepo.GetByIdWithDetailsAsync(id);
            return Ok(updatedWorkout?.ToWorkoutDto());
        }

        [HttpPost]
        [Route("/nomis/workouts/register-attendance")]
        [Authorize(Roles = "PrimaryMaster")]
        public async Task<IActionResult> SubmitAttendance([FromBody] RegisterAttendanceRequest request)
        {
            var (ok, error) = await _registerAttendance.CanExecuteAsync(request);
            if (!ok) return BadRequest(error);

            await _registerAttendance.ExecuteAsync(request);
            return Ok(new { message = "Attendance submitted successfully" });
        }

        [HttpPost]
        [Route("/nomis/workouts/enroll-workout")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> EnrollInWorkout([FromBody] EnrollWorkoutRequest request)
        {
            request.StudentId = await GetAuthenticatedStudentIdAsync();
            var (ok, error) = await _enrollWorkout.CanExecuteAsync(request);
            if (!ok) return BadRequest(error);

            var (id, name) = await _enrollWorkout.ExecuteAsync(request);
            return Ok(new { id, name });
        }

        [HttpDelete]
        [Route("nomis/workouts/delete-workout/{id}")]
        [Authorize(Roles = "PrimaryMaster")]
        public async Task<IActionResult> DeleteWorkout(int id)
        {
            var (ok, error) = await _deleteWorkout.CanExecuteAsync(id);
            if (!ok) return NotFound(error);

            await _deleteWorkout.ExecuteAsync(id);
            return Ok(new { message = "Workout deleted successfully" });
        }

        private async Task<int> GetAuthenticatedStudentIdAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                         ?? throw new UnauthorizedAccessException("User ID not found in token");
            var student = await _studentRepo.GetByApplicationUserIdAsync(userId)
                          ?? throw new UnauthorizedAccessException("Student not found");
            return student.Id;
        }
    }
}