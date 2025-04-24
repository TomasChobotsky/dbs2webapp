using Application.Interfaces;
using dbs2webapp.Application.Interfaces;
using dbs2webapp.Domain.Entities;
using dbs2webapp.Infrastructure.Repositories;
using Infrastructure.Data;
using Infrastructure.Mapping;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAppServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(config.GetConnectionString("DefaultConnection")));

            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddScoped<ITestResultRepository, TestResultRepository>();
            services.AddScoped<IBaseRepository<Course>, CourseRepository>();
            services.AddScoped<ICourseRepository, CourseRepository>();


            services.AddAutoMapper(typeof(AutoMapperProfile));

            return services;
        }
    }
}