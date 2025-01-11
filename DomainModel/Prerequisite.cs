namespace DomainModel.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    // Model for Prerequisite
    public class Prerequisite
    {
        public int Id { get; set; }

        public int CourseId { get; set; }
        
        public virtual Course Course { get; set; }

        public int PrerequisiteCourseId { get; set; }
        public virtual Course PrerequisiteCourse { get; set; }

        [Range(1, 10, ErrorMessage = "The minimum grade must be between 5.")]
        public int MinimumGrade { get; set; } = 5;
    }
}
