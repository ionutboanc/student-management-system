namespace DomainModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using DomainModel.Models;

    /// <summary>
    /// Model for Exam.
    /// </summary>
    public class Exam
    {
        /// <summary>
        /// Gets or sets the ID of the exam.
        /// </summary>
        [Key]
        public int ExamId { get; set; }

        /// <summary>
        /// Gets or sets the ID of the student taking the exam.
        /// </summary>
        [Required(ErrorMessage = "Student ID is required.")]
        public int StudentId { get; set; }

        /// <summary>
        /// Gets or sets the ID of the semester in which the exam is taken.
        /// </summary>
        [Required(ErrorMessage = "Semester ID is required.")]
        public int SemesterId { get; set; }

        /// <summary>
        /// Gets or sets the ID of the course for which the exam is taken.
        /// </summary>
        [Required(ErrorMessage = "Course ID is required.")]
        public int CourseId { get; set; }

        /// <summary>
        /// Gets or sets the grade of the exam.
        /// </summary>
        [Range(1, 10, ErrorMessage = "The grade must be between 1 and 10.")]
        public int Grade { get; set; }

        /// <summary>
        /// Gets or sets the date of the exam.
        /// </summary>
        [Required(ErrorMessage = "The exam date is required.")]
        public DateTime ExamDate { get; set; }

        /// <summary>
        /// Gets or sets the student taking the exam.
        /// </summary>
        public Student Student { get; set; }

        /// <summary>
        /// Gets or sets the semester in which the exam is taken.
        /// </summary>
        public Semester Semester { get; set; }

        /// <summary>
        /// Gets or sets the course for which the exam is taken.
        /// </summary>
        public Course Course { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Exam"/> class.
        /// </summary>
        public Exam()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Exam"/> class with specified parameters.
        /// </summary>
        /// <param name="examId">The ID of the exam.</param>
        /// <param name="studentId">The ID of the student taking the exam.</param>
        /// <param name="semesterId">The ID of the semester in which the exam is taken.</param>
        /// <param name="courseId">The ID of the course for which the exam is taken.</param>
        /// <param name="grade">The grade of the exam.</param>
        /// <param name="examDate">The date of the exam.</param>
        public Exam(int examId, int studentId, int semesterId, int courseId, int grade, DateTime examDate)
        {
            this.ExamId = examId;
            this.StudentId = studentId;
            this.SemesterId = semesterId;
            this.CourseId = courseId;
            this.Grade = grade;
            this.ExamDate = examDate;

            this.Validate();
        }

        /// <summary>
        /// Checks if the exam is passed.
        /// </summary>
        /// <returns>True if the grade is 5 or higher, otherwise false.</returns>
        public bool IsPassed()
        {
            return this.Grade >= 5;
        }

        /// <summary>
        /// Validates the exam properties.
        /// </summary>
        public void Validate()
        {
            var context = new ValidationContext(this, null, null);
            var results = new List<ValidationResult>();

            if (!Validator.TryValidateObject(this, context, results, true))
            {
                throw new ValidationException("Exam validation failed.");
            }

            if (this.Student == null)
            {
                throw new ValidationException("Student is required.");
            }

            if (this.Semester == null)
            {
                throw new ValidationException("Semester is required.");
            }

            if (this.Course == null)
            {
                throw new ValidationException("Course is required.");
            }
        }
    }
}