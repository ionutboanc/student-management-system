// <copyright file="MyApplicationContext.cs" company="Transilvania University of Brasov">
// Copyright (c) Transilvania University of Brasov. All rights reserved.
// </copyright>

namespace DataMapper.SqlServerDAO
{
    using System.Data.Entity;
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
            : base("myConStr")
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
            /// Gets or sets the Enrollments.
            /// </summary>
            public virtual DbSet<Enrollment> Enrollments { get; set; }

            /// <summary>
            /// Gets or sets the Prerequisites.
            /// </summary>
            public virtual DbSet<Prerequisite> Prerequisites { get; set; }

            /// <summary>
            /// Configures the model.
            /// </summary>
            /// <param name="modelBuilder">The model builder.</param>
            protected override void OnModelCreating(DbModelBuilder modelBuilder)
            {
                modelBuilder.Entity<Course>()
                    .HasMany(c => c.Prerequisites)
                    .WithRequired(p => p.Course)
                    .HasForeignKey(p => p.CourseId)
                    .WillCascadeOnDelete(false);

                modelBuilder.Entity<Course>()
                    .HasMany(c => c.AvailableSemesters)
                    .WithMany(s => s.AvailableCourses)
                    .Map(cs =>
                    {
                        cs.MapLeftKey("CourseId");
                        cs.MapRightKey("SemesterId");
                        cs.ToTable("CourseSemesters");
                    });

                modelBuilder.Entity<Student>()
                    .HasMany(s => s.Enrollments)
                    .WithRequired(e => e.Student)
                    .HasForeignKey(e => e.StudentId);

                modelBuilder.Entity<Course>()
                    .HasMany(c => c.Enrollments)
                    .WithRequired(e => e.Course)
                    .HasForeignKey(e => e.CourseId);

                base.OnModelCreating(modelBuilder);
            }
        }
}