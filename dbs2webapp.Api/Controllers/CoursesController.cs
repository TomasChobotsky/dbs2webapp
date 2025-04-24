using dbs2webapp.Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using dbs2webapp.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using dbs2webapp.Application.Interfaces;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseRepository _courseRepo;
        private readonly IBaseRepository<UserCourse> _userCourseRepo;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IMapper _mapper;

        public CoursesController(
            ICourseRepository courseRepo,
            IBaseRepository<UserCourse> userCourseRepo,
            UserManager<IdentityUser> userManager,
            IMapper mapper)
        {
            _courseRepo = courseRepo;
            _userCourseRepo = userCourseRepo;
            _userManager = userManager;
            _mapper = mapper;
        }

        // GET: api/courses
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<Course> courses;

            if (User.Identity?.IsAuthenticated == true
                && User.IsInRole("Teacher"))
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
                // your BaseRepository has FindAsync for predicate searches
                courses = await _courseRepo.FindAsync(c => c.TeacherId == userId);
            }
            else
            {
                courses = await _courseRepo.GetAllAsync();
            }

            var dto = _mapper.Map<List<CourseDto>>(courses);
            return Ok(dto);
        }

        // GET: api/courses/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var course = await _courseRepo.FindAsync(c => c.Id == id,
                include: q => q.Include(c => c.Chapters!).ThenInclude(ch => ch.Tests));

            var result = course.FirstOrDefault();
            return result == null ? NotFound() : Ok(_mapper.Map<CourseDto>(result));
        }

        // GET: api/courses/mine
        [Authorize(Roles = "Teacher,Admin")]
        [HttpGet("mine")]
        public async Task<IActionResult> GetMyCourses()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Unauthorized();

            var courses = await _courseRepo.FindAsync(c => c.TeacherId == userId,
                include: q => q.Include(c => c.Chapters!).ThenInclude(ch => ch.Tests));

            return Ok(_mapper.Map<List<CourseDto>>(courses));
        }

        // POST: api/courses
        [Authorize(Roles = "Teacher,Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCourseDto dto)
        {
            var course = _mapper.Map<Course>(dto);
            course.TeacherId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            course.CreatedDate = DateTime.UtcNow;

            await _courseRepo.SaveViaProcedureAsync(course, isDelete: false);

            return Ok();
        }

        // PUT: api/courses/{id}
        [Authorize(Roles = "Teacher,Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateCourseDto dto)
        {
            var existing = await _courseRepo.GetByIdAsync(id);
            if (existing == null)
                return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (existing.TeacherId != userId && !User.IsInRole("Admin"))
                return Forbid();

            _mapper.Map(dto, existing);
            await _courseRepo.SaveViaProcedureAsync(existing, isDelete: false);

            return NoContent();
        }

        // DELETE: api/courses/{id}
        [Authorize(Roles = "Teacher,Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var course = await _courseRepo.GetByIdAsync(id);
            if (course == null)
                return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (course.TeacherId != userId && !User.IsInRole("Admin"))
                return Forbid();

            await _courseRepo.SaveViaProcedureAsync(course, isDelete: true);

            return NoContent();
        }

        [HttpGet("enrollments")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> GetMyEnrollments()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            // use repository.FindAsync to pull all UserCourse for this user
            var userCourses = await _userCourseRepo
                .FindAsync(uc => uc.UserId == userId);

            // project down to just the CourseId list
            var courseIds = userCourses
                .Select(uc => uc.CourseId)
                .ToList();

            return Ok(courseIds);
        }

        // POST /api/courses/{courseId}/enroll
        [HttpPost("{courseId}/enroll")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> Enroll(int courseId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            // 1) ensure course exists
            var course = await _courseRepo.GetByIdAsync(courseId);
            if (course == null)
                return NotFound("Course not found");

            // 2) check for existing enrollment
            var existing = await _userCourseRepo
                .FindAsync(uc => uc.UserId == userId && uc.CourseId == courseId);

            if (existing.Any())
                return BadRequest("Already enrolled");

            // 3) create and save the new UserCourse
            var uc = new UserCourse
            {
                UserId = userId,
                CourseId = courseId
            };

            await _userCourseRepo.AddAsync(uc);
            await _userCourseRepo.SaveAsync();

            return Ok();
        }

        [HttpGet("{id}/chapter-count")]
        public async Task<IActionResult> GetChapterCount(int id)
        {
            var exists = await _courseRepo.GetByIdAsync(id);
            if (exists == null)
                return NotFound();

            var count = await _courseRepo.GetChapterCountAsync(id);
            return Ok(new { courseId = id, chapterCount = count });
        }

        [HttpGet("summary")]
        public async Task<IActionResult> GetCourseSummaries()
        {
            var summaries = await _courseRepo.GetSummariesAsync();
            return Ok(summaries);
        }

    }
}