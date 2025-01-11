namespace DomainModel.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    // Model for Course
    public class Course
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Name { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "The number of credits must be at least 1.")]
        public int Credits { get; set; }


        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "The cost must be positive.")]
        public decimal Cost { get; set; }

        public int? DiscountId { get; set; }

        public virtual Discount Discount { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal MinThreshold { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal MaxThreshold { get; set; }

        public bool IsCostValid() => Cost >= MinThreshold * Credits && Cost <= MaxThreshold * Credits;

        public virtual List<Prerequisite> Prerequisites { get; set; } = new List<Prerequisite>();
        public virtual List<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
        public virtual List<Semester> AvailableSemesters { get; set; } = new List<Semester>();

        public decimal GetCost()
        {
            if (Discount != null)
            {
                return Cost - (Cost * Discount.DiscountPercentage / 100);
            }
            return Cost;
        }
    }
}
