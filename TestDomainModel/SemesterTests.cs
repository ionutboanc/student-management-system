namespace TestDomainModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using DomainModel.Models;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Provides unit tests for the Semester class.
    /// </summary>
    [TestClass]
    public class SemesterTests
    {
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
                Number = 1,
                CreditThreshold = 15,
                AvailableCourses = new List<Course>()
            };
        }

        /// <summary>
        /// Tests that the semester is valid when all required fields are set correctly.
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
        /// Tests that semester validation fails when Number is zero.
        /// </summary>
        [TestMethod]
        public void TestSemesterNumberZero()
        {
            this.semester.Number = 0;

            var context = new ValidationContext(this.semester, null, null);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(this.semester, context, results, true);

            Assert.IsFalse(isValid);
            Assert.IsTrue(results.Any(r => r.MemberNames.Contains("Number")));
        }

        /// <summary>
        /// Tests that semester validation fails when CreditThreshold is negative.
        /// </summary>
        [TestMethod]
        public void TestSemesterCreditThresholdNegative()
        {
            this.semester.CreditThreshold = -5;

            var context = new ValidationContext(this.semester, null, null);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(this.semester, context, results, true);

            Assert.IsFalse(isValid);
            Assert.IsTrue(results.Any(r => r.MemberNames.Contains("CreditThreshold")));
        }

        /// <summary>
        /// Tests adding a course to the semester's available courses.
        /// </summary>
        [TestMethod]
        public void TestAddAvailableCourse()
        {
            var course = new Course { Id = 1, Name = "Mathematics", Credits = 5 };
            this.semester.AvailableCourses.Add(course);

            Assert.AreEqual(1, this.semester.AvailableCourses.Count);
            Assert.AreEqual("Mathematics", this.semester.AvailableCourses.First().Name);
        }

        /// <summary>
        /// Tests that the total credits of available courses meet the credit threshold.
        /// </summary>
        [TestMethod]
        public void TestAvailableCoursesMeetCreditThreshold()
        {
            var course1 = new Course { Id = 1, Name = "Course 1", Credits = 5 };
            var course2 = new Course { Id = 2, Name = "Course 2", Credits = 5 };
            var course3 = new Course { Id = 3, Name = "Course 3", Credits = 5 };

            this.semester.AvailableCourses.Add(course1);
            this.semester.AvailableCourses.Add(course2);
            this.semester.AvailableCourses.Add(course3);

            int totalCredits = this.semester.AvailableCourses.Sum(c => c.Credits);

            Assert.IsTrue(totalCredits >= this.semester.CreditThreshold);
        }

        /// <summary>
        /// Tests that the initial state of AvailableCourses is an empty collection.
        /// </summary>
        [TestMethod]
        public void TestAvailableCoursesInitialState()
        {
            var newSemester = new Semester();
            Assert.IsNotNull(newSemester.AvailableCourses);
            Assert.AreEqual(0, newSemester.AvailableCourses.Count);
        }

        // Additional tests can be added to cover more complex scenarios.
    }
}