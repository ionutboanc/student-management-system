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

namespace TestServiceLayer
{
    [TestClass]
    public class ExamServiceTests
    {
        private Mock<IExamDataServices> mockExamDataServices;
        private Mock<ICourseDataServices> mockCourseDataServices;
        private Mock<IStudentDataServices> mockStudentDataServices;
        private Mock<IDAOFactory> mockDAOFactory;
        private ExamService examService;
        private Exam testExam;
        private Student testStudent;
        private Course testCourse;
        private Semester testSemester;

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
            typeof(DAOFactoryMethod).GetField("_currentDAOFactory", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic).SetValue(null, this.mockDAOFactory.Object);

            // Initialize a valid exam entity
            this.testExam = new Exam
            {
                ExamId = 1,
                StudentId = 1,
                CourseId = 1,
                SemesterId = 1,
                Grade = 8,
                ExamDate = DateTime.Now.AddDays(1) // Future date
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
                EnrolledCourses = new List<Course>()
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
                AvailableSemesters = new List<Semester>()
            };

            this.testSemester = new Semester
            {
                Id = 1,
                Number = 1,
                CreditThreshold = 30,
                AvailableCourses = new List<Course> { this.testCourse },
                Students = new List<Student> { this.testStudent }
            };

            this.testExam.Course = this.testCourse;
            this.testExam.Student = this.testStudent;
            this.testExam.Semester = this.testSemester;
        }

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
                ExamDate = this.testExam.ExamDate
            };

            var exams = new List<Exam> { existingExam };
            this.mockExamDataServices.Setup(repo => repo.GetAllExams()).Returns(exams);

            // Act
            this.examService.AddExam(this.testExam);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void TestAddExam_ExceedsMaxAttempts_ThrowsException()
        {
            // Arrange
            var existingExams = new List<Exam>
            {
                new Exam { ExamId = 2, StudentId = 1, CourseId = 1, SemesterId = 1, Grade = 4, ExamDate = DateTime.Now.AddMonths(-4) },
                new Exam { ExamId = 3, StudentId = 1, CourseId = 1, SemesterId = 1, Grade = 3, ExamDate = DateTime.Now.AddMonths(-3) }
            };

            this.mockExamDataServices.Setup(repo => repo.GetAllExams()).Returns(existingExams);

            // Act
            this.examService.AddExam(this.testExam);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void TestAddExam_GradeOutOfRange_ThrowsException()
        {
            // Arrange
            this.testExam.Grade = 11;

            // Act
            this.examService.AddExam(this.testExam);
        }

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

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestDeleteExam_InvalidExam_ThrowsException()
        {
            // Act
            this.examService.DeleteExam(null);
        }

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

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestGetExamById_InvalidId_ThrowsException()
        {
            // Act
            this.examService.GetExamById(0);
        }

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

        [TestMethod]
        public void TestAddExam_TooManyAttempts_RefundsBalance()
        {
            // Arrange
            var existingExams = new List<Exam>
            {
                new Exam { ExamId = 2, StudentId = 1, CourseId = 1, SemesterId = 1, Grade = 4, ExamDate = DateTime.Now.AddMonths(-4) },
                new Exam { ExamId = 3, StudentId = 1, CourseId = 1, SemesterId = 1, Grade = 3, ExamDate = DateTime.Now.AddMonths(-3) }
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

        [TestMethod]
        public void TestAddExam_TooManyAttempts_NoDoubleRefund()
        {
            // Arrange
            var existingExams = new List<Exam>
            {
                new Exam { ExamId = 2, StudentId = 1, CourseId = 1, SemesterId = 1, Grade = 4, ExamDate = DateTime.Now.AddMonths(-4) },
                new Exam { ExamId = 3, StudentId = 1, CourseId = 1, SemesterId = 1, Grade = 3, ExamDate = DateTime.Now.AddMonths(-3) }
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

        [TestMethod]
        public void TestUpdateExam_TooManyAttempts_RefundsBalance()
        {
            // Arrange
            var existingExams = new List<Exam>
            {
                new Exam { ExamId = 2, StudentId = 1, CourseId = 1, SemesterId = 1, Grade = 4, ExamDate = DateTime.Now.AddMonths(-4) },
                new Exam { ExamId = 3, StudentId = 1, CourseId = 1, SemesterId = 1, Grade = 3, ExamDate = DateTime.Now.AddMonths(-3) }
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

        [TestMethod]
        public void TestUpdateExam_TooManyAttempts_NoDoubleRefund()
        {
            // Arrange
            var existingExams = new List<Exam>
    {
        new Exam { ExamId = 2, StudentId = 1, CourseId = 1, SemesterId = 1, Grade = 4, ExamDate = DateTime.Now.AddMonths(-4) },
        new Exam { ExamId = 3, StudentId = 1, CourseId = 1, SemesterId = 1, Grade = 3, ExamDate = DateTime.Now.AddMonths(-3) }
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
                // Assert
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