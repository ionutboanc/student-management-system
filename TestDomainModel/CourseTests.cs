using DomainModel;
using DomainModel.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace TestDomainModel
{
    [TestClass]
    public class CourseTests
    {
        private Course course;

        [TestInitialize]
        public void SetUp()
        {
            // Initialize a valid course entity
            this.course = new Course
            {
                Id = 1,
                Name = "Advanced Programming",
                Description = "An advanced programming course.",
                Credits = 5,
                Cost = 500,
                MinThreshold = 50,
                MaxThreshold = 150,
                Discount = new Discount { Percentage = 10 }
            };
        }

        [TestMethod]
        public void TestValidCourse()
        {
            var context = new ValidationContext(this.course, null, null);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(this.course, context, results, true);

            Assert.IsTrue(isValid);
            Assert.AreEqual(0, results.Count);
        }

        [TestMethod]
        public void TestCourseNameMissing()
        {
            this.course.Name = null;

            var context = new ValidationContext(this.course, null, null);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(this.course, context, results, true);

            Assert.IsFalse(isValid);
            Assert.IsTrue(results.Any(r => r.MemberNames.Contains("Name")));
        }

        [TestMethod]
        public void TestCourseCreditsInvalid()
        {
            this.course.Credits = 0;

            var context = new ValidationContext(this.course, null, null);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(this.course, context, results, true);

            Assert.IsFalse(isValid);
            Assert.IsTrue(results.Any(r => r.MemberNames.Contains("Credits")));
        }

        [TestMethod]
        public void TestCourseCostInvalid()
        {
            this.course.Cost = -1;

            var context = new ValidationContext(this.course, null, null);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(this.course, context, results, true);

            Assert.IsFalse(isValid);
            Assert.IsTrue(results.Any(r => r.MemberNames.Contains("Cost")));
        }

        [TestMethod]
        public void TestCourseMinThresholdInvalid()
        {
            this.course.MinThreshold = -1;

            var context = new ValidationContext(this.course, null, null);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(this.course, context, results, true);

            Assert.IsFalse(isValid);
            Assert.IsTrue(results.Any(r => r.MemberNames.Contains("MinThreshold")));
        }

        [TestMethod]
        public void TestCourseMaxThresholdInvalid()
        {
            this.course.MaxThreshold = -1;

            var context = new ValidationContext(this.course, null, null);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(this.course, context, results, true);

            Assert.IsFalse(isValid);
            Assert.IsTrue(results.Any(r => r.MemberNames.Contains("MaxThreshold")));
        }

        [TestMethod]
        public void TestIsCostValid_ValidCost()
        {
            this.course.Cost = this.course.MinThreshold * this.course.Credits;

            Assert.IsTrue(this.course.IsCostValid());

            this.course.Cost = this.course.MaxThreshold * this.course.Credits;

            Assert.IsTrue(this.course.IsCostValid());
        }

        [TestMethod]
        public void TestIsCostValid_InvalidCost()
        {
            this.course.Cost = (this.course.MinThreshold * this.course.Credits) - 1;

            Assert.IsFalse(this.course.IsCostValid());

            this.course.Cost = (this.course.MaxThreshold * this.course.Credits) + 1;

            Assert.IsFalse(this.course.IsCostValid());
        }

        [TestMethod]
        public void TestGetCost_WithDiscount()
        {
            var expectedCost = this.course.Cost - (this.course.Cost * this.course.Discount.Percentage / 100);

            Assert.AreEqual(expectedCost, this.course.GetCost());
        }

        [TestMethod]
        public void TestGetCost_WithoutDiscount()
        {
            this.course.Discount = null;

            Assert.AreEqual(this.course.Cost, this.course.GetCost());
        }

        [TestMethod]
        public void TestGetCost_WithBalanceRefunded()
        {
            this.course.BalanceRefunded = true;
            var expectedCost = (this.course.Cost * 0.5m) - (this.course.Cost * this.course.Discount.Percentage / 100 * 0.5m);

            Assert.AreEqual(expectedCost, this.course.GetCost());
        }
        [TestMethod]
        public void TestGetCost_WithoutBalanceRefunded_WithDiscount()
        {
            this.course.BalanceRefunded = false;
            var expectedCost = this.course.Cost - (this.course.Cost * this.course.Discount.Percentage / 100);

            Assert.AreEqual(expectedCost, this.course.GetCost());
        }

        [TestMethod]
        public void TestGetCost_WithoutBalanceRefunded_NoDiscount()
        {
            this.course.BalanceRefunded = false;
            this.course.Discount = null;

            Assert.AreEqual(this.course.Cost, this.course.GetCost());
        }

        [TestMethod]
        public void TestGetCost_WithBalanceRefunded_NoDiscount()
        {
            this.course.BalanceRefunded = true;
            this.course.Discount = null;
            var expectedCost = this.course.Cost * 0.5m;

            Assert.AreEqual(expectedCost, this.course.GetCost());
        }
        [TestMethod]
        public void TestAddPrerequisite()
        {
            var prerequisite = new Course
            {
                Id = 2,
                Name = "Introduction to Programming",
                Description = "An introductory programming course.",
                Credits = 3,
                Cost = 300,
                MinThreshold = 30,
                MaxThreshold = 90
            };

            this.course.Prerequisites.Add(prerequisite);

            Assert.AreEqual(1, this.course.Prerequisites.Count);
            Assert.IsTrue(this.course.Prerequisites.Contains(prerequisite));
        }

        [TestMethod]
        public void TestAddAvailableSemester()
        {
            var semester = new Semester
            {
                Id = 1,
                Number = 1,
                CreditThreshold = 30
            };

            this.course.AvailableSemesters.Add(semester);

            Assert.AreEqual(1, this.course.AvailableSemesters.Count);
            Assert.IsTrue(this.course.AvailableSemesters.Contains(semester));
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void TestValidate_MinThresholdGreaterThanMaxThreshold_ThrowsException()
        {
            this.course.MinThreshold = 200;
            this.course.MaxThreshold = 100;
            this.course.Validate();
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void TestValidate_CostNotWithinValidRange_ThrowsException()
        {
            this.course.Cost = (this.course.MinThreshold * this.course.Credits) - 1;
            this.course.Validate();
        }

        [TestMethod]
        public void TestCourseDescriptionNullable()
        {
            this.course.Description = null;

            var context = new ValidationContext(this.course, null, null);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(this.course, context, results, true);

            Assert.IsTrue(isValid);
            Assert.AreEqual(0, results.Count);
        }

        [TestMethod]
        public void TestCourseWithDiscount()
        {
            this.course.Discount = new Discount { Percentage = 20 };
            var expectedCost = this.course.Cost - (this.course.Cost * this.course.Discount.Percentage / 100);

            Assert.AreEqual(expectedCost, this.course.GetCost());
        }

        [TestMethod]
        public void TestCourseWithoutDiscount()
        {
            this.course.Discount = null;

            Assert.AreEqual(this.course.Cost, this.course.GetCost());
        }

        [TestMethod]
        public void TestAddStudentToCourse()
        {
            var student = new Student
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Address = "123 Main St",
                NationalId = "1234567890123",
                Balance = 1000m,
                RegistrationNumber = "123456"
            };

            this.course.Students.Add(student);

            Assert.AreEqual(1, this.course.Students.Count);
            Assert.IsTrue(this.course.Students.Contains(student));
        }

        [TestMethod]
        public void TestRemoveStudentFromCourse()
        {
            var student = new Student
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Address = "123 Main St",
                NationalId = "1234567890123",
                Balance = 1000m,
                RegistrationNumber = "123456"
            };

            this.course.Students.Add(student);
            this.course.Students.Remove(student);

            Assert.AreEqual(0, this.course.Students.Count);
            Assert.IsFalse(this.course.Students.Contains(student));
        }

        [TestMethod]
        public void TestAddExamToCourse()
        {
            var exam = new Exam
            {
                ExamId = 1,
                StudentId = 1,
                SemesterId = 1,
                CourseId = 1,
                Grade = 8,
                ExamDate = DateTime.Now
            };

            this.course.Exams.Add(exam);

            Assert.AreEqual(1, this.course.Exams.Count);
            Assert.IsTrue(this.course.Exams.Contains(exam));
        }

        [TestMethod]
        public void TestRemoveExamFromCourse()
        {
            var exam = new Exam
            {
                ExamId = 1,
                StudentId = 1,
                SemesterId = 1,
                CourseId = 1,
                Grade = 8,
                ExamDate = DateTime.Now
            };

            this.course.Exams.Add(exam);
            this.course.Exams.Remove(exam);

            Assert.AreEqual(0, this.course.Exams.Count);
            Assert.IsFalse(this.course.Exams.Contains(exam));
        }
    }
}
