using ApplicationCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    /// <summary>
    /// Currently just a example, in case custom repositories are needed in the future
    /// </summary>
    public interface ICourseRepository : IAsyncRepository<Course>
    {
        Task<List<Course>> GetCourseWithStudentsAsync();
        Task<Course> AddCourseWithChapter(Course course, Chapter chapter);
    }
}
