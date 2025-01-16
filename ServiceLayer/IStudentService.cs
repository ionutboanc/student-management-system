// <copyright file="{fileName}" company="Transilvania University of Brasov">
// Copyright (c) 2025 Boanc Ionut-Silviu. All rights reserved.
// </copyright>

namespace ServiceLayer
{
    using System.Collections.Generic;
    using DomainModel;
    using DomainModel.Models;

    /// <summary>
    /// Provides an interface for student services.
    /// </summary>
    public interface IStudentService
    {
        /// <summary>
        /// Adds a new student.
        /// </summary>
        /// <param name="student">The student to add.</param>
        void AddStudent(Student student);

        /// <summary>
        /// Updates an existing student.
        /// </summary>
        /// <param name="student">The student to update.</param>
        void UpdateStudent(Student student);

        /// <summary>
        /// Deletes a student.
        /// </summary>
        /// <param name="student">The student to delete.</param>
        void DeleteStudent(Student student);

        /// <summary>
        /// Gets a student by their ID.
        /// </summary>
        /// <param name="studentId">The ID of the student.</param>
        /// <returns>The student with the specified ID.</returns>
        Student GetStudentById(int studentId);

        /// <summary>
        /// Gets all students.
        /// </summary>
        /// <returns>A list of all students.</returns>
        IList<Student> GetAllStudents();

        /// <summary>
        /// Gets a student by their national ID.
        /// </summary>
        /// <param name="nationalID">The national ID of the student.</param>
        /// <returns>The student with the specified national ID.</returns>
        Student GetStudentByNationalID(string nationalID);

        /// <summary>
        /// Gets the courses a student is enrolled in.
        /// </summary>
        /// <param name="student">The student to get courses for.</param>
        /// <returns>A list of courses the student is enrolled in.</returns>
        IList<Course> GetEnrolledCourses(Student student);

        /// <summary>
        /// Gets the exams for a student.
        /// </summary>
        /// <param name="student">The student to get exams for.</param>
        /// <returns>A list of exams for the student.</returns>
        IList<Exam> GetExamsForStudent(Student student);

        /// <summary>
        /// Gets the current semester for a student.
        /// </summary>
        /// <param name="student">The student to get the current semester for.</param>
        /// <returns>The current semester for the student.</returns>
        Semester GetCurrentSemester(Student student);

        /// <summary>
        /// Calculates the total cost of courses for a student.
        /// </summary>
        /// <param name="student">The student to calculate the cost for.</param>
        /// <returns>The total cost of courses for the student.</returns>
        decimal CalculateCoursesCostsForStudent(Student student);

        /// <summary>
        /// Enrolls a student in a course.
        /// </summary>
        /// <param name="student">The student to enroll.</param>
        /// <param name="course">The course to enroll the student in.</param>
        void EnrollStudentInCourse(Student student, Course course);

        /// <summary>
        /// Removes a student from a course.
        /// </summary>
        /// <param name="student">The student to remove.</param>
        /// <param name="course">The course to remove the student from.</param>
        void RemoveStudentFromCourse(Student student, Course course);

        /// <summary>
        /// Promotes a student to the next semester.
        /// </summary>
        /// <param name="student">The student to promote.</param>
        /// <returns>True if the student was promoted, otherwise false.</returns>
        bool PromoteStudentToNextSemester(Student student);

        /// <summary>
        /// Gets the credits per semester for a student.
        /// </summary>
        /// <param name="student">The student to get credits for.</param>
        /// <returns>A dictionary with the semester ID as the key and the total credits as the value.</returns>
        Dictionary<int, int> GetCreditsPerSemester(Student student);

        /// <summary>
        /// Counts the number of students who have passed all their courses.
        /// </summary>
        /// <returns>The number of students who have passed all their courses.</returns>
        int CountStudentsWhoPassed();
    }
}