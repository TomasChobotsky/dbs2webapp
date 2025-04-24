using Application.Interfaces;
using dbs2webapp.Application.DTOs;
using dbs2webapp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dbs2webapp.Application.Interfaces
{
    public interface ICourseRepository : IBaseRepository<Course>
    {
        Task<int> SaveViaProcedureAsync(Course course, bool isDelete);
        Task<int> GetChapterCountAsync(int courseId);
        Task<List<CourseSummaryDto>> GetSummariesAsync();
    }
}
