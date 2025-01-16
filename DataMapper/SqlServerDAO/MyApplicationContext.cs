// <copyright file="{fileName}" company="Transilvania University of Brasov">
// Copyright (c) 2025 Boanc Ionut-Silviu. All rights reserved.
// </copyright>

namespace DataMapper.SqlServerDAO
{
    using System.Data.Entity;
    using DomainModel;
    using DomainModel.Models;

    /// <summary>
    /// Represents the database context for the application.
    /// </summary>
    public class MyApplicationContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MyApplicationContext"/> class.
        /// </summary>
        public MyApplicationContext()
    : base("server=VIVOBOOK-IONUT; database=student_db; Trusted_Connection=True;")
        {
        }

        /// <summary>
        /// Gets or sets the Students.
        /// </summary>
        public virtual DbSet<Student> Students { get; set; }

        /// <summary>
        /// Gets or sets the Courses.
        /// </summary>
        public virtual DbSet<Course> Courses { get; set; }

        /// <summary>
        /// Gets or sets the Semesters.
        /// </summary>
        public virtual DbSet<Semester> Semesters { get; set; }

        /// <summary>
        /// Gets or sets the Discounts.
        /// </summary>
        public virtual DbSet<Discount> Discounts { get; set; }

        /// <summary>
        /// Gets or sets the Exams.
        /// </summary>
        public virtual DbSet<Exam> Exams { get; set; }

        /// <summary>
        /// Configures the model.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Many-to-Many: Prerequisites between courses
            modelBuilder.Entity<Course>()
                .HasMany(c => c.Prerequisites)
                .WithMany()
                .Map(m =>
                {
                    m.MapLeftKey("CourseId");
                    m.MapRightKey("PrerequisiteId");
                    m.ToTable("CoursePrerequisites");
                });

            // Many-to-Many: Courses <-> Semesters
            modelBuilder.Entity<Course>()
                .HasMany(c => c.AvailableSemesters)
                .WithMany(s => s.AvailableCourses)
                .Map(m =>
                {
                    m.MapLeftKey("CourseId");
                    m.MapRightKey("SemesterId");
                    m.ToTable("CourseSemesters");
                });

            // One-to-Many: Discount -> Courses
            modelBuilder.Entity<Discount>()
                .HasMany(d => d.Courses)
                .WithOptional(c => c.Discount)
                .HasForeignKey(c => c.DiscountId);

            // Many-to-Many: Students <-> Courses
            modelBuilder.Entity<Student>()
                .HasMany(s => s.EnrolledCourses)
                .WithMany(c => c.Students)
                .Map(m =>
                {
                    m.MapLeftKey("StudentId");
                    m.MapRightKey("CourseId");
                    m.ToTable("StudentCourses");
                });

            // One-to-Many: Exam -> Student
            modelBuilder.Entity<Exam>()
                .HasRequired(e => e.Student)
                .WithMany(s => s.Exams)
                .HasForeignKey(e => e.StudentId)
                .WillCascadeOnDelete(true);

            // One-to-Many: Exam -> Course
            modelBuilder.Entity<Exam>()
                .HasRequired(e => e.Course)
                .WithMany(c => c.Exams)
                .HasForeignKey(e => e.CourseId)
                .WillCascadeOnDelete(false);

            // One-to-Many: Exam -> Semester
            modelBuilder.Entity<Exam>()
                .HasRequired(e => e.Semester)
                .WithMany(s => s.Exams)
                .HasForeignKey(e => e.SemesterId)
                .WillCascadeOnDelete(false);

            // One-to-Many: Student -> Semester
            modelBuilder.Entity<Student>()
                .HasOptional(s => s.CurrentSemester)
                .WithMany(s => s.Students)
                .HasForeignKey(s => s.CurrentSemesterId)
                .WillCascadeOnDelete(false);
        }
    }
}