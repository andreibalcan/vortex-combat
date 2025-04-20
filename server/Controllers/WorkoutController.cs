using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Models;

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
        [Authorize(Roles = "PrimaryMaster")]
        public async Task<ActionResult<IEnumerable<Student>>> GetWorkouts()
        {
            var workouts = await _context.Workouts
                .Include(w => w.WorkoutMasters)
                .ThenInclude(wm => wm.Master) 
                .ThenInclude(m => m.ApplicationUser)
                .Include(w => w.WorkoutStudents)
                .ThenInclude(ws => ws.Student)
                .ThenInclude(s => s.ApplicationUser)
                .ToListAsync();

            var workoutDetails = workouts.Select(w => new
            {
                w.Id,
                w.Description,
                w.Date,
                w.Duration,
                w.Room,
                Masters = w.WorkoutMasters.Select(wm => new
                {
                    wm.Master.Id,
                    wm.Master.ApplicationUser.Name // Access Name from ApplicationUser
                }).ToList(),
                Students = w.WorkoutStudents.Select(ws => new
                {
                    ws.Student.Id,
                    ws.Student.ApplicationUser.Name // Access Name from ApplicationUser
                }).ToList()
            }).ToList();

            return Ok(workoutDetails);
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

        public class SubmitAttendanceRequest
        {
            public int WorkoutId { get; set; }
            public List<int> StudentIds { get; set; } = new();
            public List<int> MasterIds { get; set; } = new();
        }
    }
}