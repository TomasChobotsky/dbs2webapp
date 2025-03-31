using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace dbs2webapp.Models;

public partial class Dbs2databaseContext : DbContext
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

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Test> Tests { get; set; }

    public virtual DbSet<TestInstance> TestInstances { get; set; }

    public virtual DbSet<TestQuestion> TestQuestions { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserCourse> UserCourses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
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
        });

        modelBuilder.Entity<CourseCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Course_c__3214EC0711D06CA9");

            entity.ToTable("Course_category");

            entity.Property(e => e.Name).HasMaxLength(255);
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
        });

        modelBuilder.Entity<Question>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Question__3214EC07BE6F4E22");

            entity.ToTable("Question");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Role__3214EC0757F8FBB8");

            entity.ToTable("Role");

            entity.Property(e => e.Name).HasMaxLength(255);
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
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__User__3214EC07C0C0ECF6");

            entity.ToTable("User");

            entity.HasIndex(e => e.Email, "UQ__User__A9D10534C2FFE62C").IsUnique();

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
                .HasConstraintName("FK__User__RoleId__3A81B327");
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
        });


        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
