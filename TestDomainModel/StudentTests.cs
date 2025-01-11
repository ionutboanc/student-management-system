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
                FirstName = "Boanc",
                LastName = "Ionut",
                Address = "Str. Arenei",
                RegistrationNumber = "123456",
                NationalId = "1234567890123",
                Phones = new List<string> { "0712345678" },
                Emails = new List<string> { "boanc.ionut@example.com" },
                Balance = 200m,
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
        /// Tests that student phone validation fails with an invalid phone number.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestInvalidPhoneNumber()
        {
            this.student.AddPhone("03123456789");
        }

        /// <summary>
        /// Tests that student phone validation passes with a valid phone number.
        /// </summary>
        [TestMethod]
        public void TestValidPhoneNumber()
        {
            this.student.AddPhone("0712345678");

            Assert.IsTrue(this.student.Phones.Contains("0712345678"));
        }

        /// <summary>
        /// Tests that student phone validation fails with a null phone number.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestNullPhoneNumber()
        {
            this.student.AddPhone(null);
        }

        /// <summary>
        /// Tests that student phone validation fails with an empty phone number.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestEmptyPhoneNumber()
        {
            this.student.AddPhone(string.Empty);
        }

        /// <summary>
        /// Tests that student phone validation fails with a whitespace phone number.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestWhitespacePhoneNumber()
        {
            this.student.AddPhone("   ");
        }

        /// <summary>
        /// Tests that student email validation fails with an invalid email address.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestInvalidEmailAddress()
        {
            this.student.AddEmail("invalid-email");
        }

        /// <summary>
        /// Tests that student email validation passes with a valid email address.
        /// </summary>
        [TestMethod]
        public void TestValidEmailAddress()
        {
            this.student.AddEmail("ionut.boanc@example.com");

            Assert.IsTrue(this.student.Emails.Contains("ionut.boanc@example.com"));
        }

        /// <summary>
        /// Tests that student email validation fails with a null email address.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestNullEmailAddress()
        {
            this.student.AddEmail(null);
        }

        /// <summary>
        /// Tests that student email validation fails with an empty email address.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestEmptyEmailAddress()
        {
            this.student.AddEmail(string.Empty);
        }

        /// <summary>
        /// Tests that student email validation fails with a whitespace email address.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestWhitespaceEmailAddress()
        {
            this.student.AddEmail("   ");
        }

        /// <summary>
        /// Tests that the balance is deducted correctly.
        /// </summary>
        [TestMethod]
        public void TestDeductBalance()
        {
            // Arrange
            decimal initialBalance = this.student.Balance;
            decimal amountToDeduct = 200m;

            // Act
            this.student.DeductBalance(amountToDeduct);

            // Assert
            Assert.AreEqual(initialBalance - amountToDeduct, this.student.Balance);
        }

        /// <summary>
        /// Tests that the balance is added correctly.
        /// </summary>
        [TestMethod]
        public void TestAddBalance()
        {
            // Arrange
            decimal initialBalance = this.student.Balance;
            decimal amountToAdd = 300m;

            // Act
            this.student.AddBalance(amountToAdd);

            // Assert
            Assert.AreEqual(initialBalance + amountToAdd, this.student.Balance);
        }

        /// <summary>
        /// Tests that the balance cannot be deducted below zero.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestDeductBalanceBelowZero()
        {
            // Arrange
            decimal amountToDeduct = this.student.Balance + 1;

            // Act
            this.student.DeductBalance(amountToDeduct);
        }
    }
}