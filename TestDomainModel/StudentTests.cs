// <copyright file="{fileName}" company="Transilvania University of Brasov">
// Copyright (c) 2025 Boanc Ionut-Silviu. All rights reserved.
// </copyright>

namespace TestDomainModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using DomainModel.Models;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Contains unit tests for the <see cref="Student"/> class.
    /// </summary>
    [TestClass]
    public class StudentTests
    {
        /// <summary>
        /// The student instance used for testing.
        /// </summary>
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
                Balance = 200m,
            };

            this.student.AddPhone("0712345678");
            this.student.AddEmail("boanc.ionut@example.com");
        }

        /// <summary>
        /// Tests if a valid student passes validation.
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
        /// Tests if a student with a missing first name fails validation.
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
        /// Tests if a student with a missing last name fails validation.
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
        /// Tests if a student with a missing national ID fails validation.
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
        /// Tests if a student with a missing registration number fails validation.
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
        /// Tests if a student has at least one valid contact method.
        /// </summary>
        [TestMethod]
        public void TestStudentHasValidContact()
        {
            this.student.Phones = new List<string>();
            this.student.Emails = new List<string> { "ionut@mail.com" };

            Assert.IsTrue(this.student.HasValidContact());
        }

        /// <summary>
        /// Tests if a student with no contact methods fails validation.
        /// </summary>
        [TestMethod]
        public void TestStudentNoContactMethods()
        {
            this.student.Phones = new List<string>();
            this.student.Emails = new List<string>();

            Assert.IsFalse(this.student.HasValidContact());
        }

        /// <summary>
        /// Tests adding an invalid phone number.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestInvalidPhoneNumber()
        {
            this.student.AddPhone("InvalidPhoneNumber");
        }

        /// <summary>
        /// Tests adding a valid phone number.
        /// </summary>
        [TestMethod]
        public void TestValidPhoneNumber()
        {
            this.student.AddPhone("0712345678");

            Assert.IsTrue(this.student.Phones.Contains("0712345678"));
        }

        /// <summary>
        /// Tests adding a null phone number.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestNullPhoneNumber()
        {
            this.student.AddPhone(null);
        }

        /// <summary>
        /// Tests adding an empty phone number.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestEmptyPhoneNumber()
        {
            this.student.AddPhone(string.Empty);
        }

        /// <summary>
        /// Tests adding a whitespace phone number.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestWhitespacePhoneNumber()
        {
            this.student.AddPhone("   ");
        }

        /// <summary>
        /// Tests adding an invalid email address.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestInvalidEmailAddress()
        {
            this.student.AddEmail("invalid-email");
        }

        /// <summary>
        /// Tests adding a valid email address.
        /// </summary>
        [TestMethod]
        public void TestValidEmailAddress()
        {
            this.student.AddEmail("ionut.boanc@example.com");

            Assert.IsTrue(this.student.Emails.Contains("ionut.boanc@example.com"));
        }

        /// <summary>
        /// Tests adding a null email address.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestNullEmailAddress()
        {
            this.student.AddEmail(null);
        }

        /// <summary>
        /// Tests adding an empty email address.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestEmptyEmailAddress()
        {
            this.student.AddEmail(string.Empty);
        }

        /// <summary>
        /// Tests adding a whitespace email address.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestWhitespaceEmailAddress()
        {
            this.student.AddEmail("   ");
        }

        /// <summary>
        /// Tests deducting balance from the student.
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
        /// Tests adding balance to the student.
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
        /// Tests adding a negative balance to the student.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestAddBalanceNegative()
        {
            // Arrange
            decimal initialBalance = this.student.Balance;
            decimal amountToAdd = -300m;

            // Act
            this.student.AddBalance(amountToAdd);
        }

        /// <summary>
        /// Tests deducting balance below zero.
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

        /// <summary>
        /// Tests deducting a negative balance from the student.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestDeductBalanceNegative()
        {
            // Arrange
            decimal amountToDeduct = -100;

            // Act
            this.student.DeductBalance(amountToDeduct);
        }

        /// <summary>
        /// Tests the parameterized constructor with a valid student.
        /// </summary>
        [TestMethod]
        public void TestParameterizedConstructor_ValidStudent()
        {
            // Arrange
            var student = new Student("Test", "Student", "Str. Testeen", "1234567890123", 100m, "123456");
            student.AddPhone("1234567890");

            // Act
            var context = new ValidationContext(student, null, null);
            var results = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(student, context, results, true);

            // Assert
            Assert.IsTrue(isValid);
            Assert.AreEqual(0, results.Count);
            Assert.AreEqual("Test", student.FirstName);
            Assert.AreEqual("Student", student.LastName);
            Assert.AreEqual("Str. Testeen", student.Address);
            Assert.AreEqual("1234567890123", student.NationalId);
            Assert.AreEqual(100m, student.Balance);
            Assert.AreEqual("123456", student.RegistrationNumber);
        }

        /// <summary>
        /// Tests the parameterized constructor with an invalid student.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void TestParameterizedConstructor_InvalidStudent()
        {
            // Act
            var student = new Student(null, "Ionut", "Str. Arenei", "1234567890123", 200m, "123456");
        }

        /// <summary>
        /// Tests adding a valid phone number to the student.
        /// </summary>
        [TestMethod]
        public void TestAddPhone_ValidPhone()
        {
            // Arrange
            var phone = "1234 567890";

            // Act
            this.student.AddPhone(phone);

            // Assert
            Assert.IsTrue(this.student.Phones.Contains(phone));
        }

        /// <summary>
        /// Tests adding an invalid phone number to the student.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestAddPhone_InvalidPhone()
        {
            // Act
            this.student.AddPhone("InvalidPhone");
        }

        /// <summary>
        /// Tests removing a valid phone number from the student.
        /// </summary>
        [TestMethod]
        public void TestRemovePhone_ValidPhone()
        {
            // Arrange
            var phone = "0712345677";
            this.student.AddPhone(phone);

            // Act
            this.student.RemovePhone(phone);

            // Assert
            Assert.IsFalse(this.student.Phones.Contains(phone));
        }

        /// <summary>
        /// Tests removing an invalid phone number from the student.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestRemovePhone_InvalidPhone()
        {
            // Act
            this.student.RemovePhone("InvalidPhone");
        }

        /// <summary>
        /// Tests adding a valid email address to the student.
        /// </summary>
        [TestMethod]
        public void TestAddEmail_ValidEmail()
        {
            // Arrange
            var email = "ionut.boanc@example.com";

            // Act
            this.student.AddEmail(email);

            // Assert
            Assert.IsTrue(this.student.Emails.Contains(email));
        }

        /// <summary>
        /// Tests adding an invalid email address to the student.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestAddEmail_InvalidEmail()
        {
            // Act
            this.student.AddEmail("InvalidEmail");
        }

        /// <summary>
        /// Tests removing a valid email address from the student.
        /// </summary>
        [TestMethod]
        public void TestRemoveEmail_ValidEmail()
        {
            // Arrange
            var email = "boanc.ionut2@example.com";
            this.student.AddEmail(email);

            // Act
            this.student.RemoveEmail(email);

            // Assert
            Assert.IsFalse(this.student.Emails.Contains(email));
        }

        /// <summary>
        /// Tests removing an invalid email address from the student.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestRemoveEmail_InvalidEmail()
        {
            // Act
            this.student.RemoveEmail("InvalidEmail");
        }
    }
}