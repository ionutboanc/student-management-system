using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using DataMapper;
using DomainModel.Models;
using ServiceLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace TestServiceLayer
{
    [TestClass]
    public class PrerequisiteServicesTests
    {
        private Mock<IPrerequisiteDataServices> mockPrerequisiteDataServices;
        private Mock<ICourseDataServices> mockCourseDataServices;
        private Mock<IStudentDataServices> mockStudentDataServices;
        private Mock<IDAOFactory> mockDAOFactory;
        private PrerequisiteService prerequisiteService;
        private Prerequisite testPrerequisite;
        private Course testCourse;
        private Course testPrerequisiteCourse;
        private Student testStudent;

        [TestInitialize]
        public void SetUp()
        {
            this.mockPrerequisiteDataServices = new Mock<IPrerequisiteDataServices>();
            this.mockCourseDataServices = new Mock<ICourseDataServices>();
            this.mockStudentDataServices = new Mock<IStudentDataServices>();
            this.mockDAOFactory = new Mock<IDAOFactory>();
            this.prerequisiteService = new PrerequisiteService();

            // Set up the mock DAOFactory to return the mock data services
            this.mockDAOFactory.Setup(factory => factory.PrerequisiteDataServices).Returns(this.mockPrerequisiteDataServices.Object);
            this.mockDAOFactory.Setup(factory => factory.CourseDataServices).Returns(this.mockCourseDataServices.Object);
            this.mockDAOFactory.Setup(factory => factory.StudentDataServices).Returns(this.mockStudentDataServices.Object);
            typeof(DAOFactoryMethod).GetField("_currentDAOFactory", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic).SetValue(null, this.mockDAOFactory.Object);

            testCourse = new Course
            {
                Id = 1,
                Name = "Advanced Programming",
                AvailableSemesters = new List<Semester> { new Semester { Number = 2 } }
            };

            testPrerequisiteCourse = new Course
            {
                Id = 2,
                Name = "Introduction to Programming",
                AvailableSemesters = new List<Semester> { new Semester { Number = 1 } }
            };

            testPrerequisite = new Prerequisite
            {
                Id = 1,
                CourseId = testCourse.Id,
                PrerequisiteCourseId = testPrerequisiteCourse.Id,
                MinimumGrade = 5
            };

            testStudent = new Student
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Enrollments = new List<Enrollment>
                {
                    new Enrollment { CourseId = testPrerequisiteCourse.Id, Grade = 6 }
                }
            };
        }

        [TestMethod]
        public void TestAddPrerequisite_ValidPrerequisite_AddsSuccessfully()
        {
            // Arrange
            mockCourseDataServices.Setup(m => m.GetCourseById(testCourse.Id)).Returns(testCourse);
            mockCourseDataServices.Setup(m => m.GetCourseById(testPrerequisiteCourse.Id)).Returns(testPrerequisiteCourse);

            // Act
            prerequisiteService.AddPrerequisite(testPrerequisite);

            // Assert
            mockPrerequisiteDataServices.Verify(m => m.AddPrerequisite(testPrerequisite), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void TestAddPrerequisite_InvalidPrerequisite_ThrowsValidationException()
        {
            // Arrange
            testPrerequisite.MinimumGrade = 11; // Invalid grade

            // Act
            prerequisiteService.AddPrerequisite(testPrerequisite);
        }

        [TestMethod]
        public void TestDeletePrerequisite_ValidPrerequisite_DeletesSuccessfully()
        {
            // Act
            prerequisiteService.DeletePrerequisite(testPrerequisite);

            // Assert
            mockPrerequisiteDataServices.Verify(m => m.DeletePrerequisite(testPrerequisite), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestDeletePrerequisite_NullPrerequisite_ThrowsArgumentNullException()
        {
            // Act
            prerequisiteService.DeletePrerequisite(null);
        }

        [TestMethod]
        public void TestGetAllPrerequisites_ReturnsPrerequisites()
        {
            // Arrange
            var prerequisites = new List<Prerequisite> { testPrerequisite };
            mockPrerequisiteDataServices.Setup(m => m.GetAllPrerequisites()).Returns(prerequisites);

            // Act
            var result = prerequisiteService.GetAllPrerequisites();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
        }

        [TestMethod]
        public void TestUpdatePrerequisite_ValidPrerequisite_UpdatesSuccessfully()
        {
            // Arrange
            mockCourseDataServices.Setup(m => m.GetCourseById(testCourse.Id)).Returns(testCourse);
            mockCourseDataServices.Setup(m => m.GetCourseById(testPrerequisiteCourse.Id)).Returns(testPrerequisiteCourse);

            // Act
            prerequisiteService.UpdatePrerequisite(testPrerequisite);

            // Assert
            mockPrerequisiteDataServices.Verify(m => m.UpdatePrerequisite(testPrerequisite), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void TestUpdatePrerequisite_InvalidPrerequisite_ThrowsValidationException()
        {
            // Arrange
            testPrerequisite.MinimumGrade = 11; // Invalid grade

            // Act
            prerequisiteService.UpdatePrerequisite(testPrerequisite);
        }

        [TestMethod]
        public void TestGetPrerequisiteById_ValidId_ReturnsPrerequisite()
        {
            // Arrange
            mockPrerequisiteDataServices.Setup(m => m.GetPrerequisiteById(testPrerequisite.Id)).Returns(testPrerequisite);

            // Act
            var result = prerequisiteService.GetPrerequisiteById(testPrerequisite.Id);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(testPrerequisite.Id, result.Id);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestGetPrerequisiteById_InvalidId_ThrowsArgumentOutOfRangeException()
        {
            // Act
            prerequisiteService.GetPrerequisiteById(0);
        }

        [TestMethod]
        public void TestGetPrerequisitesForCourse_ValidCourseId_ReturnsPrerequisites()
        {
            // Arrange
            var prerequisites = new List<Prerequisite> { testPrerequisite };
            mockPrerequisiteDataServices.Setup(m => m.GetAllPrerequisites()).Returns(prerequisites);

            // Act
            var result = prerequisiteService.GetPrerequisitesForCourse(testCourse.Id);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void TestValidatePrerequisiteStructure_CircularDependency_ThrowsValidationException()
        {
            // Arrange
            var circularPrerequisite = new Prerequisite
            {
                Id = 2,
                CourseId = testPrerequisiteCourse.Id,
                PrerequisiteCourseId = testCourse.Id,
                MinimumGrade = 5
            };
            var prerequisites = new List<Prerequisite> { testPrerequisite, circularPrerequisite };
            mockPrerequisiteDataServices.Setup(m => m.GetAllPrerequisites()).Returns(prerequisites);

            // Act
            prerequisiteService.ValidatePrerequisiteStructure(testCourse.Id);
        }

        [TestMethod]
        public void TestCheckStudentEligibilityForCourse_EligibleStudent_ReturnsTrue()
        {
            // Arrange
            mockStudentDataServices.Setup(m => m.GetStudentById(testStudent.Id)).Returns(testStudent);
            mockPrerequisiteDataServices.Setup(m => m.GetAllPrerequisites()).Returns(new List<Prerequisite> { testPrerequisite });

            // Act
            var result = prerequisiteService.CheckStudentEligibilityForCourse(testStudent.Id, testCourse.Id);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestCheckStudentEligibilityForCourse_IneligibleStudent_ReturnsFalse()
        {
            // Arrange
            testStudent.Enrollments.First().Grade = 4; // Below minimum grade
            mockStudentDataServices.Setup(m => m.GetStudentById(testStudent.Id)).Returns(testStudent);
            mockPrerequisiteDataServices.Setup(m => m.GetAllPrerequisites()).Returns(new List<Prerequisite> { testPrerequisite });

            // Act
            var result = prerequisiteService.CheckStudentEligibilityForCourse(testStudent.Id, testCourse.Id);

            // Assert
            Assert.IsFalse(result);
        }
    }
}