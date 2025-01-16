// <copyright file="{fileName}" company="Transilvania University of Brasov">
// Copyright (c) 2025 Boanc Ionut-Silviu. All rights reserved.
// </copyright>

namespace DataMapper
{
    using System.Collections.Generic;
    using DomainModel.Models;

    /// <summary>
    /// Provides an interface for student data services.
    /// </summary>
    public interface IStudentDataServices
    {
        /// <summary>
        /// Gets all students.
        /// </summary>
        /// <returns>A list of all students.</returns>
        IList<Student> GetAllStudents();

        /// <summary>
        /// Adds a new student.
        /// </summary>
        /// <param name="student">The student to add.</param>
        void AddStudent(Student student);

        /// <summary>
        /// Deletes a student.
        /// </summary>
        /// <param name="student">The student to delete.</param>
        void DeleteStudent(Student student);

        /// <summary>
        /// Updates an existing student.
        /// </summary>
        /// <param name="student">The student to update.</param>
        void UpdateStudent(Student student);

        /// <summary>
        /// Gets a student by their ID.
        /// </summary>
        /// <param name="id">The ID of the student.</param>
        /// <returns>The student with the specified ID.</returns>
        Student GetStudentById(int id);
    }
}