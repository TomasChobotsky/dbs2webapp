using ApplicationCore.Models;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class CourseRepository : EfRepository<CourseRepository>, ICourseRepository
    {
        public CourseRepository(Dbs2databaseContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<Course>> GetCoursesWithStudentsAsync()
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
