using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using DataMapper;
using DomainModel.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ServiceLayer;

namespace TestServiceLayer
{
    [TestClass]
    public class CourseServicesTests
    {
        private Mock<ICourseDataServices> mockDataServices;
        private Mock<IDAOFactory> mockDAOFactory;
        private CourseService courseService;
        private Course testCourse;
        private Discount testDiscount;

        [TestInitialize]
        public void SetUp()
        {
            this.mockDataServices = new Mock<ICourseDataServices>();
            this.mockDAOFactory = new Mock<IDAOFactory>();
            this.courseService = new CourseService();

            // Set up the mock DAOFactory to return the mockDataServices
            this.mockDAOFactory.Setup(factory => factory.CourseDataServices).Returns(this.mockDataServices.Object);
            typeof(DAOFactoryMethod).GetField("_currentDAOFactory", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic).SetValue(null, this.mockDAOFactory.Object);

            this.testDiscount = new Discount { DiscountPercentage = 10 };
            // Initialize a valid course entity
            this.testCourse = new Course
            {
                Name = "Advanced Programming",
                Description = "An advanced programming course.",
                Credits = 5,
                Cost = 500,
                MinThreshold = 50,
                MaxThreshold = 150,
                DiscountId = 0,
                Discount = this.testDiscount
            };
        }

        [TestMethod]
        public void TestAddCourse()
        {
            // Act
            this.courseService.AddCourse(this.testCourse);

            // Assert
            this.mockDataServices.Verify(repo => repo.AddCourse(this.testCourse), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestDeleteCourse_NullCourse_ThrowsException()
        {
            // Act
            this.courseService.DeleteCourse(null);
        }

        [TestMethod]
        public void TestDeleteCourse()
        {
            // Act
            this.courseService.DeleteCourse(this.testCourse);

            // Assert
            this.mockDataServices.Verify(repo => repo.DeleteCourse(this.testCourse), Times.Once);
        }

        [TestMethod]
        public void TestGetAllCourses()
        {
            // Arrange
            var courses = new List<Course> { this.testCourse };
            this.mockDataServices.Setup(repo => repo.GetAllCourses()).Returns(courses);

            // Act
            var result = this.courseService.GetAllCourses();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
        }

        [TestMethod]
        public void TestUpdateCourse()
        {
            // Act
            this.courseService.UpdateCourse(this.testCourse);

            // Assert
            this.mockDataServices.Verify(repo => repo.UpdateCourse(this.testCourse), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestGetCourseById_InvalidId_ThrowsException()
        {
            // Act
            this.courseService.GetCourseById(0);
        }

        [TestMethod]
        public void TestGetCourseById()
        {
            // Arrange
            this.mockDataServices.Setup(repo => repo.GetCourseById(1)).Returns(this.testCourse);

            // Act
            var result = this.courseService.GetCourseById(1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Advanced Programming", result.Name);
        }

        [TestMethod]
        public void TestGetCost_NoDiscount()
        {
            // Arrange
            this.testCourse.Discount = null;

            // Act
            var cost = this.courseService.GetCost(this.testCourse);

            // Assert
            Assert.AreEqual(500, cost);
        }

        [TestMethod]
        public void TestGetCost_WithDiscount()
        {
            // Act
            var cost = this.courseService.GetCost(this.testCourse);

            // Assert
            Assert.AreEqual(450, cost);
        }
    }
}