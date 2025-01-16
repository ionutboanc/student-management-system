using DomainModel.Models;
using DomainModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestDomainModel
{
    [TestClass]
    public class ExamTests
    {
        private Exam exam;

        [TestInitialize]
        public void SetUp()
        {
            // Initialize a valid exam entity
            this.exam = new Exam
            {
                ExamId = 1,
                StudentId = 1,
                SemesterId = 1,
                CourseId = 1,
                Grade = 8,
                ExamDate = DateTime.Now,
                Student = new Student
                {
                    Id = 1,
                    FirstName = "John",
                    LastName = "Doe",
                    Address = "123 Main St",
                    NationalId = "1234567890123",
                    Balance = 1000m,
                    RegistrationNumber = "123456",
                },
                Semester = new Semester
                {
                    Id = 1,
                    Number = 1,
                    CreditThreshold = 30,
                },
                Course = new Course
                {
                    Id = 1,
                    Name = "Advanced Programming",
                    Description = "An advanced programming course.",
                    Credits = 5,
                    Cost = 500,
                    MinThreshold = 50,
                    MaxThreshold = 150,
                }
            };
        }

        [TestMethod]
        public void TestValidExam()
        {
            var context = new ValidationContext(this.exam, null, null);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(this.exam, context, results, true);

            Assert.IsTrue(isValid);
            Assert.AreEqual(0, results.Count);
        }

        [TestMethod]
        public void TestGradeInvalidNegative()
        {
            this.exam.Grade = -10;

            var context = new ValidationContext(this.exam, null, null);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(this.exam, context, results, true);

            Assert.IsFalse(isValid);
            Assert.IsTrue(results.Any(r => r.MemberNames.Contains("Grade")));
        }

        [TestMethod]
        public void TestGradeInvalid()
        {
            this.exam.Grade = 11;

            var context = new ValidationContext(this.exam, null, null);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(this.exam, context, results, true);

            Assert.IsFalse(isValid);
            Assert.IsTrue(results.Any(r => r.MemberNames.Contains("Grade")));
        }

        [TestMethod]
        public void TestIsPassed_GradeAboveThreshold()
        {
            this.exam.Grade = 6;

            Assert.IsTrue(this.exam.IsPassed());
        }

        [TestMethod]
        public void TestIsPassed_GradeBelowThreshold()
        {
            this.exam.Grade = 4;

            Assert.IsFalse(this.exam.IsPassed());
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void TestValidate_StudentIsNull_ThrowsException()
        {
            this.exam.Student = null;
            this.exam.Validate();
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void TestValidate_SemesterIsNull_ThrowsException()
        {
            this.exam.Semester = null;
            this.exam.Validate();
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void TestValidate_CourseIsNull_ThrowsException()
        {
            this.exam.Course = null;
            this.exam.Validate();
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void TestValidate_GradeInvalid_ThrowsException()
        {
            this.exam.Grade = 11;
            this.exam.Validate();
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void TestValidate_ExamDateIsRequired_ThrowsException()
        {
            Exam TestExam = new Exam
            {
                ExamId = 1,
                StudentId = 1,
                SemesterId = 1,
                CourseId = 1,
                Grade = 8,
            };

            TestExam.Validate();
        }
    }
}
