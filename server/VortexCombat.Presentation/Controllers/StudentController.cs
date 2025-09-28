using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VortexCombat.Application.Mappings;
using VortexCombat.Domain.Common;
using VortexCombat.Domain.Entities;
using VortexCombat.Domain.Interfaces;

namespace VortexCombat.Presentation.Controllers
{
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
        [HttpGet("api/students")]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
        {
            var students = await _studentRepository.GetAllWithUserAsync();
            var formattedDto = students.Select(s => s.ToExtendedStudentDto()).ToList();

            return Ok(formattedDto);
        }

        [HttpGet("nomis/students/progress/{studentId?}")]
        [Authorize(Roles = "PrimaryMaster,Student")]
        public async Task<IActionResult> GetStudentProgress(int? studentId = null)
        {
            int resolvedStudentId;

            // Check if user has Student role and studentId is not provided
            if (User.IsInRole("Student") && !studentId.HasValue)
            {
                // Extract domain_user_id from JWT token
                var domainUserIdClaim = User.FindFirst("domain_user_id");
                if (domainUserIdClaim == null || !Guid.TryParse(domainUserIdClaim.Value, out var domainUserGuid))
                {
                    return Unauthorized("Invalid user token");
                }

                var domainUserId = new UserId(domainUserGuid);

                // Get student by domain user ID
                var studentByUserId = await _studentRepository.GetByUserIdAsync(domainUserId);
                if (studentByUserId == null)
                {
                    return NotFound("Student record not found for current user");
                }

                resolvedStudentId = studentByUserId.Id;
            }
            else if (studentId.HasValue)
            {
                // Use provided studentId (by PrimaryMaster role)
                resolvedStudentId = studentId.Value;

                // Authorization check to ensure Students can only access their own data
                if (User.IsInRole("Student"))
                {
                    var domainUserIdClaim = User.FindFirst("domain_user_id");
                    if (domainUserIdClaim != null && Guid.TryParse(domainUserIdClaim.Value, out var domainUserGuid))
                    {
                        var domainUserId = new UserId(domainUserGuid);
                        var studentByUserId = await _studentRepository.GetByUserIdAsync(domainUserId);

                        if (studentByUserId?.Id != studentId.Value)
                        {
                            return Forbid("Students can only access their own progress");
                        }
                    }
                }
            }
            else
            {
                return BadRequest("Student ID is required for non-student users");
            }

            var student = await _studentRepository.GetByIdWithUserAsync(resolvedStudentId);
            if (student is null) return NotFound("Student not found");

            var user = student.User;
            if (user?.Belt is null) return BadRequest("Student does not have a belt assigned");

            var currentBelt = user.Belt;

            // Calculate age for junior belt rules
            var age = (DateTime.Today - user.Birthday).TotalDays / 365.25;
            bool isAdult = age >= 16;

            // Determine allowed belt colors based on age
            var allowedColors = isAdult
                ? Enum.GetValues<EBeltColor>().ToList()
                : new List<EBeltColor>
                    { EBeltColor.White, EBeltColor.Grey, EBeltColor.Yellow, EBeltColor.Orange, EBeltColor.Green };

            // Filter current belt to allowed colors
            if (!allowedColors.Contains(currentBelt.Color))
            {
                currentBelt.Color = EBeltColor.White;
                currentBelt.Degrees = 0;
            }

            // Get all required exercises for the student for current belt within allowed degrees/colors
            var requiredExercises = (await _exerciseRepository.GetByBeltAsync(currentBelt))
                .Where(e => allowedColors.Contains(e.Grade.Color))
                .ToList();

            // Get all completed exercises for the student for this belt
            var completedExercises =
                (await _studentRepository.GetCompletedExercisesForBeltAsync(student.Id, currentBelt))
                .Where(e => allowedColors.Contains(e.Grade.Color))
                .ToList();

            // Remaining exercises
            var remainingExercises = requiredExercises.Except(completedExercises).ToList();

            var attendedWorkouts = await _studentRepository.GetAttendedWorkoutsAsync(student.Id);

            var nextBelt = CalculateNextBelt(currentBelt, isAdult);

            var progressDto = student.ToProgressDto(
                nextBelt,
                completedExercises,
                remainingExercises,
                attendedWorkouts
            );

            return Ok(progressDto);
        }

        private static Belt CalculateNextBelt(Belt currentBelt, bool isAdult)
        {
            const int maxDegrees = 4;
            var nextDegrees = currentBelt.Degrees + 1;
            var nextColor = currentBelt.Color;

            if (nextDegrees > maxDegrees)
            {
                nextDegrees = 0;
                nextColor = (EBeltColor)((int)currentBelt.Color + 1);

                // Junior belt restriction: cannot skip to adult belts
                if (!isAdult && nextColor > EBeltColor.Green)
                {
                    nextColor = EBeltColor.Green;
                }
            }

            return new Belt { Color = nextColor, Degrees = nextDegrees };
        }
    }
}