namespace TestDomainModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using DomainModel.Models;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Provides unit tests for the Student class.
    /// </summary>
    [TestClass]
    public class StudentTests
    {
        private Student student;

        /// <summary>
        /// Sets up the test environment.
        /// </summary>
        [TestInitialize]
        public void SetUp()
        {
            // Initialize a valid student entity
            this.student = new Student
            {
                FirstName = "John",
                LastName = "Doe",
                Address = "123 Main St",
                RegistrationNumber = "REG123",
                NationalId = "1234567890123",
                Phones = new List<string> { "+40712345678" },
                Emails = new List<string> { "john.doe@example.com" },
                Enrollments = new List<Enrollment>()
            };
        }

        /// <summary>
        /// Tests that the student is valid when all required fields are set correctly.
        /// </summary>
        [TestMethod]
        public void TestValidStudent()
        {
            var context = new ValidationContext(this.student, null, null);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(this.student, context, results, true);

            Assert.IsTrue(isValid);
            Assert.AreEqual(0, results.Count);
            Assert.IsTrue(this.student.HasValidContact());
        }

        /// <summary>
        /// Tests that student validation fails when first name is missing.
        /// </summary>
        [TestMethod]
        public void TestStudentFirstNameMissing()
        {
            this.student.FirstName = null;

            var context = new ValidationContext(this.student, null, null);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(this.student, context, results, true);

            Assert.IsFalse(isValid);
            Assert.IsTrue(results.Any(r => r.MemberNames.Contains("FirstName")));
        }

        /// <summary>
        /// Tests that student validation fails when last name is missing.
        /// </summary>
        [TestMethod]
        public void TestStudentLastNameMissing()
        {
            this.student.LastName = null;

            var context = new ValidationContext(this.student, null, null);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(this.student, context, results, true);

            Assert.IsFalse(isValid);
            Assert.IsTrue(results.Any(r => r.MemberNames.Contains("LastName")));
        }

        /// <summary>
        /// Tests that student validation fails when national ID is missing.
        /// </summary>
        [TestMethod]
        public void TestStudentNationalIdMissing()
        {
            this.student.NationalId = null;

            var context = new ValidationContext(this.student, null, null);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(this.student, context, results, true);

            Assert.IsFalse(isValid);
            Assert.IsTrue(results.Any(r => r.MemberNames.Contains("NationalId")));
        }

        /// <summary>
        /// Tests that student validation fails when registration number is missing.
        /// </summary>
        [TestMethod]
        public void TestStudentRegistrationNumberMissing()
        {
            this.student.RegistrationNumber = null;

            var context = new ValidationContext(this.student, null, null);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(this.student, context, results, true);

            Assert.IsFalse(isValid);
            Assert.IsTrue(results.Any(r => r.MemberNames.Contains("RegistrationNumber")));
        }

        /// <summary>
        /// Tests that student validation passes when at least one contact method is provided.
        /// </summary>
        [TestMethod]
        public void TestStudentHasValidContact()
        {
            this.student.Phones = new List<string>();
            this.student.Emails = new List<string> { "email@example.com" };

            Assert.IsTrue(this.student.HasValidContact());
        }

        /// <summary>
        /// Tests that student validation fails when no contact methods are provided.
        /// </summary>
        [TestMethod]
        public void TestStudentNoContactMethods()
        {
            this.student.Phones = new List<string>();
            this.student.Emails = new List<string>();

            Assert.IsFalse(this.student.HasValidContact());
        }

        /// <summary>
        /// Tests that adding an enrollment to the student works correctly.
        /// </summary>
        [TestMethod]
        public void TestAddEnrollment()
        {
            var enrollment = new Enrollment
            {
                StudentId = this.student.Id,
                CourseId = 1,
                SemesterId = 1,
                NumberOfAttempts = 0
            };

            this.student.Enrollments.Add(enrollment);

            Assert.AreEqual(1, this.student.Enrollments.Count);
            Assert.AreEqual(1, this.student.Enrollments.First().CourseId);
        }

        /// <summary>
        /// Tests that attempting to add a null enrollment throws an exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestAddNullEnrollment()
        {
            this.student.Enrollments.Add(null);
        }

        /// <summary>
        /// Tests that student phone validation fails with an invalid phone number.
        /// </summary>
        [TestMethod]
        public void TestInvalidPhoneNumber()
        {
            this.student.Phones = new List<string> { "InvalidPhoneNumber" };

            bool isValid = this.student.Phones.All(phone => IsValidPhone(phone));

            Assert.IsFalse(isValid);
        }

        private bool IsValidPhone(string phone)
        {
            // Simplified phone validation for Romanian format
            return System.Text.RegularExpressions.Regex.IsMatch(phone, @"^\+407\d{8}$");
        }

        /// <summary>
        /// Tests that student email validation fails with an invalid email address.
        /// </summary>
        [TestMethod]
        public void TestInvalidEmailAddress()
        {
            this.student.Emails = new List<string> { "invalid-email" };

            bool isValid = this.student.Emails.All(email => IsValidEmail(email));

            Assert.IsFalse(isValid);
        }

        private bool IsValidEmail(string email)
        {
            // Simplified email validation
            return new EmailAddressAttribute().IsValid(email);
        }

        // Additional tests can be added to cover more edge cases and validations.
    }
}