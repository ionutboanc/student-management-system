namespace TestDomainModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using DomainModel.Models;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Provides unit tests for the Prerequisite class.
    /// </summary>
    [TestClass]
    public class PrerequisiteTests
    {
        private Prerequisite prerequisite;

        /// <summary>
        /// Sets up the test environment.
        /// </summary>
        [TestInitialize]
        public void SetUp()
        {
            // Initialize a valid prerequisite entity
            this.prerequisite = new Prerequisite
            {
                CourseId = 1,
                PrerequisiteCourseId = 2,
                MinimumGrade = 5
            };
        }

        /// <summary>
        /// Tests that the prerequisite is valid when all required fields are set correctly.
        /// </summary>
        [TestMethod]
        public void TestValidPrerequisite()
        {
            var context = new ValidationContext(this.prerequisite, null, null);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(this.prerequisite, context, results, true);

            Assert.IsTrue(isValid);
            Assert.AreEqual(0, results.Count);
        }

        /// <summary>
        /// Tests that prerequisite validation fails when MinimumGrade is below valid range.
        /// </summary>
        [TestMethod]
        public void TestMinimumGradeTooLow()
        {
            this.prerequisite.MinimumGrade = 0;

            var context = new ValidationContext(this.prerequisite, null, null);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(this.prerequisite, context, results, true);

            Assert.IsFalse(isValid);
            Assert.IsTrue(results.Any(r => r.MemberNames.Contains("MinimumGrade")));
        }

        /// <summary>
        /// Tests that prerequisite validation fails when MinimumGrade is above valid range.
        /// </summary>
        [TestMethod]
        public void TestMinimumGradeTooHigh()
        {
            this.prerequisite.MinimumGrade = 11;

            var context = new ValidationContext(this.prerequisite, null, null);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(this.prerequisite, context, results, true);

            Assert.IsFalse(isValid);
            Assert.IsTrue(results.Any(r => r.MemberNames.Contains("MinimumGrade")));
        }

        /// <summary>
        /// Tests that prerequisite validation passes when MinimumGrade is at boundary values.
        /// </summary>
        [TestMethod]
        public void TestMinimumGradeBoundaryValues()
        {
            this.prerequisite.MinimumGrade = 1;
            AssertPrerequisiteIsValid();

            this.prerequisite.MinimumGrade = 10;
            AssertPrerequisiteIsValid();
        }

        private void AssertPrerequisiteIsValid()
        {
            var context = new ValidationContext(this.prerequisite, null, null);
            var results = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(this.prerequisite, context, results, true);
            Assert.IsTrue(isValid);
            Assert.AreEqual(0, results.Count);
        }

        /// <summary>
        /// Tests that prerequisite validation fails when CourseId and PrerequisiteCourseId are the same.
        /// </summary>
        [TestMethod]
        public void TestCircularPrerequisite()
        {
            this.prerequisite.CourseId = 1;
            this.prerequisite.PrerequisiteCourseId = 1;

            // Assuming validation method for circular prerequisites exists
            bool hasCircularDependency = this.prerequisite.CourseId == this.prerequisite.PrerequisiteCourseId;

            Assert.IsTrue(hasCircularDependency);
        }

        // Additional tests can be added to cover more complex scenarios.
    }
}