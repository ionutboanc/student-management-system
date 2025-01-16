// <copyright file="{fileName}" company="Transilvania University of Brasov">
// Copyright (c) 2025 Boanc Ionut-Silviu. All rights reserved.
// </copyright>

namespace DomainModel.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    /// <summary>
    /// Model for Student.
    /// </summary>
    public class Student
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Student"/> class.
        /// </summary>
        public Student()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Student"/> class with specified parameters.
        /// </summary>
        /// <param name="firstName">The first name of the student.</param>
        /// <param name="lastName">The last name of the student.</param>
        /// <param name="address">The address of the student.</param>
        /// <param name="nationalId">The national ID of the student.</param>
        /// <param name="balance">The balance of the student.</param>
        /// <param name="registrationNumber">The registration number of the student.</param>
        /// <param name="currentSemesterId">The current semester ID of the student.</param>
        public Student(string firstName, string lastName, string address, string nationalId, decimal balance, string registrationNumber, int? currentSemesterId = null)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Address = address;
            this.NationalId = nationalId;
            this.Balance = balance;
            this.RegistrationNumber = registrationNumber;
            this.CurrentSemesterId = currentSemesterId;

            this.Validate();
        }

        /// <summary>
        /// Gets or sets the ID of the student.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the first name of the student.
        /// </summary>
        [Required(ErrorMessage = "First name is required.")]
        [StringLength(40, MinimumLength = 3, ErrorMessage = "The first name must be between 3 and 40 chars.")]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name of the student.
        /// </summary>
        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(40, MinimumLength = 3, ErrorMessage = "The last name must be between 3 and 40 chars.")]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the address of the student.
        /// </summary>
        [Required(ErrorMessage = "Address is required.")]
        [StringLength(255, MinimumLength = 10, ErrorMessage = "The address must be between 10 and 255 characters.")]
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets the national ID of the student.
        /// </summary>
        [Required(ErrorMessage = "National ID is required.")]
        [RegularExpression("^[0-9]{13}$", ErrorMessage = "The national ID number must contain exactly 13 digits.")]
        public string NationalId { get; set; }

        /// <summary>
        /// Gets or sets the balance of the student.
        /// </summary>
        [Required(ErrorMessage = "Balance is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "The balance must be positive.")]
        public decimal Balance { get; set; } = 0;

        /// <summary>
        /// Gets or sets the registration number of the student.
        /// </summary>
        [Required(ErrorMessage = "Registration number is required.")]
        [StringLength(20, ErrorMessage = "The registration number must be up to 20 characters.")]
        public string RegistrationNumber { get; set; }

        /// <summary>
        /// Gets or sets the list of phone numbers of the student.
        /// </summary>
        public IList<string> Phones { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets the list of email addresses of the student.
        /// </summary>
        public IList<string> Emails { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets the current semester ID of the student.
        /// </summary>
        public int? CurrentSemesterId { get; set; }

        /// <summary>
        /// Gets or sets the current semester of the student.
        /// </summary>
        public Semester CurrentSemester { get; set; }

        /// <summary>
        /// Gets or sets the list of exams taken by the student.
        /// </summary>
        public IList<Exam> Exams { get; set; } = new List<Exam>();

        /// <summary>
        /// Gets or sets the list of courses the student is enrolled in.
        /// </summary>
        public IList<Course> EnrolledCourses { get; set; } = new List<Course>();

        /// <summary>
        /// Checks if the student has at least one valid contact method (phone or email).
        /// </summary>
        /// <returns>True if the student has at least one valid contact method, otherwise false.</returns>
        public bool HasValidContact() => this.Phones.Count > 0 || this.Emails.Count > 0;

        /// <summary>
        /// Deducts an amount from the student's balance.
        /// </summary>
        /// <param name="amount">The amount to deduct.</param>
        public void DeductBalance(decimal amount)
        {
            if (amount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(amount), "Amount to deduct cannot be negative.");
            }

            if (this.Balance < amount)
            {
                throw new InvalidOperationException("Insufficient balance.");
            }

            this.Balance -= amount;
        }

        /// <summary>
        /// Adds an amount to the student's balance.
        /// </summary>
        /// <param name="amount">The amount to add.</param>
        public void AddBalance(decimal amount)
        {
            if (amount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(amount), "Amount to add cannot be negative.");
            }

            this.Balance += amount;
        }

        /// <summary>
        /// Adds an email to the student's list of emails.
        /// </summary>
        /// <param name="email">The email to add.</param>
        public void AddEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException("Email cannot be null or empty.", nameof(email));
            }

            var emailAttribute = new EmailAddressAttribute();
            if (!emailAttribute.IsValid(email))
            {
                throw new ArgumentException("Invalid email format.", nameof(email));
            }

            this.Emails.Add(email);
        }

        /// <summary>
        /// Adds a phone number to the student's list of phone numbers.
        /// </summary>
        /// <param name="phone">The phone number to add.</param>
        public void AddPhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
            {
                throw new ArgumentException("Phone number cannot be null or empty.", nameof(phone));
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(phone, @"^\d{4}\s?\d{6}$"))
            {
                throw new ArgumentException("Invalid phone number format. Expected format: 1234567890 or 1234 567890", nameof(phone));
            }

            this.Phones.Add(phone);
        }

        /// <summary>
        /// Removes an email from the student's list of emails.
        /// </summary>
        /// <param name="email">The email to remove.</param>
        public void RemoveEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException("Email cannot be null or empty.", nameof(email));
            }

            if (!this.Emails.Contains(email))
            {
                throw new ArgumentException("Email not found.", nameof(email));
            }

            this.Emails.Remove(email);
        }

        /// <summary>
        /// Removes a phone number from the student's list of phone numbers.
        /// </summary>
        /// <param name="phone">The phone number to remove.</param>
        public void RemovePhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
            {
                throw new ArgumentException("Phone number cannot be null or empty.", nameof(phone));
            }

            if (!this.Phones.Contains(phone))
            {
                throw new ArgumentException("Phone number not found.", nameof(phone));
            }

            this.Phones.Remove(phone);
        }

        /// <summary>
        /// Validates the student properties.
        /// </summary>
        public void Validate()
        {
            var context = new ValidationContext(this, null, null);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(this, context, results, true);

            if (!isValid)
            {
                throw new ValidationException("Student validation failed: " + string.Join(", ", results.Select(r => r.ErrorMessage)));
            }
        }
    }
}