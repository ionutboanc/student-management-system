using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using DataMapper;
using DomainModel.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ServiceLayer.ServiceImplementation;

namespace TestServiceLayer
{
    /// <summary>
    /// Unit tests for the DiscountService class.
    /// </summary>
    [TestClass]
    public class DiscountServiceTests
    {
        private Mock<IDiscountDataServices> mockDataServices;
        private Mock<IDAOFactory> mockDAOFactory;
        private DiscountService discountService;
        private Discount testDiscount;
        private Course testCourse;

        /// <summary>
        /// Sets up the test environment.
        /// </summary>
        [TestInitialize]
        public void SetUp()
        {
            this.mockDataServices = new Mock<IDiscountDataServices>();
            this.mockDAOFactory = new Mock<IDAOFactory>();
            this.discountService = new DiscountService();

            // Set up the mock DAOFactory to return the mock data services
            this.mockDAOFactory.Setup(factory => factory.DiscountDataServices).Returns(this.mockDataServices.Object);
            typeof(DAOFactoryMethod).GetField("_currentDAOFactory", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic).SetValue(null, this.mockDAOFactory.Object);

            // Initialize a valid course entity
            this.testCourse = new Course
            {
                Id = 1,
                Name = "Advanced Programming",
                Description = "An advanced programming course.",
                Credits = 5,
                Cost = 500,
                MinThreshold = 50,
                MaxThreshold = 150
            };

            // Initialize a valid discount entity
            this.testDiscount = new Discount
            {
                Id = 1,
                Percentage = 10,
                Courses = new List<Course> { this.testCourse }
            };
        }

        /// <summary>
        /// Tests adding a discount.
        /// </summary>
        [TestMethod]
        public void TestAddDiscount()
        {
            // Act
            this.discountService.AddDiscount(this.testDiscount);

            // Assert
            this.mockDataServices.Verify(repo => repo.AddDiscount(this.testDiscount), Times.Once);
        }

        /// <summary>
        /// Tests updating a discount.
        /// </summary>
        [TestMethod]
        public void TestUpdateDiscount()
        {
            // Act
            this.discountService.UpdateDiscount(this.testDiscount);

            // Assert
            this.mockDataServices.Verify(repo => repo.UpdateDiscount(this.testDiscount), Times.Once);
        }

        /// <summary>
        /// Tests deleting a discount.
        /// </summary>
        [TestMethod]
        public void TestDeleteDiscount()
        {
            // Arrange
            this.mockDataServices.Setup(repo => repo.GetDiscountById(1)).Returns(this.testDiscount);

            // Act
            this.discountService.DeleteDiscount(this.testDiscount);

            // Assert
            this.mockDataServices.Verify(repo => repo.DeleteDiscount(this.testDiscount), Times.Once);
        }

        /// <summary>
        /// Tests deleting a discount with an invalid ID.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestDeleteDiscount_InvalidId_ThrowsException()
        {
            // Act
            this.discountService.DeleteDiscount(null);
        }

        /// <summary>
        /// Tests getting a discount by ID.
        /// </summary>
        [TestMethod]
        public void TestGetDiscountById()
        {
            // Arrange
            this.mockDataServices.Setup(repo => repo.GetDiscountById(1)).Returns(this.testDiscount);

            // Act
            var result = this.discountService.GetDiscountById(1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(10, result.Percentage);
            Assert.AreEqual(1, result.Courses.Count);
            Assert.AreEqual("Advanced Programming", result.Courses.First().Name);
        }

        /// <summary>
        /// Tests getting a discount by an invalid ID.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestGetDiscountById_InvalidId_ThrowsException()
        {
            // Act
            this.discountService.GetDiscountById(0);
        }

        /// <summary>
        /// Tests getting all discounts.
        /// </summary>
        [TestMethod]
        public void TestGetAllDiscounts()
        {
            // Arrange
            var discounts = new List<Discount> { this.testDiscount };
            this.mockDataServices.Setup(repo => repo.GetAllDiscounts()).Returns(discounts);

            // Act
            var result = this.discountService.GetAllDiscounts();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
        }

        public void TestGetAllDiscounts_NoDiscounts()
        {
            // Arrange
            this.mockDataServices.Setup(repo => repo.GetAllDiscounts()).Returns(new List<Discount>());
            // Act
            var result = this.discountService.GetAllDiscounts();
            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }

        /// <summary>
        /// Tests adding an invalid discount.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void TestAddDiscount_InvalidDiscount_ThrowsException()
        {
            // Arrange
            this.testDiscount.Percentage = -1;

            // Act
            this.discountService.AddDiscount(this.testDiscount);
        }

        /// <summary>
        /// Tests updating an invalid discount.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void TestUpdateDiscount_InvalidDiscount_ThrowsException()
        {
            // Arrange
            this.testDiscount.Percentage = -1;

            // Act
            this.discountService.UpdateDiscount(this.testDiscount);
        }

        /// <summary>
        /// Tests validating a valid discount.
        /// </summary>
        [TestMethod]
        public void TestValidateDiscount_ValidDiscount()
        {
            // Act
            this.discountService.ValidateDiscount(this.testDiscount);

            // Assert
            Assert.IsTrue(true); // If no exception is thrown, the test passes
        }

        /// <summary>
        /// Tests validating a null discount.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestValidateDiscount_NullDiscount_ThrowsException()
        {
            // Act
            this.discountService.ValidateDiscount(null);
        }

        /// <summary>
        /// Tests validating a discount with a percentage out of range.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void TestValidateDiscount_PercentageOutOfRange_ThrowsException()
        {
            // Arrange
            this.testDiscount.Percentage = 110;

            // Act
            this.discountService.ValidateDiscount(this.testDiscount);
        }

        /// <summary>
        /// Tests adding a discount with no courses.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void TestAddDiscount_NoCourses()
        {
            // Arrange
            var discount = new Discount
            {
                Id = 2,
                Percentage = 15,
                Courses = new List<Course>()
            };

            // Act
            this.discountService.AddDiscount(discount);

            // Assert
            this.mockDataServices.Verify(repo => repo.AddDiscount(discount), Times.Once);
        }

        /// <summary>
        /// Tests updating a discount with no courses.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void TestUpdateDiscount_NoCourses()
        {
            // Arrange
            var discount = new Discount
            {
                Id = 2,
                Percentage = 15,
                Courses = new List<Course>()
            };

            // Act
            this.discountService.UpdateDiscount(discount);

            // Assert
            this.mockDataServices.Verify(repo => repo.UpdateDiscount(discount), Times.Once);
        }
    }
}