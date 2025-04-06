using ApplicationCore.Interfaces;
using ApplicationCore.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class CourseRepository : EfRepository<Course>, ICourseRepository
    {
        public CourseRepository(Dbs2databaseContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<Course>> GetCourseWithStudentsAsync()
        {
            return await _dbContext.Courses
                .Include(c => c.UserCourses)
                .ThenInclude(uc => uc.User)
                .ToListAsync();
        }

        public async Task<Course> AddCourseWithChapter(Course course, Chapter chapter)
        {
            course.Chapters.Add(chapter);
            await _dbContext.SaveChangesAsync();
            return course;
        }
    }
}
