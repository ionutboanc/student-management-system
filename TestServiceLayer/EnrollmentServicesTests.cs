using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using DataMapper;
using DomainModel;
using DomainModel.Models;
using ServiceLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace TestServiceLayer
{
    [TestClass]
    public class EnrollmentServiceTests
    {
        private Mock<IEnrollmentDataServices> mockEnrollmentDataServices;
        private Mock<IDAOFactory> mockDAOFactory;
        private EnrollmentService enrollmentService;
        private Enrollment testEnrollment;
        private Course testCourse;
        private Student testStudent;

        [TestInitialize]
        public void SetUp()
        {
            this.mockEnrollmentDataServices = new Mock<IEnrollmentDataServices>();
            this.mockDAOFactory = new Mock<IDAOFactory>();
            this.enrollmentService = new EnrollmentService();

            // Set up the mock DAOFactory to return the mock data services
            this.mockDAOFactory.Setup(factory => factory.EnrollmentDataServices).Returns(this.mockEnrollmentDataServices.Object);
            typeof(DAOFactoryMethod).GetField("_currentDAOFactory", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic).SetValue(null, this.mockDAOFactory.Object);

            testCourse = new Course
            {
                Id = 1,
                Name = "Advanced Programming",
                Credits = 5
            };

            testStudent = new Student
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                NationalId = "1234567890123",
                RegistrationNumber = "123456",
                Balance = 1000m
            };

            testEnrollment = new Enrollment
            {
                Id = 1,
                StudentId = testStudent.Id,
                CourseId = testCourse.Id,
                SemesterId = 1,
                NumberOfAttempts = 0,
                Grade = 0
            };
        }

        [TestMethod]
        public void TestAddEnrollment_ValidEnrollment_AddsSuccessfully()
        {
            // Act
            enrollmentService.AddEnrollment(testEnrollment);

            // Assert
            mockEnrollmentDataServices.Verify(m => m.AddEnrollment(testEnrollment), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void TestAddEnrollment_InvalidEnrollment_ThrowsValidationException()
        {
            // Arrange
            testEnrollment.NumberOfAttempts = -1; // Invalid number of attempts

            // Act
            enrollmentService.AddEnrollment(testEnrollment);
        }

        [TestMethod]
        public void TestDeleteEnrollment_ValidEnrollment_DeletesSuccessfully()
        {
            // Act
            enrollmentService.DeleteEnrollment(testEnrollment);

            // Assert
            mockEnrollmentDataServices.Verify(m => m.DeleteEnrollment(testEnrollment), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestDeleteEnrollment_NullEnrollment_ThrowsArgumentNullException()
        {
            // Act
            enrollmentService.DeleteEnrollment(null);
        }

        [TestMethod]
        public void TestGetAllEnrollments_ReturnsEnrollments()
        {
            // Arrange
            var enrollments = new List<Enrollment> { testEnrollment };
            mockEnrollmentDataServices.Setup(m => m.GetAllEnrollments()).Returns(enrollments);

            // Act
            var result = enrollmentService.GetAllEnrollments();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
        }

        [TestMethod]
        public void TestUpdateEnrollment_ValidEnrollment_UpdatesSuccessfully()
        {
            // Act
            enrollmentService.UpdateEnrollment(testEnrollment);

            // Assert
            mockEnrollmentDataServices.Verify(m => m.UpdateEnrollment(testEnrollment), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void TestUpdateEnrollment_InvalidEnrollment_ThrowsValidationException()
        {
            // Arrange
            testEnrollment.NumberOfAttempts = -1; // Invalid number of attempts

            // Act
            enrollmentService.UpdateEnrollment(testEnrollment);
        }

        [TestMethod]
        public void TestGetEnrollmentById_ValidId_ReturnsEnrollment()
        {
            // Arrange
            mockEnrollmentDataServices.Setup(m => m.GetEnrollmentById(testEnrollment.Id)).Returns(testEnrollment);

            // Act
            var result = enrollmentService.GetEnrollmentById(testEnrollment.Id);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(testEnrollment.Id, result.Id);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestGetEnrollmentById_InvalidId_ThrowsArgumentOutOfRangeException()
        {
            // Act
            enrollmentService.GetEnrollmentById(0);
        }

        [TestMethod]
        public void TestRecordExamGrade_ValidGrade_RecordsSuccessfully()
        {
            // Arrange
            mockEnrollmentDataServices.Setup(m => m.GetEnrollmentById(testEnrollment.Id)).Returns(testEnrollment);

            // Act
            enrollmentService.RecordExamGrade(testEnrollment.Id, 8, DateTime.Now);

            // Assert
            mockEnrollmentDataServices.Verify(m => m.UpdateEnrollment(It.Is<Enrollment>(e => e.Grade == 8 && e.NumberOfAttempts == 1)), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestRecordExamGrade_MaxAttemptsReached_ThrowsInvalidOperationException()
        {
            // Arrange
            testEnrollment.NumberOfAttempts = Constants.MaxNumberOfAttempts;
            mockEnrollmentDataServices.Setup(m => m.GetEnrollmentById(testEnrollment.Id)).Returns(testEnrollment);

            // Act
            enrollmentService.RecordExamGrade(testEnrollment.Id, 8, DateTime.Now);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestRecordExamGrade_AlreadyPassed_ThrowsInvalidOperationException()
        {
            // Arrange
            testEnrollment.Grade = 6; // Already passed
            mockEnrollmentDataServices.Setup(m => m.GetEnrollmentById(testEnrollment.Id)).Returns(testEnrollment);

            // Act
            enrollmentService.RecordExamGrade(testEnrollment.Id, 8, DateTime.Now);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestRecordExamGrade_InvalidDate_ThrowsInvalidOperationException()
        {
            // Arrange
            testEnrollment.LastAttemptDate = DateTime.Now;
            mockEnrollmentDataServices.Setup(m => m.GetEnrollmentById(testEnrollment.Id)).Returns(testEnrollment);

            // Act
            enrollmentService.RecordExamGrade(testEnrollment.Id, 8, DateTime.Now.AddDays(-1));
        }
    }
}