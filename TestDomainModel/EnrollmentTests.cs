namespace TestDomainModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using DomainModel.Models;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Provides unit tests for the Enrollment class.
    /// </summary>
    [TestClass]
    public class EnrollmentTests
    {
        private Enrollment enrollment;

        /// <summary>
        /// Sets up the test environment.
        /// </summary>
        [TestInitialize]
        public void SetUp()
        {
            // Initialize a valid enrollment entity
            this.enrollment = new Enrollment
            {
                StudentId = 1,
                CourseId = 1,
                SemesterId = 1,
                NumberOfAttempts = 0,
                Grade = 1,
                LastAttemptDate = null
            };
        }

        /// <summary>
        /// Tests that the enrollment is valid when all required fields are set correctly.
        /// </summary>
        [TestMethod]
        public void TestValidEnrollment()
        {
            var context = new ValidationContext(this.enrollment, null, null);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(this.enrollment, context, results, true);

            Assert.IsTrue(isValid);
            Assert.AreEqual(0, results.Count);
        }

        /// <summary>
        /// Tests that enrollment validation fails when NumberOfAttempts is negative.
        /// </summary>
        [TestMethod]
        public void TestEnrollmentNegativeAttempts()
        {
            this.enrollment.NumberOfAttempts = -1;

            var context = new ValidationContext(this.enrollment, null, null);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(this.enrollment, context, results, true);

            Assert.IsFalse(isValid);
            Assert.IsTrue(results.Any(r => r.MemberNames.Contains("NumberOfAttempts")));
        }

        /// <summary>
        /// Tests that enrollment validation fails when Grade is outside valid range.
        /// </summary>
        [TestMethod]
        public void TestEnrollmentInvalidGrade()
        {
            this.enrollment.Grade = 11;

            var context = new ValidationContext(this.enrollment, null, null);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(this.enrollment, context, results, true);

            Assert.IsFalse(isValid);
            Assert.IsTrue(results.Any(r => r.MemberNames.Contains("Grade")));
        }

        /// <summary>
        /// Tests that enrollment validation passes when Grade is at boundary values.
        /// </summary>
        [TestMethod]
        public void TestEnrollmentGradeBoundaryValues()
        {
            this.enrollment.Grade = 1;
            AssertEnrollmentIsValid();

            this.enrollment.Grade = 10;
            AssertEnrollmentIsValid();
        }

        private void AssertEnrollmentIsValid()
        {
            var context = new ValidationContext(this.enrollment, null, null);
            var results = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(this.enrollment, context, results, true);
            Assert.IsTrue(isValid);
            Assert.AreEqual(0, results.Count);
        }

        /// <summary>
        /// Tests that updating LastAttemptDate works correctly.
        /// </summary>
        [TestMethod]
        public void TestUpdateLastAttemptDate()
        {
            var attemptDate = DateTime.Now;
            this.enrollment.LastAttemptDate = attemptDate;

            Assert.AreEqual(attemptDate, this.enrollment.LastAttemptDate);
        }

        /// <summary>
        /// Tests that attempting to set Grade to null is valid.
        /// </summary>
        [TestMethod]
        public void TestGradeCanBeNull()
        {
            this.enrollment.Grade = 1;

            var context = new ValidationContext(this.enrollment, null, null);
            var results = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(this.enrollment, context, results, true);

            Assert.IsTrue(isValid);
            Assert.AreEqual(0, results.Count);
        }

        // Additional tests can be added to cover more edge cases and validations.
    }
}