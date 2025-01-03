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
            // Initialize a valid entity
            this.course = new Course
            {
                Name = "Advanced Programming",
                Description = "An advanced programming course.",
                Credits = 5,
                Cost = 500,
                MinThreshold = 50,
                MaxThreshold = 150,
            };
        }

        /// <summary>
        /// Tests if the course is valid.
        /// </summary>
        [TestMethod]
        public void TestCourseValid()
        {
            var context = new ValidationContext(this.course, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            Assert.IsTrue(Validator.TryValidateObject(this.course, context, results, validateAllProperties: true));
        }

        /// <summary>
        /// Tests if the course name is null.
        /// </summary>
        [TestMethod]
        public void TestNameNull()
        {
            this.course.Name = null;

            var context = new ValidationContext(this.course, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            Assert.IsFalse(Validator.TryValidateObject(this.course, context, results, validateAllProperties: true));
        }

        /// <summary>
        /// Tests if the course name is too short.
        /// </summary>
        [TestMethod]
        public void TestNameTooShort()
        {
            this.course.Name = "AB";

            var context = new ValidationContext(this.course, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            Assert.IsFalse(Validator.TryValidateObject(this.course, context, results, validateAllProperties: true));
        }

        /// <summary>
        /// Tests if the course description is missing.
        /// </summary>
        [TestMethod]
        public void TestDescriptionMissing()
        {
            this.course.Description = null;

            var context = new ValidationContext(this.course, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            Assert.IsFalse(Validator.TryValidateObject(this.course, context, results, validateAllProperties: true));
        }

        /// <summary>
        /// Tests if the course description is too long.
        /// </summary>
        [TestMethod]
        public void TestDescriptionTooLong()
        {
            this.course.Description = new string('a', 501);

            var context = new ValidationContext(this.course, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            Assert.IsFalse(Validator.TryValidateObject(this.course, context, results, validateAllProperties: true));
        }

        /// <summary>
        /// Tests if the course credits are negative.
        /// </summary>
        [TestMethod]
        public void TestCreditsNegative()
        {
            this.course.Credits = -1;

            var context = new ValidationContext(this.course, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            Assert.IsFalse(Validator.TryValidateObject(this.course, context, results, validateAllProperties: true));
        }

        /// <summary>
        /// Tests if the course cost is invalid.
        /// </summary>
        [TestMethod]
        public void TestCostInvalid()
        {
            this.course.Cost = 10; // Does not meet MinThreshold * Credits

            Assert.IsFalse(this.course.IsCostValid());
        }

        /// <summary>
        /// Tests if the course cost is valid.
        /// </summary>
        [TestMethod]
        public void TestCostValid()
        {
            this.course.Cost = 250; // Meets MinThreshold and MaxThreshold * Credits

            Assert.IsTrue(this.course.IsCostValid());
        }

        /// <summary>
        /// Tests if the thresholds are negative.
        /// </summary>
        [TestMethod]
        public void TestThresholdsNegative()
        {
            this.course.MinThreshold = -1;
            this.course.MaxThreshold = -1;

            var context = new ValidationContext(this.course, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            Assert.IsFalse(Validator.TryValidateObject(this.course, context, results, validateAllProperties: true));
        }

        /// <summary>
        /// Tests adding prerequisites to the course.
        /// </summary>
        [TestMethod]
        public void TestAddPrerequisites()
        {
            var prerequisite = new Prerequisite
            {
                CourseId = 1,
                PrerequisiteCourseId = 2,
                MinimumGrade = 7,
            };

            this.course.Prerequisites.Add(prerequisite);

            Assert.AreEqual(1, this.course.Prerequisites.Count);
            Assert.AreEqual(7, this.course.Prerequisites.ElementAt(0).MinimumGrade);
        }
    }
}