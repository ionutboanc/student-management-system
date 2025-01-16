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
    using DomainModel;
    using DomainModel.Models;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using ServiceLayer.ServiceImplementation;
    using Utilities;

    /// <summary>
    /// Unit tests for the <see cref="ExamService"/> class.
    /// </summary>
    [TestClass]
    public class ExamServiceTests
    {
        /// <summary>
        /// Mock for exam data services.
        /// </summary>
        private Mock<IExamDataServices> mockExamDataServices;

        /// <summary>
        /// Mock for course data services.
        /// </summary>
        private Mock<ICourseDataServices> mockCourseDataServices;

        /// <summary>
        /// Mock for student data services.
        /// </summary>
        private Mock<IStudentDataServices> mockStudentDataServices;

        /// <summary>
        /// Mock for DAO factory.
        /// </summary>
        private Mock<IDAOFactory> mockDAOFactory;

        /// <summary>
        /// The exam service instance used for testing.
        /// </summary>
        private ExamService examService;

        /// <summary>
        /// The test exam instance.
        /// </summary>
        private Exam testExam;

        /// <summary>
        /// The test student instance.
        /// </summary>
        private Student testStudent;

        /// <summary>
        /// The test course instance.
        /// </summary>
        private Course testCourse;

        /// <summary>
        /// The test semester instance.
        /// </summary>
        private Semester testSemester;

        /// <summary>
        /// Sets up the test environment.
        /// </summary>
        [TestInitialize]
        public void SetUp()
        {
            this.mockExamDataServices = new Mock<IExamDataServices>();
            this.mockCourseDataServices = new Mock<ICourseDataServices>();
            this.mockStudentDataServices = new Mock<IStudentDataServices>();
            this.mockDAOFactory = new Mock<IDAOFactory>();
            this.examService = new ExamService();

            // Set up the mock DAOFactory to return the mock data services
            this.mockDAOFactory.Setup(factory => factory.ExamDataServices).Returns(this.mockExamDataServices.Object);
            this.mockDAOFactory.Setup(factory => factory.CourseDataServices).Returns(this.mockCourseDataServices.Object);
            this.mockDAOFactory.Setup(factory => factory.StudentDataServices).Returns(this.mockStudentDataServices.Object);
            typeof(DAOFactoryMethod).GetField("CurrentDaoFactory", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic).SetValue(null, this.mockDAOFactory.Object);

            // Initialize a valid exam entity
            this.testExam = new Exam
            {
                ExamId = 1,
                StudentId = 1,
                CourseId = 1,
                SemesterId = 1,
                Grade = 8,
                ExamDate = DateTime.Now.AddDays(1), // Future date
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
                EnrolledCourses = new List<Course>(),
            };

            // Initialize a valid course entity
            this.testCourse = new Course
            {
                Id = 1,
                Name = "Advanced Programming",
                Description = "An advanced programming course.",
                Credits = 5,
                Cost = 500,
                MinThreshold = 50,
                MaxThreshold = 150,
                Prerequisites = new List<Course>(),
                AvailableSemesters = new List<Semester>(),
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

            this.testExam.Course = this.testCourse;
            this.testExam.Student = this.testStudent;
            this.testExam.Semester = this.testSemester;
        }

        /// <summary>
        /// Tests adding a valid exam.
        /// </summary>
        [TestMethod]
        public void TestAddExam_ValidExam_AddsSuccessfully()
        {
            // Arrange
            var existingExams = new List<Exam>();
            this.mockExamDataServices.Setup(repo => repo.GetAllExams()).Returns(existingExams);

            // Act
            this.examService.AddExam(this.testExam);

            // Assert
            this.mockExamDataServices.Verify(repo => repo.AddExam(this.testExam), Times.Once);
        }

        /// <summary>
        /// Tests adding an exam on the same day as another exam.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void TestAddExam_ExamOnSameDay_ThrowsException()
        {
            // Arrange
            var existingExam = new Exam
            {
                ExamId = 2,
                StudentId = 1,
                CourseId = 2,
                SemesterId = 1,
                Grade = 7,
                ExamDate = this.testExam.ExamDate,
            };

            var exams = new List<Exam> { existingExam };
            this.mockExamDataServices.Setup(repo => repo.GetAllExams()).Returns(exams);

            // Act
            this.examService.AddExam(this.testExam);
        }

        /// <summary>
        /// Tests adding an exam that exceeds the maximum number of attempts.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void TestAddExam_ExceedsMaxAttempts_ThrowsException()
        {
            // Arrange
            var existingExams = new List<Exam>
            {
                new Exam { ExamId = 2, StudentId = 1, CourseId = 1, SemesterId = 1, Grade = 4, ExamDate = DateTime.Now.AddMonths(-4) },
                new Exam { ExamId = 3, StudentId = 1, CourseId = 1, SemesterId = 1, Grade = 3, ExamDate = DateTime.Now.AddMonths(-3) },
            };

            this.mockExamDataServices.Setup(repo => repo.GetAllExams()).Returns(existingExams);

            // Act
            this.examService.AddExam(this.testExam);
        }

        /// <summary>
        /// Tests adding an exam with a grade out of range.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void TestAddExam_GradeOutOfRange_ThrowsException()
        {
            // Arrange
            this.testExam.Grade = 11;

            // Act
            this.examService.AddExam(this.testExam);
        }

        /// <summary>
        /// Tests deleting an exam.
        /// </summary>
        [TestMethod]
        public void TestDeleteExam()
        {
            // Arrange
            this.mockExamDataServices.Setup(repo => repo.GetExamById(1)).Returns(this.testExam);

            // Act
            this.examService.DeleteExam(this.testExam);

            // Assert
            this.mockExamDataServices.Verify(repo => repo.DeleteExam(this.testExam), Times.Once);
        }

        /// <summary>
        /// Tests deleting an invalid exam.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestDeleteExam_InvalidExam_ThrowsException()
        {
            // Act
            this.examService.DeleteExam(null);
        }

        /// <summary>
        /// Tests getting an exam by ID.
        /// </summary>
        [TestMethod]
        public void TestGetExamById()
        {
            // Arrange
            this.mockExamDataServices.Setup(repo => repo.GetExamById(1)).Returns(this.testExam);

            // Act
            var result = this.examService.GetExamById(1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(8, result.Grade);
        }

        /// <summary>
        /// Tests getting an exam by an invalid ID.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestGetExamById_InvalidId_ThrowsException()
        {
            // Act
            this.examService.GetExamById(0);
        }

        /// <summary>
        /// Tests getting all exams.
        /// </summary>
        [TestMethod]
        public void TestGetAllExams()
        {
            // Arrange
            var exams = new List<Exam> { this.testExam };
            this.mockExamDataServices.Setup(repo => repo.GetAllExams()).Returns(exams);

            // Act
            var result = this.examService.GetAllExams();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
        }

        /// <summary>
        /// Tests getting all exams when there are no exams.
        /// </summary>
        [TestMethod]
        public void TestGetAllExams_NoExams()
        {
            // Arrange
            var exams = new List<Exam>();
            this.mockExamDataServices.Setup(repo => repo.GetAllExams()).Returns(exams);

            // Act
            var result = this.examService.GetAllExams();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }

        /// <summary>
        /// Tests getting exams by student.
        /// </summary>
        [TestMethod]
        public void TestGetExamsByStudent()
        {
            // Arrange
            var exams = new List<Exam> { this.testExam };
            this.mockExamDataServices.Setup(repo => repo.GetAllExams()).Returns(exams);

            // Act
            var result = this.examService.GetExamsByStudent(this.testStudent);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
        }

        /// <summary>
        /// Tests getting exams by student when the student has no exams.
        /// </summary>
        [TestMethod]
        public void TestGetExamsByStudent_NoExams()
        {
            // Arrange
            var exams = new List<Exam>();
            this.mockExamDataServices.Setup(repo => repo.GetAllExams()).Returns(exams);

            // Act
            var result = this.examService.GetExamsByStudent(this.testStudent);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }

        /// <summary>
        /// Tests getting exams by course.
        /// </summary>
        [TestMethod]
        public void TestGetExamsByCourse()
        {
            // Arrange
            var exams = new List<Exam> { this.testExam };
            this.mockExamDataServices.Setup(repo => repo.GetAllExams()).Returns(exams);

            // Act
            var result = this.examService.GetExamsByCourse(this.testCourse);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
        }

        /// <summary>
        /// Tests getting exams by course when there are no exams.
        /// </summary>
        [TestMethod]
        public void TestGetExamsByCourse_NoExams()
        {
            // Arrange
            var exams = new List<Exam>();
            this.mockExamDataServices.Setup(repo => repo.GetAllExams()).Returns(exams);

            // Act
            var result = this.examService.GetExamsByCourse(this.testCourse);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }

        /// <summary>
        /// Tests adding an exam when the student has too many attempts, and refunds the balance.
        /// </summary>
        [TestMethod]
        public void TestAddExam_TooManyAttempts_RefundsBalance()
        {
            // Arrange
            var existingExams = new List<Exam>
            {
                new Exam { ExamId = 2, StudentId = 1, CourseId = 1, SemesterId = 1, Grade = 4, ExamDate = DateTime.Now.AddMonths(-4) },
                new Exam { ExamId = 3, StudentId = 1, CourseId = 1, SemesterId = 1, Grade = 3, ExamDate = DateTime.Now.AddMonths(-3) },
            };

            this.mockExamDataServices.Setup(repo => repo.GetAllExams()).Returns(existingExams);
            this.testCourse.BalanceRefunded = false;

            // Act
            try
            {
                this.examService.AddExam(this.testExam);
            }
            catch (ValidationException ex)
            {
                Assert.AreEqual($"The student has already taken the exam for this course {Constants.MaxNumberOfAttempts} times.", ex.Message);
            }

            // Assert
            Assert.IsTrue(this.testCourse.BalanceRefunded);
            Assert.AreEqual(1250m, this.testStudent.Balance); // 1000m + 50% of 500m
            this.mockCourseDataServices.Verify(repo => repo.UpdateCourse(this.testCourse), Times.Once);
            this.mockStudentDataServices.Verify(repo => repo.UpdateStudent(this.testStudent), Times.Once);
        }

        /// <summary>
        /// Tests adding an exam when the student has too many attempts, and ensures no double refund.
        /// </summary>
        [TestMethod]
        public void TestAddExam_TooManyAttempts_NoDoubleRefund()
        {
            // Arrange
            var existingExams = new List<Exam>
            {
                new Exam { ExamId = 2, StudentId = 1, CourseId = 1, SemesterId = 1, Grade = 4, ExamDate = DateTime.Now.AddMonths(-4) },
                new Exam { ExamId = 3, StudentId = 1, CourseId = 1, SemesterId = 1, Grade = 3, ExamDate = DateTime.Now.AddMonths(-3) },
            };

            this.mockExamDataServices.Setup(repo => repo.GetAllExams()).Returns(existingExams);
            this.testCourse.BalanceRefunded = true;

            // Act
            try
            {
                this.examService.AddExam(this.testExam);
            }
            catch (ValidationException ex)
            {
                Assert.AreEqual($"The student has already taken the exam for this course {Constants.MaxNumberOfAttempts} times.", ex.Message);
            }

            // Assert
            Assert.IsTrue(this.testCourse.BalanceRefunded);
            Assert.AreEqual(1000m, this.testStudent.Balance); // No additional refund
            this.mockCourseDataServices.Verify(repo => repo.UpdateCourse(this.testCourse), Times.Never);
            this.mockStudentDataServices.Verify(repo => repo.UpdateStudent(this.testStudent), Times.Never);
        }

        /// <summary>
        /// Tests updating an exam when the student has too many attempts, and refunds the balance.
        /// </summary>
        [TestMethod]
        public void TestUpdateExam_TooManyAttempts_RefundsBalance()
        {
            // Arrange
            var existingExams = new List<Exam>
            {
                new Exam { ExamId = 2, StudentId = 1, CourseId = 1, SemesterId = 1, Grade = 4, ExamDate = DateTime.Now.AddMonths(-4) },
                new Exam { ExamId = 3, StudentId = 1, CourseId = 1, SemesterId = 1, Grade = 3, ExamDate = DateTime.Now.AddMonths(-3) },
            };

            this.mockExamDataServices.Setup(repo => repo.GetAllExams()).Returns(existingExams);
            this.testCourse.BalanceRefunded = false;

            // Act
            try
            {
                this.examService.UpdateExam(this.testExam);
            }
            catch (ValidationException ex)
            {
                Assert.AreEqual($"The student has already taken the exam for this course {Constants.MaxNumberOfAttempts} times.", ex.Message);
            }

            // Assert
            Assert.IsTrue(this.testCourse.BalanceRefunded);
            Assert.AreEqual(1250m, this.testStudent.Balance); // 1000m + 50% of 500m
            this.mockCourseDataServices.Verify(repo => repo.UpdateCourse(this.testCourse), Times.Once);
            this.mockStudentDataServices.Verify(repo => repo.UpdateStudent(this.testStudent), Times.Once);
        }

        /// <summary>
        /// Tests updating an exam when the student has too many attempts, and ensures no double refund.
        /// </summary>
        [TestMethod]
        public void TestUpdateExam_TooManyAttempts_NoDoubleRefund()
        {
            // Arrange
            var existingExams = new List<Exam>
            {
                new Exam { ExamId = 2, StudentId = 1, CourseId = 1, SemesterId = 1, Grade = 4, ExamDate = DateTime.Now.AddMonths(-4) },
                new Exam { ExamId = 3, StudentId = 1, CourseId = 1, SemesterId = 1, Grade = 3, ExamDate = DateTime.Now.AddMonths(-3) },
            };

            this.mockExamDataServices.Setup(repo => repo.GetAllExams()).Returns(existingExams);
            this.testCourse.BalanceRefunded = true;

            // Act
            try
            {
                this.examService.UpdateExam(this.testExam);
            }
            catch (ValidationException ex)
            {
                Assert.AreEqual($"The student has already taken the exam for this course {Constants.MaxNumberOfAttempts} times.", ex.Message);
            }

            // Assert
            Assert.IsTrue(this.testCourse.BalanceRefunded);
            Assert.AreEqual(1000m, this.testStudent.Balance); // No additional refund
            this.mockCourseDataServices.Verify(repo => repo.UpdateCourse(this.testCourse), Times.Never);
            this.mockStudentDataServices.Verify(repo => repo.UpdateStudent(this.testStudent), Times.Never);
        }
    }
}