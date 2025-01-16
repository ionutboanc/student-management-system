using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using DataMapper;
using DomainModel;
using DomainModel.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ServiceLayer;

namespace TestServiceLayer
{
    /// <summary>
    /// Unit tests for the CourseService class.
    /// </summary>
    [TestClass]
    public class CourseServiceTests
    {
        private Mock<ICourseDataServices> mockDataServices;
        private Mock<IStudentDataServices> mockStudentDataServices;
        private Mock<IDAOFactory> mockDAOFactory;
        private CourseService courseService;
        private Course testCourse;
        private Discount testDiscount;
        private Student testStudent;
        private Exam testExam;
        private Semester testSemester;

        /// <summary>
        /// Sets up the test environment.
        /// </summary>
        [TestInitialize]
        public void SetUp()
        {
            this.mockDataServices = new Mock<ICourseDataServices>();
            this.mockStudentDataServices = new Mock<IStudentDataServices>();
            this.mockDAOFactory = new Mock<IDAOFactory>();
            this.courseService = new CourseService();

            // Set up the mock DAOFactory to return the mock data services
            this.mockDAOFactory.Setup(factory => factory.CourseDataServices).Returns(this.mockDataServices.Object);
            this.mockDAOFactory.Setup(factory => factory.StudentDataServices).Returns(this.mockStudentDataServices.Object);
            typeof(DAOFactoryMethod).GetField("_currentDAOFactory", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic).SetValue(null, this.mockDAOFactory.Object);

            this.testDiscount = new Discount { Percentage = 10 };

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
                DiscountId = 0,
                Discount = this.testDiscount,
                Prerequisites = new List<Course>(),
                AvailableSemesters = new List<Semester>(),
                Exams = new List<Exam>(),
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

            // Initialize a valid exam entity
            this.testExam = new Exam
            {
                ExamId = 1,
                StudentId = 1,
                CourseId = 1,
                Grade = 8,
                ExamDate = DateTime.Now,
                Student = this.testStudent,
                Course = this.testCourse,
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

            this.testCourse.Exams.Add(this.testExam);
        }

        /// <summary>
        /// Tests adding a course.
        /// </summary>
        [TestMethod]
        public void TestAddCourse()
        {
            // Act
            this.courseService.AddCourse(this.testCourse);

            // Assert
            this.mockDataServices.Verify(repo => repo.AddCourse(this.testCourse), Times.Once);
        }

        /// <summary>
        /// Tests updating a course.
        /// </summary>
        [TestMethod]
        public void TestUpdateCourse()
        {
            // Act
            this.courseService.UpdateCourse(this.testCourse);

            // Assert
            this.mockDataServices.Verify(repo => repo.UpdateCourse(this.testCourse), Times.Once);
        }

        /// <summary>
        /// Tests deleting a course.
        /// </summary>
        [TestMethod]
        public void TestDeleteCourse()
        {
            // Arrange
            this.mockDataServices.Setup(repo => repo.GetCourseById(1)).Returns(this.testCourse);

            // Act
            this.courseService.DeleteCourse(this.testCourse);

            // Assert
            this.mockDataServices.Verify(repo => repo.DeleteCourse(this.testCourse), Times.Once);
        }

        /// <summary>
        /// Tests deleting a course with an invalid ID.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestDeleteCourse_InvalidCourse_ThrowsException()
        {
            // Act
            this.courseService.DeleteCourse(null);
        }

        /// <summary>
        /// Tests getting a course by ID.
        /// </summary>
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

        /// <summary>
        /// Tests getting a course by an invalid ID.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestGetCourseById_InvalidId_ThrowsException()
        {
            // Act
            this.courseService.GetCourseById(0);
        }

        /// <summary>
        /// Tests getting all courses.
        /// </summary>
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

        /// <summary>
        /// Tests getting prerequisites for a course.
        /// </summary>
        [TestMethod]
        public void TestGetPrerequisites()
        {
            // Arrange
            var prerequisite = new Course
            {
                Id = 2,
                Name = "Introduction to Programming",
                Description = "An introductory programming course.",
                Credits = 3,
                Cost = 300,
                MinThreshold = 30,
                MaxThreshold = 90,
            };
            this.testCourse.Prerequisites.Add(prerequisite);
            this.mockDataServices.Setup(repo => repo.GetCourseById(1)).Returns(this.testCourse);

            // Act
            var result = this.courseService.GetPrerequisites(this.testCourse);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
        }

        /// <summary>
        /// Tests getting enrolled students for a course.
        /// </summary>
        [TestMethod]
        public void TestGetEnrolledStudents()
        {
            // Arrange
            this.mockDataServices.Setup(repo => repo.GetCourseById(1)).Returns(this.testCourse);

            // Act
            var result = this.courseService.GetEnrolledStudents(this.testCourse);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
        }

        /// <summary>
        /// Tests getting exams for a course.
        /// </summary>
        [TestMethod]
        public void TestGetExamsForCourse()
        {
            // Arrange
            this.mockDataServices.Setup(repo => repo.GetCourseById(1)).Returns(this.testCourse);

            // Act
            var result = this.courseService.GetExamsForCourse(this.testCourse);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
        }

        /// <summary>
        /// Tests getting semesters for a course.
        /// </summary>
        [TestMethod]
        public void TestGetSemestersForCourse()
        {
            // Arrange
            this.testCourse.AvailableSemesters.Add(this.testSemester);
            this.mockDataServices.Setup(repo => repo.GetCourseById(1)).Returns(this.testCourse);

            // Act
            var result = this.courseService.GetSemestersForCourse(this.testCourse);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
        }

        /// <summary>
        /// Tests calculating the average grades above five.
        /// </summary>
        [TestMethod]
        public void TestCalculateAverageGradesAboveFive()
        {
            // Arrange
            var courses = new List<Course> { this.testCourse };
            this.mockDataServices.Setup(repo => repo.GetAllCourses()).Returns(courses);

            // Act
            var result = this.courseService.CalculateAverageGradesAboveFive(this.testCourse);

            // Assert
            Assert.AreEqual(8, result);
        }

        /// <summary>
        /// Tests calculating the average number of courses per student.
        /// </summary>
        [TestMethod]
        public void TestCalculateAverageNumberOfCoursesPerStudent()
        {
            // Arrange
            var students = new List<Student> { this.testStudent };
            this.mockStudentDataServices.Setup(repo => repo.GetAllStudents()).Returns(students);

            // Act
            var result = this.courseService.CalculateAverageNumberOfCoursesPerStudent();

            // Assert
            Assert.AreEqual(1, result);
        }

        /// <summary>
        /// Tests getting the number of enrolled students for a course.
        /// </summary>
        [TestMethod]
        public void TestGetNumberOfEnrolledStudents()
        {
            // Arrange
            this.mockDataServices.Setup(repo => repo.GetCourseById(1)).Returns(this.testCourse);

            // Act
            var result = this.courseService.GetNumberOfEnrolledStudents(this.testCourse);

            // Assert
            Assert.AreEqual(1, result);
        }

        /// <summary>
        /// Tests adding an invalid course.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void TestAddCourse_InvalidCourse_ThrowsException()
        {
            // Arrange
            this.testCourse.Cost = -1;

            // Act
            this.courseService.AddCourse(this.testCourse);
        }

        /// <summary>
        /// Tests updating an invalid course.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void TestUpdateCourse_InvalidCourse_ThrowsException()
        {
            // Arrange
            this.testCourse.Cost = -1;

            // Act
            this.courseService.UpdateCourse(this.testCourse);
        }

        /// <summary>
        /// Tests calculating the average grades above five with no grades above five.
        /// </summary>
        [TestMethod]
        public void TestCalculateAverageGradesAboveFive_NoGradesAboveFive()
        {
            // Arrange
            this.testExam.Grade = 4; // Set grade below 5
            var courses = new List<Course> { this.testCourse };
            this.mockDataServices.Setup(repo => repo.GetAllCourses()).Returns(courses);

            // Act
            var result = this.courseService.CalculateAverageGradesAboveFive(this.testCourse);

            // Assert
            Assert.AreEqual(0.0, result);
        }

        /// <summary>
        /// Tests calculating the average grades above five with multiple grades.
        /// </summary>
        [TestMethod]
        public void TestCalculateAverageGradesAboveFive_MultipleGrades()
        {
            // Arrange
            var exam2 = new Exam
            {
                ExamId = 2,
                StudentId = 1,
                CourseId = 1,
                Grade = 6,
                ExamDate = DateTime.Now,
                Student = this.testStudent,
                Course = this.testCourse,
            };
            this.testCourse.Exams.Add(exam2);
            var courses = new List<Course> { this.testCourse };
            this.mockDataServices.Setup(repo => repo.GetAllCourses()).Returns(courses);

            // Act
            var result = this.courseService.CalculateAverageGradesAboveFive(this.testCourse);

            // Assert
            Assert.AreEqual(7, result); // Average of 8 and 6
        }

        /// <summary>
        /// Tests calculating the average number of courses per student with no students.
        /// </summary>
        [TestMethod]
        public void TestCalculateAverageNumberOfCoursesPerStudent_NoStudents()
        {
            // Arrange
            var students = new List<Student>();
            this.mockStudentDataServices.Setup(repo => repo.GetAllStudents()).Returns(students);

            // Act
            var result = this.courseService.CalculateAverageNumberOfCoursesPerStudent();

            // Assert
            Assert.AreEqual(0.0, result);
        }

        /// <summary>
        /// Tests calculating the average number of courses per student with multiple students.
        /// </summary>
        [TestMethod]
        public void TestCalculateAverageNumberOfCoursesPerStudent_MultipleStudents()
        {
            // Arrange
            var student2 = new Student
            {
                Id = 2,
                FirstName = "Jane",
                LastName = "Doe",
                Address = "456 Main St",
                NationalId = "9876543210987",
                Balance = 1000m,
                RegistrationNumber = "654321",
                CurrentSemesterId = 1,
                EnrolledCourses = new List<Course> { this.testCourse },
            };
            var students = new List<Student> { this.testStudent, student2 };
            this.mockStudentDataServices.Setup(repo => repo.GetAllStudents()).Returns(students);

            // Act
            var result = this.courseService.CalculateAverageNumberOfCoursesPerStudent();

            // Assert
            Assert.AreEqual(1.0, result); // Each student is enrolled in 1 course
        }

        /// <summary>
        /// Tests getting the number of enrolled students for a course with no students.
        /// </summary>
        [TestMethod]
        public void TestGetNumberOfEnrolledStudents_NoStudents()
        {
            // Arrange
            this.testCourse.Exams.Clear(); // No exams, hence no students
            this.mockDataServices.Setup(repo => repo.GetCourseById(1)).Returns(this.testCourse);

            // Act
            var result = this.courseService.GetNumberOfEnrolledStudents(this.testCourse);

            // Assert
            Assert.AreEqual(0, result);
        }

        /// <summary>
        /// Tests getting the number of enrolled students for a course with multiple students.
        /// </summary>
        [TestMethod]
        public void TestGetNumberOfEnrolledStudents_MultipleStudents()
        {
            // Arrange
            var student2 = new Student
            {
                Id = 2,
                FirstName = "Jane",
                LastName = "Doe",
                Address = "456 Main St",
                NationalId = "9876543210987",
                Balance = 1000m,
                RegistrationNumber = "654321",
                CurrentSemesterId = 1,
            };
            var exam2 = new Exam
            {
                ExamId = 2,
                StudentId = 2,
                CourseId = 1,
                Grade = 8,
                ExamDate = DateTime.Now,
                Student = student2,
                Course = this.testCourse,
            };
            this.testCourse.Exams.Add(exam2);
            this.mockDataServices.Setup(repo => repo.GetCourseById(1)).Returns(this.testCourse);

            // Act
            var result = this.courseService.GetNumberOfEnrolledStudents(this.testCourse);

            // Assert
            Assert.AreEqual(2, result); // Two students enrolled
        }

        /// <summary>
        /// Tests validating a course with circular dependencies.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestValidateCourse_CircularDependencies_ThrowsException()
        {
            // Arrange
            var prerequisite = new Course
            {
                Id = 2,
                Name = "Introduction to Programming",
                Description = "An introductory programming course.",
                Credits = 3,
                Cost = 300,
                MinThreshold = 30,
                MaxThreshold = 90,
                Prerequisites = new List<Course> { this.testCourse } 
            };
            this.testCourse.Prerequisites.Add(prerequisite);

            // Act
            this.courseService.ValidateCourse(this.testCourse);
        }

        /// <summary>
        /// Tests validating a course with no circular dependencies.
        /// </summary>
        [TestMethod]
        public void TestValidateCourse_NoCircularDependencies()
        {
            // Arrange
            var prerequisite = new Course
            {
                Id = 2,
                Name = "Introduction to Programming",
                Description = "An introductory programming course.",
                Credits = 3,
                Cost = 300,
                MinThreshold = 30,
                MaxThreshold = 90,
            };
            this.testCourse.Prerequisites.Add(prerequisite);

            // Act
            this.courseService.ValidateCourse(this.testCourse);

            // Assert
            Assert.IsTrue(true); // No exception thrown
        }

        /// <summary>
        /// Tests that a course can appear in multiple semesters.
        /// </summary>
        [TestMethod]
        public void TestCourseInMultipleSemesters()
        {
            // Arrange
            var semester2 = new Semester
            {
                Id = 2,
                Number = 2,
                CreditThreshold = 30,
                AvailableCourses = new List<Course> { this.testCourse },
                Students = new List<Student> { this.testStudent },
            };
            this.testCourse.AvailableSemesters.Add(this.testSemester);
            this.testCourse.AvailableSemesters.Add(semester2);
            this.mockDataServices.Setup(repo => repo.GetCourseById(1)).Returns(this.testCourse);

            // Act
            var result = this.courseService.GetSemestersForCourse(this.testCourse);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
        }

        /// <summary>
        /// Tests that a course can appear in multiple semesters and retrieves the correct semesters.
        /// </summary>
        [TestMethod]
        public void TestCourseInMultipleSemesters_RetrievesCorrectSemesters()
        {
            // Arrange
            var semester2 = new Semester
            {
                Id = 2,
                Number = 2,
                CreditThreshold = 30,
                AvailableCourses = new List<Course> { this.testCourse },
                Students = new List<Student> { this.testStudent },
            };
            this.testCourse.AvailableSemesters.Add(this.testSemester);
            this.testCourse.AvailableSemesters.Add(semester2);
            this.mockDataServices.Setup(repo => repo.GetCourseById(1)).Returns(this.testCourse);

            // Act
            var result = this.courseService.GetSemestersForCourse(this.testCourse);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.Any(s => s.Id == 1));
            Assert.IsTrue(result.Any(s => s.Id == 2));
        }
    }
}