// <copyright file="{fileName}" company="Transilvania University of Brasov">
// Copyright (c) 2025 Boanc Ionut-Silviu. All rights reserved.
// </copyright>

namespace DataMapper
{
    using System.Collections.Generic;
    using DomainModel.Models;

    /// <summary>
    /// Provides data services for courses.
    /// </summary>
    public interface ICourseDataServices
    {
        /// <summary>
        /// Gets all courses.
        /// </summary>
        /// <returns>A list of all courses.</returns>
        IList<Course> GetAllCourses();

        /// <summary>
        /// Adds a new course.
        /// </summary>
        /// <param name="course">The course to add.</param>
        void AddCourse(Course course);

        /// <summary>
        /// Deletes a course.
        /// </summary>
        /// <param name="course">The course to delete.</param>
        void DeleteCourse(Course course);

        /// <summary>
        /// Updates an existing course.
        /// </summary>
        /// <param name="course">The course to update.</param>
        void UpdateCourse(Course course);

        /// <summary>
        /// Gets a course by its ID.
        /// </summary>
        /// <param name="id">The ID of the course.</param>
        /// <returns>The course with the specified ID.</returns>
        Course GetCourseById(int id);
    }
}