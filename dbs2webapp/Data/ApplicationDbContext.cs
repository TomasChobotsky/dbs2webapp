using dbs2webapp.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace dbs2webapp.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser, IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships and any additional model configuration
            modelBuilder.Entity<UserCourse>()
                .HasKey(uc => uc.Id);

            modelBuilder.Entity<UserCourse>()
                .HasOne(uc => uc.User)
                .WithMany()
                .HasForeignKey(uc => uc.UserId);

            modelBuilder.Entity<UserCourse>()
                .HasOne(uc => uc.Course)
                .WithMany()
                .HasForeignKey(uc => uc.CourseId);

            modelBuilder.Entity<TestResult>()
                .HasOne(tr => tr.User)
                .WithMany()
                .HasForeignKey(tr => tr.UserId);

            modelBuilder.Entity<TestResult>()
                .HasOne(tr => tr.Test)
                .WithMany()
                .HasForeignKey(tr => tr.TestId);

            // Add any additional configuration here

            // Seed initial data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed Courses
            var course1 = new Course { Id = 1, Name = "Introduction to Programming" };
            var course2 = new Course { Id = 2, Name = "Web Development Fundamentals" };

            modelBuilder.Entity<Course>().HasData(course1, course2);

            // Seed Chapters for course1
            var chapter1 = new Chapter { Id = 1, Name = "Getting Started with C#", Content = "Basic syntax and structure...", CourseId = 1 };
            var chapter2 = new Chapter { Id = 2, Name = "Object-Oriented Programming", Content = "Classes, objects, inheritance...", CourseId = 1 };
            var chapter3 = new Chapter { Id = 3, Name = "HTML Basics", Content = "HTML tags and structure...", CourseId = 2 };

            modelBuilder.Entity<Chapter>().HasData(chapter1, chapter2, chapter3);

            // Seed Tests
            var test1 = new Test { Id = 1, Title = "C# Basics Quiz", ChapterId = 1 };
            var test2 = new Test { Id = 2, Title = "OOP Concepts Test", ChapterId = 2 };

            modelBuilder.Entity<Test>().HasData(test1, test2);

            // Seed Questions for test1
            var question1 = new Question { Id = 1, Content = "What is the entry point of a C# program?", TestId = 1 };
            var question2 = new Question { Id = 2, Content = "Which keyword is used to declare a variable in C#?", TestId = 1 };

            modelBuilder.Entity<Question>().HasData(question1, question2);

            // Seed Options for question1
            var option1 = new Option { Id = 1, Text = "Main()", IsCorrect = true, QuestionId = 1 };
            var option2 = new Option { Id = 2, Text = "Start()", IsCorrect = false, QuestionId = 1 };
            var option3 = new Option { Id = 3, Text = "Init()", IsCorrect = false, QuestionId = 1 };
            var option4 = new Option { Id = 4, Text = "var", IsCorrect = true, QuestionId = 2 };
            var option5 = new Option { Id = 5, Text = "variable", IsCorrect = false, QuestionId = 2 };

            modelBuilder.Entity<Option>().HasData(option1, option2, option3, option4, option5);
        }
    }
}
