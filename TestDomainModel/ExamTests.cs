// <copyright file="{fileName}" company="Transilvania University of Brasov">
// Copyright (c) 2025 Boanc Ionut-Silviu. All rights reserved.
// </copyright>

namespace TestDomainModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using DomainModel;
    using DomainModel.Models;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Contains unit tests for the <see cref="Exam"/> class.
    /// </summary>
    [TestClass]
    public class ExamTests
    {
        /// <summary>
        /// The exam instance used for testing.
        /// </summary>
        private Exam exam;

        /// <summary>
        /// Sets up the test environment.
        /// </summary>
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
                },
            };
        }

        /// <summary>
        /// Tests if a valid exam passes validation.
        /// </summary>
        [TestMethod]
        public void TestValidExam()
        {
            var context = new ValidationContext(this.exam, null, null);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(this.exam, context, results, true);

            Assert.IsTrue(isValid);
            Assert.AreEqual(0, results.Count);
        }

        /// <summary>
        /// Tests if an exam with a negative grade fails validation.
        /// </summary>
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

        /// <summary>
        /// Tests if an exam with a grade above the valid range fails validation.
        /// </summary>
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

        /// <summary>
        /// Tests if an exam is passed when the grade is above the passing threshold.
        /// </summary>
        [TestMethod]
        public void TestIsPassed_GradeAboveThreshold()
        {
            this.exam.Grade = 6;

            Assert.IsTrue(this.exam.IsPassed());
        }

        /// <summary>
        /// Tests if an exam is not passed when the grade is below the passing threshold.
        /// </summary>
        [TestMethod]
        public void TestIsPassed_GradeBelowThreshold()
        {
            this.exam.Grade = 4;

            Assert.IsFalse(this.exam.IsPassed());
        }

        /// <summary>
        /// Tests validation when the student is null.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void TestValidate_StudentIsNull_ThrowsException()
        {
            this.exam.Student = null;
            this.exam.Validate();
        }

        /// <summary>
        /// Tests validation when the semester is null.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void TestValidate_SemesterIsNull_ThrowsException()
        {
            this.exam.Semester = null;
            this.exam.Validate();
        }

        /// <summary>
        /// Tests validation when the course is null.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void TestValidate_CourseIsNull_ThrowsException()
        {
            this.exam.Course = null;
            this.exam.Validate();
        }

        /// <summary>
        /// Tests validation when the grade is invalid.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void TestValidate_GradeInvalid_ThrowsException()
        {
            this.exam.Grade = 11;
            this.exam.Validate();
        }

        /// <summary>
        /// Tests validation when the exam date is required but not provided.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void TestValidate_ExamDateIsRequired_ThrowsException()
        {
            Exam testExam = new Exam
            {
                ExamId = 1,
                StudentId = 1,
                SemesterId = 1,
                CourseId = 1,
                Grade = 8,
            };

            testExam.Validate();
        }
    }
}