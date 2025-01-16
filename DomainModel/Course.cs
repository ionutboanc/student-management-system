namespace DomainModel.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Model for Course.
    /// </summary>
    public class Course
    {
        /// <summary>
        /// Gets or sets the ID of the course.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the course.
        /// </summary>
        [Required(ErrorMessage = "Course name is required.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Course name must be between 3 and 100 characters.")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description of the course.
        /// </summary>
        [StringLength(255, MinimumLength = 5, ErrorMessage = "Course description must be between 5 and 255 characters.")]
        public string Description { get; set; } // Nullable property

        /// <summary>
        /// Gets or sets the number of credits for the course.
        /// </summary>
        [Required(ErrorMessage = "Number of credits is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "The number of credits must be at least 1.")]
        public int Credits { get; set; }

        /// <summary>
        /// Gets or sets the cost of the course.
        /// </summary>
        [Required(ErrorMessage = "Cost is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "The cost must be positive.")]
        public decimal Cost { get; set; }

        /// <summary>
        /// Gets or sets the discount ID for the course.
        /// </summary>
        public int? DiscountId { get; set; }

        /// <summary>
        /// Gets or sets the discount for the course.
        /// </summary>
        public virtual Discount Discount { get; set; }

        /// <summary>
        /// Gets or sets the minimum threshold for the course cost.
        /// </summary>
        [Required(ErrorMessage = "Minimum threshold is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Minimum threshold must be non-negative.")]
        public decimal MinThreshold { get; set; }

        /// <summary>
        /// Gets or sets the maximum threshold for the course cost.
        /// </summary>
        [Required(ErrorMessage = "Maximum threshold is required.")]
        [Range(1, double.MaxValue, ErrorMessage = "Maximum threshold must be positive.")]
        public decimal MaxThreshold { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the balance is refunded.
        /// </summary>
        [Required(ErrorMessage = "Balance refunded status is required.")]
        public bool BalanceRefunded { get; set; } = false;

        /// <summary>
        /// Gets or sets the list of students enrolled in the course.
        /// </summary>
        public List<Student> Students { get; set; } = new List<Student>();

        /// <summary>
        /// Gets or sets the list of prerequisite courses.
        /// </summary>
        public List<Course> Prerequisites { get; set; } = new List<Course>();

        /// <summary>
        /// Gets or sets the list of available semesters for the course.
        /// </summary>
        public virtual List<Semester> AvailableSemesters { get; set; } = new List<Semester>();

        /// <summary>
        /// Gets or sets the list of exams for the course.
        /// </summary>
        public List<Exam> Exams { get; set; } = new List<Exam>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Course"/> class.
        /// </summary>
        public Course()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Course"/> class with specified parameters.
        /// </summary>
        /// <param name="id">The ID of the course.</param>
        /// <param name="name">The name of the course.</param>
        /// <param name="description">The description of the course.</param>
        /// <param name="credits">The number of credits for the course.</param>
        /// <param name="cost">The cost of the course.</param>
        /// <param name="minThreshold">The minimum threshold for the course cost.</param>
        /// <param name="maxThreshold">The maximum threshold for the course cost.</param>
        public Course(int id, string name, string description, int credits, decimal cost, decimal minThreshold, decimal maxThreshold)
        {
            this.Id = id;
            this.Name = name;
            this.Description = description;
            this.Credits = credits;
            this.Cost = cost;
            this.MinThreshold = minThreshold;
            this.MaxThreshold = maxThreshold;

            this.Validate();
        }

        /// <summary>
        /// Checks if the cost of the course is valid.
        /// </summary>
        /// <returns>True if the cost is valid, otherwise false.</returns>
        public bool IsCostValid()
        {
            return this.Cost >= this.MinThreshold * this.Credits && this.Cost <= this.MaxThreshold * this.Credits;
        }

        /// <summary>
        /// Gets the cost of the course after applying any discounts and refund factors.
        /// </summary>
        /// <returns>The calculated cost of the course.</returns>
        public decimal GetCost()
        {
            decimal refundFactor = 1m;
            if (this.BalanceRefunded)
            {
                refundFactor = 0.5m;
            }
            if (this.Discount != null)
            {
                return (this.Cost - (this.Cost * this.Discount.Percentage / 100)) * refundFactor;
            }
            return this.Cost * refundFactor;
        }

        /// <summary>
        /// Validates the course properties.
        /// </summary>
        public void Validate()
        {
            var context = new ValidationContext(this, null, null);
            var results = new List<ValidationResult>();

            if (!Validator.TryValidateObject(this, context, results, true))
            {
                throw new ValidationException("Course validation failed.");
            }

            if (this.MinThreshold > this.MaxThreshold)
            {
                throw new ValidationException("MinThreshold cannot be greater than MaxThreshold.");
            }

            if (!this.IsCostValid())
            {
                throw new ValidationException("Cost is not within the valid range.");
            }
        }
    }
}