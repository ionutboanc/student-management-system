using System.Collections.Generic;
using DomainModel.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestDomainModel
{
    [TestClass]
    public class DiscountTests
    {
        [TestMethod]
        public void TestDiscountInitialization()
        {
            // Arrange
            var discount = new Discount();

            // Act & Assert
            Assert.IsNotNull(discount.Courses);
            Assert.AreEqual(0, discount.Courses.Count);
        }

        [TestMethod]
        public void TestDiscountPercentage()
        {
            // Arrange
            var discount = new Discount
            {
                DiscountPercentage = 15.5m
            };

            // Act & Assert
            Assert.AreEqual(15.5m, discount.DiscountPercentage);
        }

        [TestMethod]
        public void TestAddCourseToDiscount()
        {
            // Arrange
            var discount = new Discount();
            var course = new Course
            {
                Id = 1,
                Name = "Math 101",
                Description = "Basic Mathematics",
                Credits = 3,
                Cost = 300
            };

            // Act
            discount.Courses.Add(course);

            // Assert
            Assert.AreEqual(1, discount.Courses.Count);
            Assert.AreEqual(course, discount.Courses[0]);
        }

        [TestMethod]
        public void TestRemoveCourseFromDiscount()
        {
            // Arrange
            var discount = new Discount();
            var course = new Course
            {
                Id = 1,
                Name = "Math 101",
                Description = "Basic Mathematics",
                Credits = 3,
                Cost = 300
            };
            discount.Courses.Add(course);

            // Act
            discount.Courses.Remove(course);

            // Assert
            Assert.AreEqual(0, discount.Courses.Count);
        }

        [TestMethod]
        public void TestDiscountConstructorWithParameters()
        {
            List<Course> courses = new List<Course>();
            var course = new Course
            {
                Id = 1,
                Name = "Math 101",
                Description = "Basic Mathematics",
                Credits = 3,
                Cost = 300
            };
            courses.Add(course);
            // Arrange
            var discount = new Discount(courses, 20m);

            // Act & Assert
            Assert.AreEqual(20m, discount.DiscountPercentage);
            Assert.IsNotNull(discount.Courses);
            Assert.AreEqual(1, discount.Courses.Count);
        }
    }
}