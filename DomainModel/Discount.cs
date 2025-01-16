// <copyright file="{fileName}" company="Transilvania University of Brasov">
// Copyright (c) 2025 Boanc Ionut-Silviu. All rights reserved.
// </copyright>

namespace DomainModel.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    /// <summary>
    /// Model for Discount.
    /// </summary>
    public class Discount
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Discount"/> class.
        /// </summary>
        public Discount()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Discount"/> class with the specified courses and percentage.
        /// </summary>
        /// <param name="courses">The list of courses associated with the discount.</param>
        /// <param name="percentage">The percentage of the discount.</param>
        public Discount(List<Course> courses, decimal percentage)
        {
            this.Courses = courses;
            this.Percentage = percentage;
            this.Validate();
        }

        /// <summary>
        /// Gets or sets the ID of the discount.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the percentage of the discount.
        /// </summary>
        [Required(ErrorMessage = "The percentage is required.")]
        [Range(0, 100, ErrorMessage = "The percentage must be between 0 and 100.")]
        public decimal Percentage { get; set; } = 0;

        /// <summary>
        /// Gets or sets the list of courses associated with the discount.
        /// </summary>
        public virtual List<Course> Courses { get; set; } = new List<Course>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Discount"/> class.
        /// </summary>

        /// <summary>
        /// Validates the discount properties.
        /// </summary>
        public void Validate()
        {
            var context = new ValidationContext(this, null, null);
            var results = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(this, context, results, true);
            if (!isValid)
            {
                throw new ValidationException("The discount is not valid.");
            }

            if (this.Courses == null || !this.Courses.Any())
            {
                throw new ValidationException("The list of courses cannot be empty.");
            }
        }
    }
}