namespace DomainModel.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    // Model for Enrollment
    public class Enrollment
    {
        public int Id { get; set; }

        public int StudentId { get; set; }
        public virtual Student Student { get; set; }

        public int CourseId { get; set; }
        public virtual Course Course { get; set; }

        public int SemesterId { get; set; }
        public virtual Semester Semester { get; set; }

        [Range(1, Constants.MaxNumberOfAttempts, ErrorMessage = "The number of attempts must be between 1 and " + nameof(Constants.MaxNumberOfAttempts))]
        public int NumberOfAttempts { get; set; }

        public DateTime? LastAttemptDate { get; set; }

        [Range(1, 10, ErrorMessage = "The grade must be between 1 and 10.")]
        public int Grade { get; set; }
    }
}
