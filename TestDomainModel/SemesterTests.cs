// <copyright file="{fileName}" company="Transilvania University of Brasov">
// Copyright (c) 2025 Boanc Ionut-Silviu. All rights reserved.
// </copyright>

namespace TestDomainModel
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using DomainModel.Models;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Contains unit tests for the <see cref="Semester"/> class.
    /// </summary>
    [TestClass]
    public class SemesterTests
    {
        /// <summary>
        /// The semester instance used for testing.
        /// </summary>
        private Semester semester;

        /// <summary>
        /// Sets up the test environment.
        /// </summary>
        [TestInitialize]
        public void SetUp()
        {
            // Initialize a valid semester entity
            this.semester = new Semester
            {
                Id = 1,
                Number = 1,
                CreditThreshold = 30,
                AvailableCourses = new List<Course>
                {
                    new Course
                    {
                        Id = 1,
                        Name = "Advanced Programming",
                        Description = "An advanced programming course.",
                        Credits = 5,
                        Cost = 500,
                        MinThreshold = 50,
                        MaxThreshold = 150,
                    },
                },
                Students = new List<Student>
                {
                    new Student
                    {
                        Id = 1,
                        FirstName = "John",
                        LastName = "Doe",
                        Address = "123 Main St",
                        NationalId = "1234567890123",
                        Balance = 1000m,
                        RegistrationNumber = "123456",
                    },
                },
            };
        }

        /// <summary>
        /// Tests if a valid semester passes validation.
        /// </summary>
        [TestMethod]
        public void TestValidSemester()
        {
            var context = new ValidationContext(this.semester, null, null);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(this.semester, context, results, true);

            Assert.IsTrue(isValid);
            Assert.AreEqual(0, results.Count);
        }

        /// <summary>
        /// Tests if a semester with an invalid number fails validation.
        /// </summary>
        [TestMethod]
        public void TestSemesterNumberInvalid()
        {
            this.semester.Number = 0;

            var context = new ValidationContext(this.semester, null, null);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(this.semester, context, results, true);

            Assert.IsFalse(isValid);
            Assert.IsTrue(results.Any(r => r.MemberNames.Contains("Number")));
        }

        /// <summary>
        /// Tests if a semester with an invalid credit threshold fails validation.
        /// </summary>
        [TestMethod]
        public void TestCreditThresholdInvalid()
        {
            this.semester.CreditThreshold = -1;

            var context = new ValidationContext(this.semester, null, null);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(this.semester, context, results, true);

            Assert.IsFalse(isValid);
            Assert.IsTrue(results.Any(r => r.MemberNames.Contains("CreditThreshold")));
        }

        /// <summary>
        /// Tests adding a course to the semester.
        /// </summary>
        [TestMethod]
        public void TestAddCourseToSemester()
        {
            var newCourse = new Course
            {
                Id = 2,
                Name = "Introduction to Programming",
                Description = "An introductory programming course.",
                Credits = 3,
                Cost = 300,
                MinThreshold = 30,
                MaxThreshold = 90,
            };

            this.semester.AvailableCourses.Add(newCourse);

            Assert.AreEqual(2, this.semester.AvailableCourses.Count);
            Assert.IsTrue(this.semester.AvailableCourses.Contains(newCourse));
        }

        /// <summary>
        /// Tests adding a student to the semester.
        /// </summary>
        [TestMethod]
        public void TestAddStudentToSemester()
        {
            var newStudent = new Student
            {
                Id = 2,
                FirstName = "Jane",
                LastName = "Doe",
                Address = "456 Main St",
                NationalId = "9876543210987",
                Balance = 500m,
                RegistrationNumber = "654321",
            };

            this.semester.Students.Add(newStudent);

            Assert.AreEqual(2, this.semester.Students.Count);
            Assert.IsTrue(this.semester.Students.Contains(newStudent));
        }

        /// <summary>
        /// Tests removing a course from the semester.
        /// </summary>
        [TestMethod]
        public void TestRemoveCourseFromSemester()
        {
            var courseToRemove = this.semester.AvailableCourses.First();

            this.semester.AvailableCourses.Remove(courseToRemove);

            Assert.AreEqual(0, this.semester.AvailableCourses.Count);
            Assert.IsFalse(this.semester.AvailableCourses.Contains(courseToRemove));
        }

        /// <summary>
        /// Tests removing a student from the semester.
        /// </summary>
        [TestMethod]
        public void TestRemoveStudentFromSemester()
        {
            var studentToRemove = this.semester.Students.First();

            this.semester.Students.Remove(studentToRemove);

            Assert.AreEqual(0, this.semester.Students.Count);
            Assert.IsFalse(this.semester.Students.Contains(studentToRemove));
        }

        /// <summary>
        /// Tests validating a semester with valid data.
        /// </summary>
        [TestMethod]
        public void TestValidate_ValidSemester()
        {
            // Arrange
            var semester = new Semester
            {
                Id = 1,
                Number = 1,
                CreditThreshold = 10,
                AvailableCourses = new List<Course>
                {
                    new Course { Id = 1, Name = "Course 1", Credits = 5 },
                    new Course { Id = 2, Name = "Course 2", Credits = 5 },
                },
                Students = new List<Student>
                {
                    new Student { Id = 1, FirstName = "John", LastName = "Doe" },
                },
            };

            // Act & Assert
            semester.Validate();
        }

        /// <summary>
        /// Tests validating a semester with no available courses.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ValidationException), "Semester must have at least one available course.")]
        public void TestValidate_NoAvailableCourses_ThrowsException()
        {
            // Arrange
            var semester = new Semester
            {
                Id = 1,
                Number = 1,
                CreditThreshold = 10,
                AvailableCourses = new List<Course>(),
                Students = new List<Student>
                {
                    new Student { Id = 1, FirstName = "John", LastName = "Doe" },
                },
            };

            // Act & Assert
            semester.Validate();
        }

        /// <summary>
        /// Tests validating a semester with no students.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ValidationException), "Semester must have at least one student.")]
        public void TestValidate_NoStudents_ThrowsException()
        {
            // Arrange
            var semester = new Semester
            {
                Id = 1,
                Number = 1,
                CreditThreshold = 10,
                AvailableCourses = new List<Course>
                {
                    new Course { Id = 1, Name = "Course 1", Credits = 5 },
                    new Course { Id = 2, Name = "Course 2", Credits = 5 },
                },
                Students = new List<Student>(),
            };

            // Act & Assert
            semester.Validate();
        }

        /// <summary>
        /// Tests validating a semester with total credits less than the credit threshold.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ValidationException), "The total credits of the available courses (5) must be equal to or more than the credit threshold (10).")]
        public void TestValidate_TotalCreditsLessThanThreshold_ThrowsException()
        {
            // Arrange
            var semester = new Semester
            {
                Id = 1,
                Number = 1,
                CreditThreshold = 10,
                AvailableCourses = new List<Course>
                {
                    new Course { Id = 1, Name = "Course 1", Credits = 5 },
                },
                Students = new List<Student>
                {
                    new Student { Id = 1, FirstName = "Boanc", LastName = "Ionut" },
                },
            };

            // Act & Assert
            semester.Validate();
        }

        /// <summary>
        /// Tests validating a semester with total credits equal to the credit threshold.
        /// </summary>
        [TestMethod]
        public void TestValidate_TotalCreditsEqualToThreshold()
        {
            // Arrange
            var semester = new Semester
            {
                Id = 1,
                Number = 1,
                CreditThreshold = 10,
                AvailableCourses = new List<Course>
                {
                    new Course { Id = 1, Name = "Course 1", Credits = 5 },
                    new Course { Id = 2, Name = "Course 2", Credits = 5 },
                },
                Students = new List<Student>
                {
                    new Student { Id = 1, FirstName = "Boanc", LastName = "Ionut" },
                },
            };

            // Act & Assert
            semester.Validate();
        }

        /// <summary>
        /// Tests validating a semester with total credits more than the credit threshold.
        /// </summary>
        [TestMethod]
        public void TestValidate_TotalCreditsMoreThanThreshold()
        {
            // Arrange
            var semester = new Semester
            {
                Id = 1,
                Number = 1,
                CreditThreshold = 10,
                AvailableCourses = new List<Course>
                {
                    new Course { Id = 1, Name = "Course 1", Credits = 5 },
                    new Course { Id = 2, Name = "Course 2", Credits = 6 },
                },
                Students = new List<Student>
                {
                    new Student { Id = 1, FirstName = "Boanc", LastName = "Ionut" },
                },
            };

            // Act & Assert
            semester.Validate();
        }
    }
}