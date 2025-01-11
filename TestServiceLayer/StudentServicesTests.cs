using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using DomainModel.Models;
using Moq;
using DataMapper;
using ServiceLayer;
using System.ComponentModel.DataAnnotations;

[TestClass]
public class StudentServiceTests
{
    private Mock<IStudentDataServices> mockStudentDataServices;
    private Mock<IDAOFactory> mockDAOFactory;
    private StudentService studentService;
    private Student testStudent;
    private Course testCourse;
    private Semester testSemester;

    [TestInitialize]
    public void SetUp()
    {
        this.mockStudentDataServices = new Mock<IStudentDataServices>();
        this.mockDAOFactory = new Mock<IDAOFactory>();
        this.studentService = new StudentService();

        this.mockDAOFactory.Setup(factory => factory.StudentDataServices).Returns(this.mockStudentDataServices.Object);
        typeof(DAOFactoryMethod).GetField("_currentDAOFactory", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic).SetValue(null, this.mockDAOFactory.Object);

        this.testCourse = new Course
        {
            Name = "Advanced Programming",
            Description = "An advanced programming course.",
            Credits = 5,
            Cost = 500,
            MinThreshold = 50,
            MaxThreshold = 150,
        };

        this.testSemester = new Semester
        {
            Id = 1,
            Number = 1,
            CreditThreshold = 15,
        };


    }

    [TestMethod]
    public void TestAddStudent_ValidStudent_AddsSuccessfully()
    {
        // Act
        studentService.AddStudent(testStudent);

        // Assert
        mockStudentDataServices.Verify(m => m.AddStudent(testStudent), Times.Once);
    }

    [TestMethod]
    [ExpectedException(typeof(ValidationException))]
    public void TestAddStudent_InvalidStudent_ThrowsValidationException()
    {
        // Arrange
        testStudent.Name = null; // Invalid student

        // Act
        studentService.AddStudent(testStudent);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void TestGetStudentById_InvalidId_ThrowsException()
    {
        // Act
        studentService.GetStudentById(0);
    }

    [TestMethod]
    public void TestEnrollStudentInCourse_ValidEnrollment_AddsSuccessfully()
    {
        // Arrange
        mockStudentDataServices.Setup(m => m.GetStudentById(It.IsAny<int>())).Returns(testStudent);
        mockCourseDataServices.Setup(m => m.GetCourseById(It.IsAny<int>())).Returns(testCourse);
        mockSemesterDataServices.Setup(m => m.GetSemesterById(It.IsAny<int>())).Returns(testSemester);

        // Act
        studentService.EnrollStudentInCourse(1, 101, 1);

        // Assert
        Assert.AreEqual(1, testStudent.Enrollments.Count);
        Assert.AreEqual(101, testStudent.Enrollments[0].CourseId);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void TestEnrollStudentInCourse_AlreadyEnrolled_ThrowsException()
    {
        // Arrange
        testStudent.Enrollments.Add(new Enrollment { CourseId = 101 });
        mockStudentDataServices.Setup(m => m.GetStudentById(It.IsAny<int>())).Returns(testStudent);
        mockCourseDataServices.Setup(m => m.GetCourseById(It.IsAny<int>())).Returns(testCourse);
        mockSemesterDataServices.Setup(m => m.GetSemesterById(It.IsAny<int>())).Returns(testSemester);

        // Act
        studentService.EnrollStudentInCourse(1, 101, 1);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void TestRecordExamGrade_MaxAttemptsReached_ThrowsException()
    {
        // Arrange
        var enrollment = new Enrollment { CourseId = 101, NumberOfAttempts = 3 };
        testStudent.Enrollments.Add(enrollment);
        mockStudentDataServices.Setup(m => m.GetStudentById(It.IsAny<int>())).Returns(testStudent);

        // Act
        studentService.RecordExamGrade(1, 101, 5);
    }

    [TestMethod]
    public void TestGetTotalCredits_ValidCredits_ReturnsTotal()
    {
        // Arrange
        var enrollment = new Enrollment { CourseId = 101, SemesterId = 1, Grade = 5, Course = testCourse };
        testStudent.Enrollments.Add(enrollment);
        mockStudentDataServices.Setup(m => m.GetStudentById(It.IsAny<int>())).Returns(testStudent);

        // Act
        var totalCredits = studentService.GetTotalCredits(1, 1);

        // Assert
        Assert.AreEqual(3, totalCredits);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void TestPromoteStudent_InsufficientCredits_ThrowsException()
    {
        // Arrange
        var enrollment = new Enrollment { CourseId = 101, SemesterId = 1, Grade = 5, Course = testCourse };
        testStudent.Enrollments.Add(enrollment);
        mockStudentDataServices.Setup(m => m.GetStudentById(It.IsAny<int>())).Returns(testStudent);
        mockSemesterDataServices.Setup(m => m.GetSemesterById(It.IsAny<int>())).Returns(new Semester
        {
            Id = 2,
            CreditThreshold = 10
        });

        // Act
        studentService.PromoteStudent(1);
    }
}
