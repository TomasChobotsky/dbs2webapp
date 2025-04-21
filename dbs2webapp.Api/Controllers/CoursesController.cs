using dbs2webapp.Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CoursesController : ControllerBase
    {
        private readonly IBaseRepository<Course> _courseRepo;
        private readonly IBaseRepository<UserCourse> _userCourseRepo;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IMapper _mapper;

        public CoursesController(
            IBaseRepository<Course> courseRepo,
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
            var courses = await _courseRepo.GetAllAsync();
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
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var entity = _mapper.Map<Course>(dto);
            entity.TeacherId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            entity.CreatedDate = DateTime.UtcNow;

            await _courseRepo.AddAsync(entity);
            await _courseRepo.SaveAsync();

            var resultDto = _mapper.Map<CourseDto>(entity);
            return CreatedAtAction(nameof(GetById), new { id = entity.Id }, resultDto);
        }

        // PUT: api/courses/{id}
        [Authorize(Roles = "Teacher,Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateCourseDto dto)
        {
            var course = await _courseRepo.GetByIdAsync(id);
            if (course == null)
                return NotFound();

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (course.TeacherId != userId && !User.IsInRole("Admin"))
                return Forbid();

            _mapper.Map(dto, course); // Map updated values onto existing entity
            await _courseRepo.SaveAsync();

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

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (course.TeacherId != userId && !User.IsInRole("Admin"))
                return Forbid();

            _courseRepo.Remove(course);
            await _courseRepo.SaveAsync();

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
    }
}