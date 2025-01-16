// <copyright file="{fileName}" company="Transilvania University of Brasov">
// Copyright (c) 2025 Boanc Ionut-Silviu. All rights reserved.
// </copyright>

namespace DataMapper.SqlServerDAO
{
    using System.Collections.Generic;
    using System.Linq;
    using DomainModel.Models;

    /// <summary>
    /// Provides data services for courses.
    /// </summary>
    public class SQLCourseDataServices : ICourseDataServices
    {
        /// <summary>
        /// Adds a course.
        /// </summary>
        /// <param name="course">The course to add.</param>
        public void AddCourse(Course course)
        {
            using (var context = new MyApplicationContext())
            {
                context.Courses.Add(course);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Deletes a course.
        /// </summary>
        /// <param name="course">The course to delete.</param>
        public void DeleteCourse(Course course)
        {
            using (var context = new MyApplicationContext())
            {
                var courseToDelete = new Course { Id = course.Id };
                context.Courses.Attach(courseToDelete);
                context.Courses.Remove(courseToDelete);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Gets all courses.
        /// </summary>
        /// <returns>A list of all courses.</returns>
        public IList<Course> GetAllCourses()
        {
            using (var context = new MyApplicationContext())
            {
                return context.Courses.ToList();
            }
        }

        /// <summary>
        /// Gets a course by its ID.
        /// </summary>
        /// <param name="id">The ID of the course.</param>
        /// <returns>The course with the specified ID.</returns>
        public Course GetCourseById(int id)
        {
            using (var context = new MyApplicationContext())
            {
                return context.Courses.SingleOrDefault(c => c.Id == id);
            }
        }

        /// <summary>
        /// Updates a course.
        /// </summary>
        /// <param name="course">The course to update.</param>
        public void UpdateCourse(Course course)
        {
            using (var context = new MyApplicationContext())
            {
                context.Entry(course).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            }
        }
    }
}