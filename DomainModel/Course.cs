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

        [Range(1, int.MaxValue, ErrorMessage = "The number of credits must be at least 1.")]
        public int Credits { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "The cost must be positive.")]
        public decimal Cost { get; set; }

        [Range(0, double.MaxValue)]
        public decimal MinThreshold { get; set; }

        [Range(0, double.MaxValue)]
        public decimal MaxThreshold { get; set; }

        public bool IsCostValid() => Cost >= MinThreshold * Credits && Cost <= MaxThreshold * Credits;

        public virtual ICollection<Prerequisite> Prerequisites { get; set; } = new List<Prerequisite>();
        public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
        public virtual ICollection<Semester> AvailableSemesters { get; set; } = new List<Semester>();
    }
}
