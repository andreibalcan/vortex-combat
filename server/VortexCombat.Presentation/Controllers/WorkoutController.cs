using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VortexCombat.Application.DTOs;
using VortexCombat.Infrastructure.Data;
using VortexCombat.Domain.Entities;

namespace server.Controllers
{
    [ApiController]
    public class WorkoutsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public WorkoutsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("api/workouts")]
        [Authorize(Roles = "PrimaryMaster,Student")]
        public async Task<ActionResult<IEnumerable<object>>> GetWorkouts()
        {
            var workouts = await _context.Workouts
                .Include(w => w.WorkoutStudents)
                .ThenInclude(ws => ws.Student)
                .ThenInclude(s => s.ApplicationUser)
                .Include(w => w.WorkoutMasters)
                .ThenInclude(wm => wm.Master)
                .ThenInclude(m => m.ApplicationUser)
                .Select(w => new
                {
                    w.Id,
                    w.Description,
                    w.StartDate,
                    w.EndDate,
                    w.Room,
                    Students = w.WorkoutStudents.Select(ws => new
                    {
                        Id = ws.Student.Id,
                        Name = ws.Student.ApplicationUser.Name
                    }).ToList(),
                    Masters = w.WorkoutMasters.Select(wm => new
                    {
                        Id = wm.Master.Id,
                        Name = wm.Master.ApplicationUser.Name
                    }).ToList()
                })
                .ToListAsync();

            return Ok(workouts);
        }

        [HttpPost]
        [Route("nomis/workouts/schedule-workout")]
        [Authorize(Roles = "PrimaryMaster")]
        public async Task<ActionResult<ScheduleWorkoutDTO>> CreateWorkout([FromBody] ScheduleWorkoutDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var workout = new Workout
            {
                Description = dto.Description,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Room = dto.Room
            };

            _context.Workouts.Add(workout);
            await _context.SaveChangesAsync();

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
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var workout = await _context.Workouts.FindAsync(id);
            if (workout == null)
                return NotFound("Workout not found");

            workout.Description = dto.Description;
            workout.Room = dto.Room;
            workout.StartDate = dto.StartDate;
            workout.EndDate = dto.EndDate;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                workout.Id,
                workout.Description,
                workout.StartDate,
                workout.EndDate,
                workout.Room
            });
        }

        /// <summary>
        /// Submits student attendance for a workout.
        /// </summary>
        /// <param name="request">The attendance request containing workoutId, studentIds and masterIds</param>
        /// <returns>Success message or error</returns>
        [HttpPost]
        [Route("/nomis/workouts/register-attendance")]
        [Authorize(Roles = "PrimaryMaster")]
        public async Task<IActionResult> SubmitAttendance([FromBody] SubmitAttendanceRequest request)
        {
            var workout = await _context.Workouts.FindAsync(request.WorkoutId);

            if (workout == null)
                return NotFound("Workout not found");

            var students = await _context.Students.ToListAsync();
            var selectedStudents = students.Where(s => request.StudentIds.Contains(s.Id)).ToList();

            if (selectedStudents.Count != request.StudentIds.Count)
                return BadRequest("Some students not found");

            var masters = await _context.Masters.ToListAsync();
            var selectedMasters = masters.Where(m => request.MasterIds.Contains(m.Id)).ToList();

            if (selectedMasters.Count != request.MasterIds.Count)
                return BadRequest("Some masters not found");

            foreach (var student in selectedStudents)
            {
                bool alreadyExists = await _context.WorkoutStudents
                    .AnyAsync(ws => ws.WorkoutId == workout.Id && ws.StudentId == student.Id);

                if (!alreadyExists)
                {
                    _context.WorkoutStudents.Add(new WorkoutStudent
                    {
                        WorkoutId = workout.Id,
                        StudentId = student.Id
                    });
                }
            }

            foreach (var master in selectedMasters)
            {
                bool alreadyExists = await _context.WorkoutMasters
                    .AnyAsync(wm => wm.WorkoutId == workout.Id && wm.MasterId == master.Id);

                if (!alreadyExists)
                {
                    _context.WorkoutMasters.Add(new WorkoutMaster
                    {
                        WorkoutId = workout.Id,
                        MasterId = master.Id
                    });
                }
            }

            await _context.SaveChangesAsync();
            return Ok("Attendance submitted successfully");
        }

        [HttpPost]
        [Route("/nomis/workouts/enroll-workout")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> EnrollInWorkout([FromBody] EnrollInWorkoutRequest request)
        {
            var studentId = await GetAuthenticatedStudentIdAsync();

            var workout = await _context.Workouts.FindAsync(request.WorkoutId);
            if (workout == null)
                return NotFound("Workout not found");

            var alreadyEnrolled = await _context.WorkoutStudents
                .AnyAsync(ws => ws.WorkoutId == workout.Id && ws.StudentId == studentId);
            if (alreadyEnrolled)
                return BadRequest("You are already enrolled in this workout.");

            var hasTimeConflict = await _context.WorkoutStudents
                .Where(ws => ws.StudentId == studentId)
                .AnyAsync(ws =>
                    _context.Workouts.Any(w =>
                            w.Id == ws.WorkoutId &&
                            w.Id != workout.Id &&
                            w.StartDate < workout.EndDate &&
                            workout.StartDate < w.EndDate
                    )
                );

            if (hasTimeConflict)
                return BadRequest("You are already enrolled in another workout during this time.");

            _context.WorkoutStudents.Add(new WorkoutStudent
            {
                WorkoutId = workout.Id,
                StudentId = studentId
            });

            await _context.SaveChangesAsync();

            var student = await _context.Students
                .Include(s => s.ApplicationUser)
                .FirstOrDefaultAsync(s => s.Id == studentId);

            return Ok(new
            {
                id = student.Id,
                name = student.ApplicationUser.Name
            });
        }

        [HttpDelete]
        [Route("nomis/workouts/delete-workout/{id}")]
        [Authorize(Roles = "PrimaryMaster")]
        public async Task<IActionResult> DeleteWorkout(int id)
        {
            var workout = await _context.Workouts.FindAsync(id);

            if (workout == null)
                return NotFound("Workout not found");

            _context.Workouts.Remove(workout);
            await _context.SaveChangesAsync();

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

            var student = await _context.Students
                .FirstOrDefaultAsync(s => s.ApplicationUserId == userId);

            if (student == null)
                throw new UnauthorizedAccessException("Student not found");

            return student.Id;
        }
    }
}