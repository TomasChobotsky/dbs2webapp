using dbs2webapp.Application.DTOs;
using dbs2webapp.Application.Interfaces;
using dbs2webapp.Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace dbs2webapp.Infrastructure.Repositories
{
    public class CourseRepository : BaseRepository<Course>, ICourseRepository
    {
        private readonly AppDbContext _db;

        public CourseRepository(AppDbContext context) : base(context)
        {
            _db = context;
        }

        public async Task<int> SaveViaProcedureAsync(Course course, bool isDelete)
        {
            var idParam = course.Id;
            var name = course.Name ?? "";
            var desc = course.Description ?? "";
            var teacherId = course.TeacherId ?? "";

            return await _db.Database.ExecuteSqlInterpolatedAsync(
                $@"EXEC sp_Course_Manage 
                @Id = {idParam},
                @Name = {name},
                @Description = {desc},
                @TeacherId = {teacherId},
                @IsDelete = {isDelete}");
        }

        public async Task<int> GetChapterCountAsync(int courseId)
        {
            var conn = _db.Database.GetDbConnection();
            await conn.OpenAsync();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT dbo.fn_GetChapterCount(@CourseId)";
            cmd.CommandType = CommandType.Text;

            var param = cmd.CreateParameter();
            param.ParameterName = "@CourseId";
            param.Value = courseId;
            cmd.Parameters.Add(param);

            var result = await cmd.ExecuteScalarAsync();
            return Convert.ToInt32(result);
        }

        public async Task<List<CourseSummaryDto>> GetSummariesAsync()
        {
            return await _db.Set<CourseSummaryDto>()
                .FromSqlRaw("SELECT * FROM vw_CourseSummary")
                .ToListAsync();
        }
    }
}