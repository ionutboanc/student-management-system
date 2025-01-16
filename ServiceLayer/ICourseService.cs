// <copyright file="{fileName}" company="Transilvania University of Brasov">
// Copyright (c) 2025 Boanc Ionut-Silviu. All rights reserved.
// </copyright>

namespace ServiceLayer
{
    using System.Collections.Generic;
    using DomainModel;
    using DomainModel.Models;

    /// <summary>
    /// Provides an interface for course services.
    /// </summary>
    public interface ICourseService
    {
        /// <summary>
        /// Adds a new course.
        /// </summary>
        /// <param name="course">The course to add.</param>
        void AddCourse(Course course);

        /// <summary>
        /// Updates an existing course.
        /// </summary>
        /// <param name="course">The course to update.</param>
        void UpdateCourse(Course course);

        /// <summary>
        /// Deletes a course.
        /// </summary>
        /// <param name="course">The course to delete.</param>
        void DeleteCourse(Course course);

        /// <summary>
        /// Gets a course by its ID.
        /// </summary>
        /// <param name="courseId">The ID of the course.</param>
        /// <returns>The course with the specified ID.</returns>
        Course GetCourseById(int courseId);

        /// <summary>
        /// Gets all courses.
        /// </summary>
        /// <returns>A list of all courses.</returns>
        IList<Course> GetAllCourses();

        /// <summary>
        /// Gets the prerequisites for a course.
        /// </summary>
        /// <param name="course">The course to get prerequisites for.</param>
        /// <returns>A list of prerequisite courses.</returns>
        IList<Course> GetPrerequisites(Course course);

        /// <summary>
        /// Gets the discount for a course.
        /// </summary>
        /// <param name="course">The course to get the discount for.</param>
        /// <returns>The discount for the course.</returns>
        Discount GetDiscount(Course course);

        /// <summary>
        /// Validates a course.
        /// </summary>
        /// <param name="course">The course to validate.</param>
        void ValidateCourse(Course course);

        /// <summary>
        /// Gets the students enrolled in a course.
        /// </summary>
        /// <param name="course">The course to get students for.</param>
        /// <returns>A list of students enrolled in the course.</returns>
        IList<Student> GetEnrolledStudents(Course course);

        /// <summary>
        /// Gets the exams for a course.
        /// </summary>
        /// <param name="course">The course to get exams for.</param>
        /// <returns>A list of exams for the course.</returns>
        IList<Exam> GetExamsForCourse(Course course);

        /// <summary>
        /// Gets the semesters for a course.
        /// </summary>
        /// <param name="course">The course to get semesters for.</param>
        /// <returns>A list of semesters for the course.</returns>
        IList<Semester> GetSemestersForCourse(Course course);

        /// <summary>
        /// Gets the courses for a semester.
        /// </summary>
        /// <param name="semester">The semester to get courses for.</param>
        /// <returns>A list of courses for the semester.</returns>
        IList<Course> GetCoursesBySemester(Semester semester);

        /// <summary>
        /// Calculates the average grades above five for a course.
        /// </summary>
        /// <param name="course">The course to calculate the average grades for.</param>
        /// <returns>The average grades above five.</returns>
        double CalculateAverageGradesAboveFive(Course course);

        /// <summary>
        /// Calculates the average number of courses per student.
        /// </summary>
        /// <returns>The average number of courses per student.</returns>
        double CalculateAverageNumberOfCoursesPerStudent();

        /// <summary>
        /// Gets the number of students enrolled in a course.
        /// </summary>
        /// <param name="course">The course to get the number of enrolled students for.</param>
        /// <returns>The number of students enrolled in the course.</returns>
        int GetNumberOfEnrolledStudents(Course course);
    }
}