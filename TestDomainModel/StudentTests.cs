using DomainModel.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;
using System.Linq;

namespace TestDomainModel
{
    [TestClass]
    public class StudentTests
    {
        private Student student;

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

        [TestMethod]
        public void TestStudentHasValidContact()
        {
            this.student.Phones = new List<string>();
            this.student.Emails = new List<string> { "ionut@mail.com" };

            Assert.IsTrue(this.student.HasValidContact());
        }

        [TestMethod]
        public void TestStudentNoContactMethods()
        {
            this.student.Phones = new List<string>();
            this.student.Emails = new List<string>();

            Assert.IsFalse(this.student.HasValidContact());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestInvalidPhoneNumber()
        {
            this.student.AddPhone("InvalidPhoneNumber");
        }

        [TestMethod]
        public void TestValidPhoneNumber()
        {
            this.student.AddPhone("0712345678");

            Assert.IsTrue(this.student.Phones.Contains("0712345678"));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestNullPhoneNumber()
        {
            this.student.AddPhone(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestEmptyPhoneNumber()
        {
            this.student.AddPhone(string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestWhitespacePhoneNumber()
        {
            this.student.AddPhone("   ");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestInvalidEmailAddress()
        {
            this.student.AddEmail("invalid-email");
        }

        [TestMethod]
        public void TestValidEmailAddress()
        {
            this.student.AddEmail("ionut.boanc@example.com");

            Assert.IsTrue(this.student.Emails.Contains("ionut.boanc@example.com"));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestNullEmailAddress()
        {
            this.student.AddEmail(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestEmptyEmailAddress()
        {
            this.student.AddEmail(string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestWhitespaceEmailAddress()
        {
            this.student.AddEmail("   ");
        }

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

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestDeductBalanceBelowZero()
        {
            // Arrange
            decimal amountToDeduct = this.student.Balance + 1;

            // Act
            this.student.DeductBalance(amountToDeduct);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestDeductBalanceNegative()
        {
            // Arrange
            decimal amountToDeduct = -100;

            // Act
            this.student.DeductBalance(amountToDeduct);
        }

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

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void TestParameterizedConstructor_InvalidStudent()
        {
            // Act
            var student = new Student(null, "Ionut", "Str. Arenei", "1234567890123", 200m, "123456");
        }

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

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestAddPhone_InvalidPhone()
        {
            // Act
            this.student.AddPhone("InvalidPhone");
        }

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

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestRemovePhone_InvalidPhone()
        {
            // Act
            this.student.RemovePhone("InvalidPhone");
        }

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

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestAddEmail_InvalidEmail()
        {
            // Act
            this.student.AddEmail("InvalidEmail");
        }

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

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestRemoveEmail_InvalidEmail()
        {
            // Act
            this.student.RemoveEmail("InvalidEmail");
        }
    }
}