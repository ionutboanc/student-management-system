// <copyright file="{fileName}" company="Transilvania University of Brasov">
// Copyright (c) 2025 Boanc Ionut-Silviu. All rights reserved.
// </copyright>

namespace TestServiceLayer
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using DataMapper;
    using DomainModel.Models;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using ServiceLayer;

    /// <summary>
    /// Unit tests for the <see cref="SemesterService"/> class.
    /// </summary>
    [TestClass]
    public class SemesterServiceTests
    {
        /// <summary>
        /// Mock for semester data services.
        /// </summary>
        private Mock<ISemesterDataServices> mockSemesterDataServices;

        /// <summary>
        /// Mock for DAO factory.
        /// </summary>
        private Mock<IDAOFactory> mockDAOFactory;

        /// <summary>
        /// The semester service instance used for testing.
        /// </summary>
        private SemesterService semesterService;

        /// <summary>
        /// The test semester instance.
        /// </summary>
        private Semester testSemester;

        /// <summary>
        /// The test course instance.
        /// </summary>
        private Course testCourse;

        /// <summary>
        /// The test student instance.
        /// </summary>
        private Student testStudent;

        /// <summary>
        /// Sets up the test environment.
        /// </summary>
        [TestInitialize]
        public void SetUp()
        {
            this.mockSemesterDataServices = new Mock<ISemesterDataServices>();
            this.mockDAOFactory = new Mock<IDAOFactory>();
            this.semesterService = new SemesterService();

            // Set up the mock DAOFactory to return the mock data services
            this.mockDAOFactory.Setup(factory => factory.SemesterDataServices).Returns(this.mockSemesterDataServices.Object);
            typeof(DAOFactoryMethod).GetField("CurrentDaoFactory", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic).SetValue(null, this.mockDAOFactory.Object);

            // Initialize a valid course entity
            this.testCourse = new Course
            {
                Id = 1,
                Name = "Advanced Programming",
                Description = "An advanced programming course.",
                Credits = 30,
                Cost = 500,
                MinThreshold = 10,
                MaxThreshold = 150,
                Prerequisites = new List<Course>(),
                AvailableSemesters = new List<Semester>(),
            };

            // Initialize a valid student entity
            this.testStudent = new Student
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Address = "123 Main St",
                NationalId = "1234567890123",
                Balance = 1000m,
                RegistrationNumber = "123456",
                CurrentSemesterId = 1,
                EnrolledCourses = new List<Course> { this.testCourse },
            };

            // Initialize a valid semester entity
            this.testSemester = new Semester
            {
                Id = 1,
                Number = 1,
                CreditThreshold = 30,
                AvailableCourses = new List<Course> { this.testCourse },
                Students = new List<Student> { this.testStudent },
            };
        }

        /// <summary>
        /// Tests adding a semester.
        /// </summary>
        [TestMethod]
        public void TestAddSemester()
        {
            // Act
            this.semesterService.AddSemester(this.testSemester);

            // Assert
            this.mockSemesterDataServices.Verify(repo => repo.AddSemester(this.testSemester), Times.Once);
        }

        /// <summary>
        /// Tests updating a semester.
        /// </summary>
        [TestMethod]
        public void TestUpdateSemester()
        {
            // Act
            this.semesterService.UpdateSemester(this.testSemester);

            // Assert
            this.mockSemesterDataServices.Verify(repo => repo.UpdateSemester(this.testSemester), Times.Once);
        }

        /// <summary>
        /// Tests deleting a semester.
        /// </summary>
        [TestMethod]
        public void TestDeleteSemester()
        {
            // Arrange
            this.mockSemesterDataServices.Setup(repo => repo.GetSemesterById(1)).Returns(this.testSemester);

            // Act
            this.semesterService.DeleteSemester(this.testSemester);

            // Assert
            this.mockSemesterDataServices.Verify(repo => repo.DeleteSemester(this.testSemester), Times.Once);
        }

        /// <summary>
        /// Tests deleting a semester with an invalid semester.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestDeleteSemester_InvalidSemester_ThrowsException()
        {
            // Act
            this.semesterService.DeleteSemester(null);
        }

        /// <summary>
        /// Tests getting a semester by ID.
        /// </summary>
        [TestMethod]
        public void TestGetSemesterById()
        {
            // Arrange
            this.mockSemesterDataServices.Setup(repo => repo.GetSemesterById(1)).Returns(this.testSemester);

            // Act
            var result = this.semesterService.GetSemesterById(1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Number);
        }

        /// <summary>
        /// Tests getting a semester by an invalid ID.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestGetSemesterById_InvalidId_ThrowsException()
        {
            // Act
            this.semesterService.GetSemesterById(-1);
        }

        /// <summary>
        /// Tests getting all semesters.
        /// </summary>
        [TestMethod]
        public void TestGetAllSemesters()
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

        /// <summary>
        /// Tests getting courses in a semester.
        /// </summary>
        [TestMethod]
        public void TestGetCoursesInSemester()
        {
            // Arrange
            this.mockSemesterDataServices.Setup(repo => repo.GetSemesterById(1)).Returns(this.testSemester);

            // Act
            var result = this.semesterService.GetCoursesInSemester(this.testSemester);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
        }

        /// <summary>
        /// Tests getting courses in a semester when there are no courses.
        /// </summary>
        [TestMethod]
        public void TestGetCoursesInSemester_NoCourses()
        {
            // Arrange
            this.testSemester.AvailableCourses = new List<Course>();
            this.mockSemesterDataServices.Setup(repo => repo.GetSemesterById(1)).Returns(this.testSemester);

            // Act
            var result = this.semesterService.GetCoursesInSemester(this.testSemester);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }

        /// <summary>
        /// Tests getting students in a semester.
        /// </summary>
        [TestMethod]
        public void TestGetStudentsInSemester()
        {
            // Arrange
            this.mockSemesterDataServices.Setup(repo => repo.GetSemesterById(1)).Returns(this.testSemester);

            // Act
            var result = this.semesterService.GetStudentsInSemester(this.testSemester);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
        }

        /// <summary>
        /// Tests getting students in a semester when there are no students.
        /// </summary>
        [TestMethod]
        public void TestGetStudentsInSemester_NoStudents()
        {
            // Arrange
            this.testSemester.Students = new List<Student>();
            this.mockSemesterDataServices.Setup(repo => repo.GetSemesterById(1)).Returns(this.testSemester);

            // Act
            var result = this.semesterService.GetStudentsInSemester(this.testSemester);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }

        /// <summary>
        /// Tests getting the next semester.
        /// </summary>
        [TestMethod]
        public void TestGetNextSemester()
        {
            // Arrange
            var nextSemester = new Semester { Id = 2, Number = 2, CreditThreshold = 30 };
            var semesters = new List<Semester> { this.testSemester, nextSemester };
            this.mockSemesterDataServices.Setup(repo => repo.GetAllSemesters()).Returns(semesters);

            // Act
            var result = this.semesterService.GetNextSemester(this.testSemester);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Number);
        }

        /// <summary>
        /// Tests getting the next semester with an invalid current semester.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestGetNextSemester_InvalidCurrentSemester_ThrowsException()
        {
            // Act
            this.semesterService.GetNextSemester(null);
        }

        /// <summary>
        /// Tests adding an invalid semester.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void TestAddSemester_InvalidSemester_ThrowsException()
        {
            // Arrange
            this.testSemester.CreditThreshold = -1;

            // Act
            this.semesterService.AddSemester(this.testSemester);
        }

        /// <summary>
        /// Tests updating an invalid semester.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void TestUpdateSemester_InvalidSemester_ThrowsException()
        {
            // Arrange
            this.testSemester.CreditThreshold = -1;

            // Act
            this.semesterService.UpdateSemester(this.testSemester);
        }
    }
}