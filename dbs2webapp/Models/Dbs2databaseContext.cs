using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace dbs2webapp.Models;

public partial class Dbs2databaseContext : IdentityDbContext<IdentityUser>
{
    public Dbs2databaseContext()
    {
    }

    public Dbs2databaseContext(DbContextOptions<Dbs2databaseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Assignment> Assignments { get; set; }

    public virtual DbSet<AssignmentUser> AssignmentUsers { get; set; }

    public virtual DbSet<AssignmentSubmission> AssignmentSubmissions { get; set; }

    public virtual DbSet<Chapter> Chapters { get; set; }

    public virtual DbSet<ChapterContent> ChapterContents { get; set; }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<CourseCategory> CourseCategories { get; set; }

    public virtual DbSet<Option> Options { get; set; }

    public virtual DbSet<Question> Questions { get; set; }

    public virtual DbSet<AccountRole> AccountRoles { get; set; }

    public virtual DbSet<Test> Tests { get; set; }

    public virtual DbSet<TestInstance> TestInstances { get; set; }

    public virtual DbSet<TestQuestion> TestQuestions { get; set; }

    public virtual DbSet<AppUser> AppUsers { get; set; }

    public virtual DbSet<UserCourse> UserCourses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<IdentityUser>(entity =>
        {
            entity.ToTable("AspNetUsers");
            
        });

        modelBuilder.Entity<AccountRole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__AccountRole__3214EC0757F8FBB8");

            entity.ToTable("AccountRole");

            entity.Property(e => e.Name).HasMaxLength(255);

            entity.HasData(
                new AccountRole { Id = 1, Name = "Student" },
                new AccountRole { Id = 2, Name = "Učitel" },
                new AccountRole { Id = 3, Name = "Administrátor" }
            );
        });

        modelBuilder.Entity<AppUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__AppUser__3214EC07C0C0ECF6");

            entity.ToTable("AppUser");

            entity.HasIndex(e => e.Email, "UQ__AppUser__A9D10534C2FFE62C").IsUnique();

            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.Firstname).HasMaxLength(255);
            entity.Property(e => e.Password)
                .HasMaxLength(60)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Surname).HasMaxLength(255);

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__AppUser__RoleId__3A81B327");

            entity.HasData(
                new AppUser { Id = 1, Email = "student1@example.com", Password = "hashed_password1", Firstname = "Jan", Surname = "Novák", RoleId = 1 },
                new AppUser { Id = 2, Email = "ucitel1@example.com", Password = "hashed_password2", Firstname = "Petr", Surname = "Dvořák", RoleId = 2 },
                new AppUser { Id = 3, Email = "admin1@example.com", Password = "hashed_password3", Firstname = "Karel", Surname = "Svoboda", RoleId = 3 }
            );
        });

        modelBuilder.Entity<CourseCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Course_c__3214EC0711D06CA9");

            entity.ToTable("Course_category");

            entity.Property(e => e.Name).HasMaxLength(255);

            entity.HasData(
                new CourseCategory { Id = 1, Name = "Programování" },
                new CourseCategory { Id = 2, Name = "Databáze" },
                new CourseCategory { Id = 3, Name = "Kybernetická bezpečnost" }
            );
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Course__3214EC07ADE10529");

            entity.ToTable("Course");

            entity.Property(e => e.CourseCategoryId).HasColumnName("Course_categoryId");
            entity.Property(e => e.Name).HasMaxLength(255);

            entity.HasOne(d => d.CourseCategory).WithMany(p => p.Courses)
                .HasForeignKey(d => d.CourseCategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Course__Course_c__3F466844");

            entity.HasData(
                new Course { Id = 1, Name = "SQL pro začátečníky", CourseCategoryId = 2 },
                new Course { Id = 2, Name = "C# pokročilé techniky", CourseCategoryId = 1 },
                new Course { Id = 3, Name = "Základy kybernetické bezpečnosti", CourseCategoryId = 3 }
            );
        });

        modelBuilder.Entity<UserCourse>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__User_Cou__3214EC07D1836DD5");

            entity.ToTable("User_Course");

            entity.HasOne(d => d.Course).WithMany(p => p.UserCourses)
                .HasForeignKey(d => d.CourseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__User_Cour__Cours__4316F928");

            entity.HasOne(d => d.User).WithMany(p => p.UserCourses)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__User_Cour__UserI__4222D4EF");

            entity.HasData(
                new UserCourse { Id = 1, UserId = 1, CourseId = 1 },
                new UserCourse { Id = 2, UserId = 1, CourseId = 3 },
                new UserCourse { Id = 3, UserId = 2, CourseId = 1 }
            );
        });

        modelBuilder.Entity<Chapter>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Chapter__3214EC0715667D5C");

            entity.ToTable("Chapter");

            entity.Property(e => e.Name).HasMaxLength(255);

            entity.HasOne(d => d.Course).WithMany(p => p.Chapters)
                .HasForeignKey(d => d.CourseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Chapter__CourseI__45F365D3");

            entity.HasData(
                new Chapter { Id = 1, Name = "Úvod do SQL", Contents = "Tato kapitola se věnuje základům SQL.", CourseId = 1 },
                new Chapter { Id = 2, Name = "SELECT příkazy", Contents = "Jak správně psát SELECT dotazy.", CourseId = 1 },
                new Chapter { Id = 3, Name = "Úvod do C#", Contents = "Začínáme s programováním v C#.", CourseId = 2 }
            );
        });

        modelBuilder.Entity<ChapterContent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Chapter___3214EC07967DB6BE");

            entity.ToTable("Chapter_content");

            entity.Property(e => e.Type).HasMaxLength(255);

            entity.HasOne(d => d.Chapter).WithMany(p => p.ChapterContents)
                .HasForeignKey(d => d.ChapterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Chapter_c__Chapt__48CFD27E");

            entity.HasData(
                new ChapterContent { Id = 1, Type = "Text", TextFile = "uvod_sql.pdf", ChapterId = 1 },
                new ChapterContent { Id = 2, Type = "Video", TextFile = "select_prikazy.mp4", ChapterId = 2 },
                new ChapterContent { Id = 3, Type = "Text", TextFile = "uvod_csharp.pdf", ChapterId = 3 }
            );
        });

        modelBuilder.Entity<Test>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Test__3214EC077FA06AA2");

            entity.ToTable("Test");

            entity.Property(e => e.Title).HasMaxLength(255);

            entity.HasOne(d => d.Chapter).WithMany(p => p.Tests)
                .HasForeignKey(d => d.ChapterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Test__ChapterId__4BAC3F29");

            entity.HasData(
                new Test { Id = 1, Title = "Test základů SQL", Description = "Ověření znalostí základních SQL příkazů.", MaxAttempts = 3, ChapterId = 1 },
                new Test { Id = 2, Title = "Test SELECT", Description = "Test zaměřený na SELECT příkazy.", MaxAttempts = 3, ChapterId = 2 }
            );
        });

        modelBuilder.Entity<Question>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Question__3214EC07BE6F4E22");

            entity.ToTable("Question");

            entity.HasData(
                new Question { Id = 1, Content = "Co znamená SQL?" },
                new Question { Id = 2, Content = "Který příkaz získává všechna data z tabulky?" }
            );
        });

        modelBuilder.Entity<TestQuestion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Test_Que__3214EC076A260717");

            entity.ToTable("Test_Question");

            entity.HasOne(d => d.Question).WithMany(p => p.TestQuestions)
                .HasForeignKey(d => d.QuestionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Test_Ques__Quest__5441852A");

            entity.HasOne(d => d.Test).WithMany(p => p.TestQuestions)
                .HasForeignKey(d => d.TestId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Test_Ques__TestI__5535A963");

            entity.HasData(
                new TestQuestion { Id = 1, QuestionId = 1, TestId = 1 },
                new TestQuestion { Id = 2, QuestionId = 2, TestId = 2 }
            );
        });

        modelBuilder.Entity<Option>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Option__3214EC0720FD934B");

            entity.ToTable("Option");

            entity.Property(e => e.Text).HasMaxLength(255);

            entity.HasOne(d => d.Question).WithMany(p => p.Options)
                .HasForeignKey(d => d.QuestionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Option__Question__5812160E");

            entity.HasData(
                new Option { Id = 1, Text = "Structured Query Language", IsCorrect = true, QuestionId = 1 },
                new Option { Id = 2, Text = "System Query List", IsCorrect = false, QuestionId = 1 },
                new Option { Id = 3, Text = "SELECT * FROM tabulka;", IsCorrect = true, QuestionId = 2 },
                new Option { Id = 4, Text = "GET ALL FROM tabulka;", IsCorrect = false, QuestionId = 2 }
            );
        });

        modelBuilder.Entity<Assignment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Assignme__3214EC072522C8E5");

            entity.ToTable("Assignment");

            entity.Property(e => e.DueDate).HasColumnType("datetime");
            entity.Property(e => e.Title).HasMaxLength(255);

            entity.HasOne(d => d.Chapter).WithMany(p => p.Assignments)
                .HasForeignKey(d => d.ChapterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Assignmen__Chapt__5BE2A6F2");

            entity.HasOne(d => d.Teacher).WithMany(p => p.Assignments)
                .HasForeignKey(d => d.TeacherId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Assignmen__UserI__5AEE82B9");

            entity.HasData(
                new Assignment { Id = 1, Title = "Domácí úkol 1", Description = "Vytvořte jednoduchou SQL tabulku.", DueDate = DateTime.Parse("2025-03-30"), TeacherId = 2, ChapterId = 1 }
            );
        });


        modelBuilder.Entity<AssignmentSubmission>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Assignme__3214EC079993DCD9");

            entity.ToTable("Assignment_submission");

            entity.HasOne(d => d.Assignment).WithMany(p => p.AssignmentSubmissions)
                .HasForeignKey(d => d.AssignmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Assignmen__Assig__5EBF139D");

            entity.HasOne(d => d.User).WithMany(p => p.AssignmentSubmissions)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Assignmen__UserI__5FB337D6");

            entity.HasData(
                new AssignmentSubmission { Id = 1, Text = "Vytvořil jsem tabulku users.", File = "users_script.sql", AssignmentId = 1, UserId = 1 }
            );
        });

        modelBuilder.Entity<AssignmentUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Assignment_User");

            entity.ToTable("Assignment_User");

            entity.HasOne(d => d.Assignment).WithMany(p => p.AssignmentUsers)
                .HasForeignKey(d => d.AssignmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Assignment_User_Assignment");

            entity.HasOne(d => d.User).WithMany(p => p.AssignmentUsers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Assignment_User");

            entity.HasData(
                new AssignmentUser { Id = 1, AssignmentId = 1, UserId = 1 }
            );
        });

        modelBuilder.Entity<TestInstance>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Test_ins__3214EC07C1C5D6D4");

            entity.ToTable("Test_instance");

            entity.Property(e => e.StartedAt).HasColumnType("datetime");

            entity.HasOne(d => d.Test).WithMany(p => p.TestInstances)
                .HasForeignKey(d => d.TestId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Test_inst__TestI__4E88ABD4");

            entity.HasOne(d => d.User).WithMany(p => p.TestInstances)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Test_inst__UserI__4F7CD00D");

            entity.HasData(
                new TestInstance { Id = 1, StartedAt = DateTime.Parse("2025-03-18 10:00:00"), Attempt = 1, TestId = 1, UserId = 1 }
            );
        });


        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
