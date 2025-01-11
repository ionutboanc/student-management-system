using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using DataMapper;
using DomainModel.Models;
using ServiceLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TestServiceLayer
{
    [TestClass]
    public class SemesterServiceTests
    {
        private Mock<ISemesterDataServices> mockSemesterDataServices;
        private Mock<IDAOFactory> mockDAOFactory;
        private SemesterService semesterService;
        private Semester testSemester;
        private Course testCourse;

        [TestInitialize]
        public void SetUp()
        {
            this.mockSemesterDataServices = new Mock<ISemesterDataServices>();
            this.mockDAOFactory = new Mock<IDAOFactory>();
            this.semesterService = new SemesterService();

            this.mockDAOFactory.Setup(factory => factory.SemesterDataServices).Returns(this.mockSemesterDataServices.Object);
            typeof(DAOFactoryMethod).GetField("_currentDAOFactory", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic).SetValue(null, this.mockDAOFactory.Object);

            this.testCourse = new Course
            {
                Name = "Advanced Programming",
                Description = "An advanced programming course.",
                Credits = 5,
                Cost = 500,
                MinThreshold = 50,
                MaxThreshold = 150,
                DiscountId = 0,
            };

            this.testSemester = new Semester
            {
                Id = 1,
                Number = 1,
                CreditThreshold = 15,
            };
        }

        [TestMethod]
        public void TestAddSemester_ValidSemester_CallsDataServices()
        {
            // Act
            this.semesterService.AddSemester(this.testSemester);

            // Assert
            this.mockSemesterDataServices.Verify(repo => repo.AddSemester(this.testSemester), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void TestAddSemester_InvalidSemester_ThrowsValidationException()
        {
            // Arrange
            this.testSemester.CreditThreshold = -5;

            // Act
            this.semesterService.AddSemester(this.testSemester);
        }


        [TestMethod]
        public void TestDeleteSemester_ValidSemester_CallsDataServices()
        {
            // Act
            this.semesterService.DeleteSemester(this.testSemester);

            // Assert
            this.mockSemesterDataServices.Verify(repo => repo.DeleteSemester(this.testSemester), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestDeleteSemester_NullSemester_ThrowsException()
        {
            // Act
            this.semesterService.DeleteSemester(null);
        }

        [TestMethod]
        public void TestGetAllSemesters_ReturnsSemesters()
        {
            // Arrange
            var semesters = new List<Semester> { this.testSemester };
            this.mockSemesterDataServices.Setup(repo => repo.GetAllSemesters()).Returns(semesters);

            // Act
            var result = this.semesterService.GetAllSemesters();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
        }

        [TestMethod]
        public void TestUpdateSemester_ValidSemester_CallsDataServices()
        {
            // Act
            this.semesterService.UpdateSemester(this.testSemester);

            // Assert
            this.mockSemesterDataServices.Verify(repo => repo.UpdateSemester(this.testSemester), Times.Once);
        }

        [TestMethod]
        public void TestGetSemesterById_ValidId_ReturnsSemester()
        {
            // Arrange
            this.mockSemesterDataServices.Setup(repo => repo.GetSemesterById(1)).Returns(this.testSemester);

            // Act
            var result = this.semesterService.GetSemesterById(1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestGetSemesterById_InvalidId_ThrowsException()
        {
            // Act
            this.semesterService.GetSemesterById(0);
        }

        [TestMethod]
        public void TestAddCourseToSemester_ValidCourse_CourseAdded()
        {
            // Act
            this.semesterService.AddCourseToSemester(this.testSemester, this.testCourse);

            // Assert
            Assert.IsTrue(this.testSemester.AvailableCourses.Contains(this.testCourse));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestAddCourseToSemester_DuplicateCourse_ThrowsException()
        {
            // Arrange
            this.testSemester.AvailableCourses.Add(this.testCourse);

            // Act
            this.semesterService.AddCourseToSemester(this.testSemester, this.testCourse);
        }

        [TestMethod]
        public void TestRemoveCourseFromSemester_ValidCourse_CourseRemoved()
        {
            // Arrange
            this.testSemester.AvailableCourses.Add(this.testCourse);

            // Act
            this.semesterService.RemoveCourseFromSemester(this.testSemester, this.testCourse);

            // Assert
            Assert.IsFalse(this.testSemester.AvailableCourses.Contains(this.testCourse));
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void TestRemoveCourseFromSemester_NonExistentCourse_ThrowsException()
        {
            // Act
            this.semesterService.RemoveCourseFromSemester(this.testSemester, this.testCourse);
        }

        [TestMethod]
        public void TestGetCoursesInSemester_ReturnsCourses()
        {
            // Arrange
            this.testSemester.AvailableCourses.Add(this.testCourse);

            // Act
            var result = this.semesterService.GetCoursesInSemester(this.testSemester);

            // Assert
            Assert.AreEqual(1, result.Count);
        }

        [TestMethod]
        public void TestCalculateTotalCredits_ReturnsTotalCredits()
        {
            // Arrange
            this.testSemester.AvailableCourses.Add(this.testCourse);
            Course newTestCourse = new Course
            {
                Name = "More Advanced Programming",
                Description = "A more advanced programming course.",
                Credits = 10,
                Cost = 500,
                MinThreshold = 50,
                MaxThreshold = 150,
            };
            this.testSemester.AvailableCourses.Add(newTestCourse);

            // Act
            var totalCredits = this.semesterService.CalculateTotalCredits(this.testSemester);

            // Assert
            Assert.AreEqual(15, totalCredits);
        }

        [TestMethod]
        public void TestCanAddCourse_ValidScenario_ReturnsTrue()
        {
            // Arrange
            this.testCourse.Credits = 6;

            // Act
            var canAdd = this.semesterService.CanAddCourse(this.testSemester, this.testCourse);

            // Assert
            Assert.IsTrue(canAdd);
        }

        [TestMethod]
        public void TestCanAddCourse_ExceedsThreshold_ReturnsFalse()
        {
            // Arrange
            this.testCourse.MinThreshold = 1;
            this.testCourse.MaxThreshold = 100;
            this.testCourse.Credits = 16;

            // Act
            var canAdd = this.semesterService.CanAddCourse(this.testSemester, this.testCourse);

            // Assert
            Assert.IsFalse(canAdd);
        }
    }
}
