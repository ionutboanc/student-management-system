// <copyright file="{fileName}" company="Transilvania University of Brasov">
// Copyright (c) 2025 Boanc Ionut-Silviu. All rights reserved.
// </copyright>

namespace ServiceLayer
{
    using System.Collections.Generic;
    using DomainModel;
    using DomainModel.Models;

    /// <summary>
    /// Provides an interface for exam services.
    /// </summary>
    public interface IExamService
    {
        /// <summary>
        /// Adds a new exam.
        /// </summary>
        /// <param name="exam">The exam to add.</param>
        void AddExam(Exam exam);

        /// <summary>
        /// Updates an existing exam.
        /// </summary>
        /// <param name="exam">The exam to update.</param>
        void UpdateExam(Exam exam);

        /// <summary>
        /// Deletes an exam.
        /// </summary>
        /// <param name="exam">The exam to delete.</param>
        void DeleteExam(Exam exam);

        /// <summary>
        /// Gets an exam by its ID.
        /// </summary>
        /// <param name="examId">The ID of the exam.</param>
        /// <returns>The exam with the specified ID.</returns>
        Exam GetExamById(int examId);

        /// <summary>
        /// Gets all exams.
        /// </summary>
        /// <returns>A list of all exams.</returns>
        IList<Exam> GetAllExams();

        /// <summary>
        /// Gets the exams for a student.
        /// </summary>
        /// <param name="student">The student to get exams for.</param>
        /// <returns>A list of exams for the student.</returns>
        IList<Exam> GetExamsByStudent(Student student);

        /// <summary>
        /// Gets the exams for a course.
        /// </summary>
        /// <param name="course">The course to get exams for.</param>
        /// <returns>A list of exams for the course.</returns>
        IList<Exam> GetExamsByCourse(Course course);
    }
}