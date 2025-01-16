// <copyright file="{fileName}" company="Transilvania University of Brasov">
// Copyright (c) 2025 Boanc Ionut-Silviu. All rights reserved.
// </copyright>

namespace ServiceLayer.ServiceImplementation
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using DataMapper;
    using DomainModel;
    using DomainModel.Models;
    using log4net;
    using Utilities;

    /// <summary>
    /// Provides services for managing exams.
    /// </summary>
    public class ExamService : IExamService
    {
        /// <summary>
        /// Logger instance for logging.
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(ExamService));

        /// <summary>
        /// Initializes a new instance of the <see cref="ExamService"/> class.
        /// </summary>
        public ExamService()
        {
            Logger.Info("ExamService initialized.");
        }

        /// <summary>
        /// Adds a new exam.
        /// </summary>
        /// <param name="exam">The exam to add.</param>
        public void AddExam(Exam exam)
        {
            Logger.Info("Adding a new exam.");
            this.ValidateExam(exam);
            DAOFactoryMethod.CurrentDAOFactory.ExamDataServices.AddExam(exam);
        }

        /// <summary>
        /// Updates an existing exam.
        /// </summary>
        /// <param name="exam">The exam to update.</param>
        public void UpdateExam(Exam exam)
        {
            Logger.Info("Updating an existing exam.");
            this.ValidateExam(exam);
            DAOFactoryMethod.CurrentDAOFactory.ExamDataServices.UpdateExam(exam);
        }

        /// <summary>
        /// Deletes an exam.
        /// </summary>
        /// <param name="exam">The exam to delete.</param>
        public void DeleteExam(Exam exam)
        {
            Logger.Info("Deleting an exam.");
            if (exam == null)
            {
                Logger.Error("Exam not found.", new ArgumentException("Exam not found."));
                throw new ArgumentException("Exam not found.");
            }

            DAOFactoryMethod.CurrentDAOFactory.ExamDataServices.DeleteExam(exam);
        }

        /// <summary>
        /// Gets an exam by its ID.
        /// </summary>
        /// <param name="examId">The ID of the exam.</param>
        /// <returns>The exam with the specified ID.</returns>
        public Exam GetExamById(int examId)
        {
            Logger.Info($"Getting exam by ID: {examId}.");
            if (examId <= 0)
            {
                Logger.Error("Exam ID must be greater than zero.", new ArgumentOutOfRangeException(nameof(examId), "Exam ID must be greater than zero."));
                throw new ArgumentOutOfRangeException(nameof(examId), "Exam ID must be greater than zero.");
            }

            return DAOFactoryMethod.CurrentDAOFactory.ExamDataServices.GetExamById(examId);
        }

        /// <summary>
        /// Gets all exams.
        /// </summary>
        /// <returns>A list of all exams.</returns>
        public IList<Exam> GetAllExams()
        {
            Logger.Info("Getting all exams.");
            return DAOFactoryMethod.CurrentDAOFactory.ExamDataServices.GetAllExams();
        }

        /// <summary>
        /// Gets the exams for a student.
        /// </summary>
        /// <param name="student">The student to get exams for.</param>
        /// <returns>A list of exams for the student.</returns>
        public IList<Exam> GetExamsByStudent(Student student)
        {
            Logger.Info("Getting exams for a student.");
            return DAOFactoryMethod.CurrentDAOFactory.ExamDataServices.GetAllExams()
                .Where(e => e.StudentId == student.Id).ToList();
        }

        /// <summary>
        /// Gets the exams for a course.
        /// </summary>
        /// <param name="course">The course to get exams for.</param>
        /// <returns>A list of exams for the course.</returns>
        public IList<Exam> GetExamsByCourse(Course course)
        {
            Logger.Info("Getting exams for a course.");
            return DAOFactoryMethod.CurrentDAOFactory.ExamDataServices.GetAllExams()
                .Where(e => e.CourseId == course.Id).ToList();
        }

        /// <summary>
        /// Checks if a student has too many attempts for an exam.
        /// </summary>
        /// <param name="exam">The exam to check.</param>
        /// <returns>True if the student has too many attempts, otherwise false.</returns>
        public bool HasTooManyAttempts(Exam exam)
        {
            Logger.Info("Checking if a student has too many attempts for an exam.");
            if (exam == null)
            {
                Logger.Error("Exam cannot be null.", new ArgumentNullException(nameof(exam), "Exam cannot be null."));
                throw new ArgumentNullException(nameof(exam), "Exam cannot be null.");
            }

            var examAttempts = DAOFactoryMethod.CurrentDAOFactory.ExamDataServices.GetAllExams()
                .Count(e => e.StudentId == exam.StudentId && e.CourseId == exam.CourseId);

            if (examAttempts >= Constants.MaxNumberOfAttempts)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Validates an exam.
        /// </summary>
        /// <param name="exam">The exam to validate.</param>
        private void ValidateExam(Exam exam)
        {
            Logger.Info("Validating an exam.");
            if (exam == null)
            {
                Logger.Error("Exam cannot be null.", new ArgumentNullException(nameof(exam), "Exam cannot be null."));
                throw new ArgumentNullException(nameof(exam), "Exam cannot be null.");
            }

            exam.Validate();

            var existingExams = DAOFactoryMethod.CurrentDAOFactory.ExamDataServices.GetAllExams()
                .Where(e => e.StudentId == exam.StudentId && e.ExamDate.Date == exam.ExamDate.Date && e.ExamId != exam.ExamId)
                .ToList();

            if (existingExams.Any())
            {
                Logger.Error("The student already has an exam scheduled on this day.", new ValidationException("The student already has an exam scheduled on this day."));
                throw new ValidationException("The student already has an exam scheduled on this day.");
            }

            if (this.HasTooManyAttempts(exam))
            {
                if (exam.Course.BalanceRefunded == false)
                {
                    exam.Course.BalanceRefunded = true;
                    exam.Student.AddBalance(exam.Course.GetCost());

                    DAOFactoryMethod.CurrentDAOFactory.CourseDataServices.UpdateCourse(exam.Course);
                    DAOFactoryMethod.CurrentDAOFactory.StudentDataServices.UpdateStudent(exam.Student);
                }

                Logger.Error($"The student has already taken the exam for this course {Constants.MaxNumberOfAttempts} times.", new ValidationException($"The student has already taken the exam for this course {Constants.MaxNumberOfAttempts} times."));
                throw new ValidationException($"The student has already taken the exam for this course {Constants.MaxNumberOfAttempts} times.");
            }
        }
    }
}