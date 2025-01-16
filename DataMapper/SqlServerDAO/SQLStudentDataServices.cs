// <copyright file="{fileName}" company="Transilvania University of Brasov">
// Copyright (c) 2025 Boanc Ionut-Silviu. All rights reserved.
// </copyright>

namespace DataMapper.SqlServerDAO
{
    using System.Collections.Generic;
    using System.Linq;
    using DomainModel.Models;

    /// <summary>
    /// Provides SQL Server data services for managing students.
    /// </summary>
    internal class SQLStudentDataServices : IStudentDataServices
    {
        /// <summary>
        /// Gets all students.
        /// </summary>
        /// <returns>A list of all students.</returns>
        public IList<Student> GetAllStudents()
        {
            using (var context = new MyApplicationContext())
            {
                return context.Students.ToList();
            }
        }

        /// <summary>
        /// Gets a student by their ID.
        /// </summary>
        /// <param name="id">The ID of the student.</param>
        /// <returns>The student with the specified ID.</returns>
        public Student GetStudentById(int id)
        {
            using (var context = new MyApplicationContext())
            {
                return context.Students.SingleOrDefault(s => s.Id == id);
            }
        }

        /// <summary>
        /// Adds a new student.
        /// </summary>
        /// <param name="student">The student to add.</param>
        public void AddStudent(Student student)
        {
            using (var context = new MyApplicationContext())
            {
                context.Students.Add(student);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Deletes a student.
        /// </summary>
        /// <param name="student">The student to delete.</param>
        public void DeleteStudent(Student student)
        {
            using (var context = new MyApplicationContext())
            {
                var studentToDelete = new Student { Id = student.Id };
                context.Students.Attach(studentToDelete);
                context.Students.Remove(studentToDelete);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Updates an existing student.
        /// </summary>
        /// <param name="student">The student to update.</param>
        public void UpdateStudent(Student student)
        {
            using (var context = new MyApplicationContext())
            {
                context.Entry(student).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            }
        }
    }
}