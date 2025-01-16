// <copyright file="{fileName}" company="Transilvania University of Brasov">
// Copyright (c) 2025 Boanc Ionut-Silviu. All rights reserved.
// </copyright>

namespace TestDomainModel
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using DomainModel.Models;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Contains unit tests for the <see cref="Discount"/> class.
    /// </summary>
    [TestClass]
    public class DiscountTests
    {
        /// <summary>
        /// Tests the constructor of the <see cref="Discount"/> class with courses and percentage.
        /// </summary>
        [TestMethod]
        public void TestDiscountConstructorWithCoursesAndPercentage()
        {
            // Arrange
            var courses = new List<Course>
            {
                new Course
                {
                    Id = 1,
                    Name = "Math 101",
                    Description = "Basic Mathematics",
                    Credits = 3,
                    Cost = 300,
                },
            };
            var discount = new Discount(courses, 30m);

            // Act & Assert
            Assert.AreEqual(30m, discount.Percentage);
            Assert.IsNotNull(discount.Courses);
            Assert.AreEqual(1, discount.Courses.Count);
            Assert.AreEqual(courses[0], discount.Courses[0]);
        }

        /// <summary>
        /// Tests the constructor of the <see cref="Discount"/> class with empty courses list.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void TestDiscountConstructorWithEmptyCourses_ThrowsException()
        {
            // Act
            var discount = new Discount(new List<Course>(), 30m);
        }

        /// <summary>
        /// Tests if a negative discount percentage throws a validation exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void TestDiscountPercentageNegative_ThrowsException()
        {
            // Arrange
            var courses = new List<Course>
            {
                new Course
                {
                    Id = 1,
                    Name = "Math 101",
                    Description = "Basic Mathematics",
                    Credits = 3,
                    Cost = 300,
                },
            };
            var discount = new Discount(courses, -15m);

            // Act
            var context = new ValidationContext(discount, null, null);
            Validator.ValidateObject(discount, context, true);
        }

        /// <summary>
        /// Tests if a discount percentage greater than 100 throws a validation exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void TestDiscountPercentageTooBig_ThrowsException()
        {
            // Arrange
            var courses = new List<Course>
            {
                new Course
                {
                    Id = 1,
                    Name = "Math 101",
                    Description = "Basic Mathematics",
                    Credits = 3,
                    Cost = 300,
                },
            };
            var discount = new Discount(courses, 105m);

            // Act
            var context = new ValidationContext(discount, null, null);
            Validator.ValidateObject(discount, context, true);
        }

        /// <summary>
        /// Tests setting the discount percentage.
        /// </summary>
        [TestMethod]
        public void TestSetDiscountPercentage()
        {
            // Arrange
            var courses = new List<Course>
            {
                new Course
                {
                    Id = 1,
                    Name = "Math 101",
                    Description = "Basic Mathematics",
                    Credits = 3,
                    Cost = 300,
                },
            };
            var discount = new Discount(courses, 0m);

            // Act
            discount.Percentage = 50m;

            // Assert
            Assert.AreEqual(50m, discount.Percentage);
        }

        /// <summary>
        /// Tests adding courses to the discount.
        /// </summary>
        [TestMethod]
        public void TestAddCoursesToDiscount()
        {
            // Arrange
            var courses = new List<Course>
            {
                new Course
                {
                    Id = 1,
                    Name = "Math 101",
                    Description = "Basic Mathematics",
                    Credits = 3,
                    Cost = 300,
                },
            };
            var discount = new Discount(courses, 0m);
            var course2 = new Course
            {
                Id = 2,
                Name = "Physics 101",
                Description = "Basic Physics",
                Credits = 4,
                Cost = 400,
            };

            // Act
            discount.Courses.Add(course2);

            // Assert
            Assert.AreEqual(2, discount.Courses.Count);
            Assert.IsTrue(discount.Courses.Contains(courses[0]));
            Assert.IsTrue(discount.Courses.Contains(course2));
        }

        /// <summary>
        /// Tests removing courses from the discount.
        /// </summary>
        [TestMethod]
        public void TestRemoveCoursesFromDiscount()
        {
            // Arrange
            var courses = new List<Course>
            {
                new Course
                {
                    Id = 1,
                    Name = "Math 101",
                    Description = "Basic Mathematics",
                    Credits = 3,
                    Cost = 300,
                },
                new Course
                {
                    Id = 2,
                    Name = "Physics 101",
                    Description = "Basic Physics",
                    Credits = 4,
                    Cost = 400,
                },
            };
            var discount = new Discount(courses, 0m);

            // Act
            discount.Courses.Clear();

            // Assert
            Assert.AreEqual(0, discount.Courses.Count);
        }
    }
}