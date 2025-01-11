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
        [StringLength(255)]
        public string Address { get; set; }

        [Required]
        [RegularExpression("^[0-9]{13}$", ErrorMessage = "The national identification number must contain exactly 13 digits.")]
        public string NationalId { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "The balance must be positive.")]
        public decimal Balance { get; set; } = 0;



        [Required]
        [StringLength(20)]
        public string RegistrationNumber { get; set; }
        public List<string> Phones { get; set; } = new List<string>();
        public List<string> Emails { get; set; } = new List<string>();

        public bool HasValidContact() => Phones.Count > 0 || Emails.Count > 0;

        public virtual List<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

        // Basic balance-related methods
        public void DeductBalance(decimal amount)
        {
            if (amount < 0)
                throw new ArgumentOutOfRangeException(nameof(amount), "Amount to deduct cannot be negative.");

            if (Balance < amount)
                throw new InvalidOperationException("Insufficient balance.");

            Balance -= amount;
        }

        public void AddBalance(decimal amount)
        {
            if (amount < 0)
                throw new ArgumentOutOfRangeException(nameof(amount), "Amount to add cannot be negative.");

            Balance += amount;
        }

        public void AddEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email cannot be null or empty.", nameof(email));

            var emailAttribute = new EmailAddressAttribute();
            if (!emailAttribute.IsValid(email))
                throw new ArgumentException("Invalid email format.", nameof(email));

            Emails.Add(email);
        }
        public void AddPhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                throw new ArgumentException("Phone number cannot be null or empty.", nameof(phone));

            if (!System.Text.RegularExpressions.Regex.IsMatch(phone, @"^\d{4}\s?\d{6}$"))
                throw new ArgumentException("Invalid phone number format. Expected format: 1234567890 or 1234 567890", nameof(phone));

            Phones.Add(phone);
        }
    }
}
