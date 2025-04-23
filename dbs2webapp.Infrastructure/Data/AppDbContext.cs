using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using dbs2webapp.Domain.Entities;

namespace Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<IdentityUser, IdentityRole, string>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Chapter> Chapters { get; set; }
        public DbSet<Test> Tests { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Option> Options { get; set; }
        public DbSet<UserCourse> UserCourses { get; set; }
        public DbSet<TestResult> TestResults { get; set; }
        public DbSet<TestAnswer> TestAnswers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Course>()
                .HasOne(c => c.Teacher)
                .WithMany()
                .HasForeignKey(c => c.TeacherId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete

            modelBuilder.Entity<UserCourse>()
                .HasOne(uc => uc.User)
                .WithMany()
                .HasForeignKey(uc => uc.UserId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete when user is deleted

            modelBuilder.Entity<UserCourse>()
                .HasOne(uc => uc.Course)
                .WithMany(c => c.UserCourses)
                .HasForeignKey(uc => uc.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Chapter>()
                .HasOne(ch => ch.Course)
                .WithMany(c => c.Chapters)
                .HasForeignKey(ch => ch.CourseId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete when course is deleted

            modelBuilder.Entity<TestResult>()
                .HasOne(tr => tr.User)
                .WithMany()
                .HasForeignKey(tr => tr.UserId);

            modelBuilder.Entity<TestResult>()
                .HasOne(tr => tr.Test)
                .WithMany()
                .HasForeignKey(tr => tr.TestId);

            modelBuilder.Entity<TestAnswer>(ta =>
            {
                ta.HasKey(e => e.Id);

                ta.HasOne(e => e.TestResult)
                  .WithMany(r => r.Answers)
                  .HasForeignKey(e => e.TestResultId)
                  .OnDelete(DeleteBehavior.Cascade);

                ta.HasOne(e => e.Question)
                  .WithMany(q => q.Answers)
                  .HasForeignKey(e => e.QuestionId)
                  .OnDelete(DeleteBehavior.Restrict);

                ta.HasOne(e => e.SelectedOption)
                  .WithMany()
                  .HasForeignKey(e => e.SelectedOptionId)
                  .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}