namespace DomainModel.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    // Model for Student
    public class Student
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [StringLength(200)]
        public string Address { get; set; }

        [Required]
        [RegularExpression("^[0-9]{13}$", ErrorMessage = "The national identification number must contain exactly 13 digits.")]
        public string NationalId { get; set; }

        [Required]
        [StringLength(20)]
        public string RegistrationNumber { get; set; }

        public ICollection<string> Phones { get; set; } = new List<string>();
        public ICollection<string> Emails { get; set; } = new List<string>();

        public bool HasValidContact() => Phones.Count > 0 || Emails.Count > 0;

        public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }
}
