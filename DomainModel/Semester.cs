namespace DomainModel.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    /// <summary>
    /// Model for Semester.
    /// </summary>
    public class Semester
    {
        /// <summary>
        /// Gets or sets the ID of the semester.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the number of the semester.
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "The semester number must be greater than 0.")]
        public int Number { get; set; }

        /// <summary>
        /// Gets or sets the credit threshold for the semester.
        /// </summary>
        [Range(0, int.MaxValue, ErrorMessage = "The credit threshold must be non-negative.")]
        public int CreditThreshold { get; set; }

        /// <summary>
        /// Gets or sets the list of available courses for the semester.
        /// </summary>
        public virtual List<Course> AvailableCourses { get; set; } = new List<Course>();

        /// <summary>
        /// Gets or sets the list of students in the semester.
        /// </summary>
        public virtual List<Student> Students { get; set; } = new List<Student>();

        /// <summary>
        /// Gets or sets the list of exams in the semester.
        /// </summary>
        public virtual List<Exam> Exams { get; set; } = new List<Exam>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Semester"/> class.
        /// </summary>
        public Semester()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Semester"/> class with specified parameters.
        /// </summary>
        /// <param name="id">The ID of the semester.</param>
        /// <param name="number">The number of the semester.</param>
        /// <param name="creditThreshold">The credit threshold for the semester.</param>
        public Semester(int id, int number, int creditThreshold)
        {
            this.Id = id;
            this.Number = number;
            this.CreditThreshold = creditThreshold;
        }

        /// <summary>
        /// Validates the semester properties.
        /// </summary>
        public void Validate()
        {
            var context = new ValidationContext(this, null, null);
            var results = new List<ValidationResult>();
            if (!Validator.TryValidateObject(this, context, results, true))
            {
                throw new ValidationException("Semester validation failed.");
            }

            if (this.AvailableCourses == null || this.AvailableCourses.Count == 0)
            {
                throw new ValidationException("Semester must have at least one available course.");
            }

            if (this.Students == null || this.Students.Count == 0)
            {
                throw new ValidationException("Semester must have at least one student.");
            }

            int totalCredits = this.AvailableCourses.Sum(course => course.Credits);
            if (totalCredits < this.CreditThreshold)
            {
                throw new ValidationException($"The total credits of the available courses ({totalCredits}) must be equal to or more than the credit threshold ({this.CreditThreshold}).");
            }
        }
    }
}