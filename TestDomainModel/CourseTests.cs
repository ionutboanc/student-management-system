namespace TestDomainModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using DomainModel.Models;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Provides unit tests for the Course class.
    /// </summary>
    [TestClass]
    public class CourseTests
    {
        private Course course;

        /// <summary>
        /// Sets up the test environment.
        /// </summary>
        [TestInitialize]
        public void SetUp()
        {
            // Initialize a valid course entity
            this.course = new Course
            {
                Name = "Advanced Programming",
                Description = "An advanced programming course.",
                Credits = 5,
                Cost = 500,
                MinThreshold = 50,
                MaxThreshold = 150,
                Prerequisites = new List<Prerequisite>(),
                Enrollments = new List<Enrollment>(),
                AvailableSemesters = new List<Semester>()
            };
        }

        /// <summary>
        /// Tests that the course is valid when all required fields are set correctly.
        /// </summary>
        [TestMethod]
        public void TestValidCourse()
        {
            var context = new ValidationContext(this.course, null, null);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(this.course, context, results, true);

            Assert.IsTrue(isValid);
            Assert.AreEqual(0, results.Count);
        }

        /// <summary>
        /// Tests that course validation fails when the name is missing.
        /// </summary>
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

        /// <summary>
        /// Tests that course validation fails when the name is too short.
        /// </summary>
        [TestMethod]
        public void TestCourseNameTooShort()
        {
            this.course.Name = "AB";

            var context = new ValidationContext(this.course, null, null);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(this.course, context, results, true);

            Assert.IsFalse(isValid);
            Assert.IsTrue(results.Any(r => r.MemberNames.Contains("Name")));
        }

        /// <summary>
        /// Tests that course validation fails when the description is missing.
        /// </summary>
        [TestMethod]
        public void TestCourseDescriptionMissing()
        {
            this.course.Description = null;

            var context = new ValidationContext(this.course, null, null);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(this.course, context, results, true);

            Assert.IsFalse(isValid);
            Assert.IsTrue(results.Any(r => r.MemberNames.Contains("Description")));
        }

        /// <summary>
        /// Tests that course validation fails when the description exceeds maximum length.
        /// </summary>
        [TestMethod]
        public void TestCourseDescriptionTooLong()
        {
            this.course.Description = new string('a', 501);

            var context = new ValidationContext(this.course, null, null);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(this.course, context, results, true);

            Assert.IsFalse(isValid);
            Assert.IsTrue(results.Any(r => r.MemberNames.Contains("Description")));
        }

        /// <summary>
        /// Tests that course validation fails when credits are zero.
        /// </summary>
        [TestMethod]
        public void TestCourseCreditsZero()
        {
            this.course.Credits = 0;

            var context = new ValidationContext(this.course, null, null);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(this.course, context, results, true);

            Assert.IsFalse(isValid);
            Assert.IsTrue(results.Any(r => r.MemberNames.Contains("Credits")));
        }

        /// <summary>
        /// Tests that course validation fails when credits are negative.
        /// </summary>
        [TestMethod]
        public void TestCourseCreditsNegative()
        {
            this.course.Credits = -3;

            var context = new ValidationContext(this.course, null, null);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(this.course, context, results, true);

            Assert.IsFalse(isValid);
            Assert.IsTrue(results.Any(r => r.MemberNames.Contains("Credits")));
        }

        /// <summary>
        /// Tests that course validation fails when cost is negative.
        /// </summary>
        [TestMethod]
        public void TestCourseCostNegative()
        {
            this.course.Cost = -100;

            var context = new ValidationContext(this.course, null, null);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(this.course, context, results, true);

            Assert.IsFalse(isValid);
            Assert.IsTrue(results.Any(r => r.MemberNames.Contains("Cost")));
        }

        /// <summary>
        /// Tests that course cost validation passes when cost is within the valid range.
        /// </summary>
        [TestMethod]
        public void TestCourseCostWithinValidRange()
        {
            this.course.Cost = this.course.MinThreshold * this.course.Credits;

            Assert.IsTrue(this.course.IsCostValid());

            this.course.Cost = this.course.MaxThreshold * this.course.Credits;

            Assert.IsTrue(this.course.IsCostValid());
        }

        /// <summary>
        /// Tests that course cost validation fails when cost is below minimum threshold.
        /// </summary>
        [TestMethod]
        public void TestCourseCostBelowMinimum()
        {
            this.course.Cost = (this.course.MinThreshold * this.course.Credits) - 1;

            Assert.IsFalse(this.course.IsCostValid());
        }

        /// <summary>
        /// Tests that course cost validation fails when cost is above maximum threshold.
        /// </summary>
        [TestMethod]
        public void TestCourseCostAboveMaximum()
        {
            this.course.Cost = (this.course.MaxThreshold * this.course.Credits) + 1;

            Assert.IsFalse(this.course.IsCostValid());
        }

        /// <summary>
        /// Tests that adding a prerequisite to the course works correctly.
        /// </summary>
        [TestMethod]
        public void TestAddPrerequisite()
        {
            var prerequisite = new Prerequisite
            {
                CourseId = this.course.Id,
                PrerequisiteCourseId = 2,
                MinimumGrade = 7
            };

            this.course.Prerequisites.Add(prerequisite);

            Assert.AreEqual(1, this.course.Prerequisites.Count);
            Assert.AreEqual(7, this.course.Prerequisites.First().MinimumGrade);
        }

        /// <summary>
        /// Tests that attempting to add a null prerequisite throws an exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestAddNullPrerequisite()
        {
            this.course.Prerequisites.Add(null);
        }

        /// <summary>
        /// Tests that course has availability in semesters.
        /// </summary>
        [TestMethod]
        public void TestCourseAvailableInSemester()
        {
            var semester = new Semester { Id = 1, Number = 1 };
            this.course.AvailableSemesters.Add(semester);

            Assert.AreEqual(1, this.course.AvailableSemesters.Count);
            Assert.AreEqual(1, this.course.AvailableSemesters.First().Id);
        }

        /// <summary>
        /// Tests that course validation fails when MinThreshold is negative.
        /// </summary>
        [TestMethod]
        public void TestCourseMinThresholdNegative()
        {
            this.course.MinThreshold = -10;

            var context = new ValidationContext(this.course, null, null);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(this.course, context, results, true);

            Assert.IsFalse(isValid);
            Assert.IsTrue(results.Any(r => r.MemberNames.Contains("MinThreshold")));
        }

        /// <summary>
        /// Tests that course validation fails when MaxThreshold is negative.
        /// </summary>
        [TestMethod]
        public void TestCourseMaxThresholdNegative()
        {
            this.course.MaxThreshold = -20;

            var context = new ValidationContext(this.course, null, null);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(this.course, context, results, true);

            Assert.IsFalse(isValid);
            Assert.IsTrue(results.Any(r => r.MemberNames.Contains("MaxThreshold")));
        }

        /// <summary>
        /// Tests that course validation fails when MaxThreshold is less than MinThreshold.
        /// </summary>
        [TestMethod]
        public void TestMaxThresholdLessThanMinThreshold()
        {
            this.course.MinThreshold = 100;
            this.course.MaxThreshold = 50;

            Assert.IsFalse(this.course.IsCostValid());
        }

        /// <summary>
        /// Tests that the IsCostValid method returns false when cost is not between thresholds.
        /// </summary>
        [TestMethod]
        public void TestIsCostValidMethod()
        {
            this.course.Cost = 400; // Within thresholds
            Assert.IsTrue(this.course.IsCostValid());

            this.course.Cost = 800; // Above max threshold
            Assert.IsFalse(this.course.IsCostValid());
        }

        /// <summary>
        /// Tests that the initial state of Prerequisites is an empty collection.
        /// </summary>
        [TestMethod]
        public void TestPrerequisitesInitialState()
        {
            var newCourse = new Course();
            Assert.IsNotNull(newCourse.Prerequisites);
            Assert.AreEqual(0, newCourse.Prerequisites.Count);
        }

        /// <summary>
        /// Tests that adding an enrollment to the course works correctly.
        /// </summary>
        [TestMethod]
        public void TestAddEnrollment()
        {
            var enrollment = new Enrollment
            {
                StudentId = 1,
                CourseId = this.course.Id,
                SemesterId = 1,
                NumberOfAttempts = 0
            };

            this.course.Enrollments.Add(enrollment);

            Assert.AreEqual(1, this.course.Enrollments.Count);
            Assert.AreEqual(1, this.course.Enrollments.First().StudentId);
        }

        /// <summary>
        /// Tests that attempting to add a null enrollment throws an exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestAddNullEnrollment()
        {
            this.course.Enrollments.Add(null);
        }

        // Additional tests can be added to cover more edge cases and validations.
    }
}