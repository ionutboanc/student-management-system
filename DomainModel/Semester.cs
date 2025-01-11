namespace DomainModel.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    // Model for Semester
    public class Semester
    {
        public int Id { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "The semester number must be greater than 0.")]
        public int Number { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "The credit threshold must be non-negative.")]
        public int CreditThreshold { get; set; }

        public virtual List<Course> AvailableCourses { get; set; } = new List<Course>();
    }
}
