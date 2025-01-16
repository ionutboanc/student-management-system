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
    using ServiceLayer;

    /// <summary>
    /// Unit tests for the <see cref="StudentService"/> class.
    /// </summary>
    [TestClass]
    public class StudentServiceTests
    {
        /// <summary>
        /// Mock for student data services.
        /// </summary>
        private Mock<IStudentDataServices> mockStudentDataServices;

        /// <summary>
        /// Mock for course data services.
        /// </summary>
        private Mock<ICourseDataServices> mockCourseDataServices;

        /// <summary>
        /// Mock for semester data services.
        /// </summary>
        private Mock<ISemesterDataServices> mockSemesterDataServices;

        /// <summary>
        /// Mock for DAO factory.
        /// </summary>
        private Mock<IDAOFactory> mockDAOFactory;

        /// <summary>
        /// The student service instance used for testing.
        /// </summary>
        private StudentService studentService;

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
            this.mockStudentDataServices = new Mock<IStudentDataServices>();
            this.mockCourseDataServices = new Mock<ICourseDataServices>();
            this.mockSemesterDataServices = new Mock<ISemesterDataServices>();
            this.mockDAOFactory = new Mock<IDAOFactory>();
            this.studentService = new StudentService();

            // Set up the mock DAOFactory to return the mock data services
            this.mockDAOFactory.Setup(factory => factory.StudentDataServices).Returns(this.mockStudentDataServices.Object);
            this.mockDAOFactory.Setup(factory => factory.CourseDataServices).Returns(this.mockCourseDataServices.Object);
            this.mockDAOFactory.Setup(factory => factory.SemesterDataServices).Returns(this.mockSemesterDataServices.Object);
            typeof(DAOFactoryMethod).GetField("CurrentDaoFactory", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic).SetValue(null, this.mockDAOFactory.Object);

            // Initialize a valid student entity
            this.testStudent = new Student
            {
                Id = 1,
                FirstName = "Ionut",
                LastName = "Boanc",
                Address = "123 Main St",
                NationalId = "1234567890123",
                Balance = 1000m,
                RegistrationNumber = "123456",
                CurrentSemesterId = 1,
                EnrolledCourses = new List<Course>(),
            };

            this.testStudent.AddEmail("ionut@mail.com");

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
                CreditThreshold = 4,
                AvailableCourses = new List<Course> { this.testCourse },
                Students = new List<Student> { this.testStudent },
            };
        }

        /// <summary>
        /// Tests adding a student.
        /// </summary>
        [TestMethod]
        public void TestAddStudent()
        {
            // Act
            this.studentService.AddStudent(this.testStudent);

            // Assert
            this.mockStudentDataServices.Verify(repo => repo.AddStudent(this.testStudent), Times.Once);
        }

        /// <summary>
        /// Tests updating a student.
        /// </summary>
        [TestMethod]
        public void TestUpdateStudent()
        {
            // Act
            this.studentService.UpdateStudent(this.testStudent);

            // Assert
            this.mockStudentDataServices.Verify(repo => repo.UpdateStudent(this.testStudent), Times.Once);
        }

        /// <summary>
        /// Tests deleting a student.
        /// </summary>
        [TestMethod]
        public void TestDeleteStudent()
        {
            // Act
            this.studentService.DeleteStudent(this.testStudent);

            // Assert
            this.mockStudentDataServices.Verify(repo => repo.DeleteStudent(this.testStudent), Times.Once);
        }

        /// <summary>
        /// Tests deleting a student with a null student.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestDeleteStudent_Null_ThrowsException()
        {
            // Act
            this.studentService.DeleteStudent(null);
        }

        /// <summary>
        /// Tests getting a student by ID.
        /// </summary>
        [TestMethod]
        public void TestGetStudentById()
        {
            // Arrange
            this.mockStudentDataServices.Setup(repo => repo.GetStudentById(1)).Returns(this.testStudent);

            // Act
            var result = this.studentService.GetStudentById(1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Ionut", result.FirstName);
        }

        /// <summary>
        /// Tests getting a student by an invalid ID.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestGetStudentById_InvalidId_ThrowsException()
        {
            // Act
            this.studentService.GetStudentById(-1);
        }

        /// <summary>
        /// Tests getting all students.
        /// </summary>
        [TestMethod]
        public void TestGetAllStudents()
        {
            // Arrange
            var students = new List<Student> { this.testStudent };
            this.mockStudentDataServices.Setup(repo => repo.GetAllStudents()).Returns(students);

            // Act
            var result = this.studentService.GetAllStudents();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
        }

        /// <summary>
        /// Tests getting a student by national ID.
        /// </summary>
        [TestMethod]
        public void TestGetStudentByNationalID()
        {
            // Arrange
            this.mockStudentDataServices.Setup(repo => repo.GetAllStudents()).Returns(new List<Student> { this.testStudent });

            // Act
            var result = this.studentService.GetStudentByNationalID("1234567890123");

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Ionut", result.FirstName);
        }

        /// <summary>
        /// Tests getting enrolled courses for a student.
        /// </summary>
        [TestMethod]
        public void TestGetEnrolledCourses()
        {
            // Arrange
            this.testStudent.EnrolledCourses.Add(this.testCourse);

            // Act
            var result = this.studentService.GetEnrolledCourses(this.testStudent);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
        }

        /// <summary>
        /// Tests getting exams for a student.
        /// </summary>
        [TestMethod]
        public void TestGetExamsForStudent()
        {
            // Arrange
            var exam = new Exam
            {
                ExamId = 1,
                StudentId = 1,
                CourseId = 1,
                Grade = 8,
                ExamDate = DateTime.Now,
                Student = this.testStudent,
                Course = this.testCourse,
            };
            this.testCourse.Exams.Add(exam);
            this.testStudent.EnrolledCourses.Add(this.testCourse);

            // Act
            var result = this.studentService.GetExamsForStudent(this.testStudent);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
        }

        /// <summary>
        /// Tests getting the current semester for a student.
        /// </summary>
        [TestMethod]
        public void TestGetCurrentSemester()
        {
            // Arrange
            this.testStudent.CurrentSemester = this.testSemester;

            // Act
            var result = this.studentService.GetCurrentSemester(this.testStudent);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Number);
        }

        /// <summary>
        /// Tests calculating the total course costs for a student.
        /// </summary>
        [TestMethod]
        public void TestCalculateTotalCourseCostsForStudent()
        {
            // Arrange
            this.testStudent.EnrolledCourses.Add(this.testCourse);

            // Act
            var result = this.studentService.CalculateCoursesCostsForStudent(this.testStudent);

            // Assert
            Assert.AreEqual(500, result);
        }

        /// <summary>
        /// Tests enrolling a student in a course.
        /// </summary>
        [TestMethod]
        public void TestEnrollStudentInCourse()
        {
            // Act
            this.studentService.EnrollStudentInCourse(this.testStudent, this.testCourse);

            // Assert
            this.mockStudentDataServices.Verify(repo => repo.UpdateStudent(this.testStudent), Times.Once);
            Assert.AreEqual(1, this.testStudent.EnrolledCourses.Count);
        }

        /// <summary>
        /// Tests enrolling a student in a course when the student is already enrolled.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestEnrollStudentInCourse_AlreadyEnrolled_ThrowsException()
        {
            // Arrange
            this.testStudent.EnrolledCourses.Add(this.testCourse);

            // Act
            this.studentService.EnrollStudentInCourse(this.testStudent, this.testCourse);
        }

        /// <summary>
        /// Tests removing a student from a course.
        /// </summary>
        [TestMethod]
        public void TestRemoveStudentFromCourse()
        {
            // Arrange
            this.testStudent.EnrolledCourses.Add(this.testCourse);

            // Act
            this.studentService.RemoveStudentFromCourse(this.testStudent, this.testCourse);

            // Assert
            this.mockStudentDataServices.Verify(repo => repo.UpdateStudent(this.testStudent), Times.Once);
            Assert.AreEqual(0, this.testStudent.EnrolledCourses.Count);
        }

        /// <summary>
        /// Tests removing a student from a course when the student is not enrolled.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestRemoveStudentFromCourse_NotEnrolled_ThrowsException()
        {
            // Act
            this.studentService.RemoveStudentFromCourse(this.testStudent, this.testCourse);
        }

        /// <summary>
        /// Tests promoting a student to the next semester successfully.
        /// </summary>
        [TestMethod]
        public void TestPromoteStudentToNextSemester_Success()
        {
            // Arrange
            var exam = new Exam
            {
                ExamId = 1,
                StudentId = 1,
                CourseId = 1,
                Grade = 8,
                ExamDate = DateTime.Now,
                Student = this.testStudent,
                Course = this.testCourse,
            };
            this.testCourse.Exams.Add(exam);
            this.testStudent.CurrentSemester = this.testSemester;
            this.testStudent.EnrolledCourses.Add(this.testCourse);
            this.mockSemesterDataServices.Setup(repo => repo.GetSemesterById(2)).Returns(new Semester { Id = 2, Number = 2, CreditThreshold = 5 });

            // Act
            var result = this.studentService.PromoteStudentToNextSemester(this.testStudent);

            // Assert
            Assert.IsTrue(result);
            this.mockStudentDataServices.Verify(repo => repo.UpdateStudent(this.testStudent), Times.Once);
            Assert.AreEqual(2, this.testStudent.CurrentSemesterId);
        }

        /// <summary>
        /// Tests promoting a student to the next semester when the student is not found.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestPromoteStudentToNextSemester_StudentNotFound_ThrowsException()
        {
            // Act & Assert
            this.studentService.PromoteStudentToNextSemester(new Student());
        }

        /// <summary>
        /// Tests promoting a student to the next semester when the student is null.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestPromoteStudentToNextSemester_StudentNull_ThrowsException()
        {
            // Act & Assert
            this.studentService.PromoteStudentToNextSemester(null);
        }

        /// <summary>
        /// Tests promoting a student to the next semester when the current semester is not set.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestPromoteStudentToNextSemester_CurrentSemesterNotSet_ThrowsException()
        {
            // Arrange
            this.testStudent.CurrentSemester = null;

            // Act & Assert
            this.studentService.PromoteStudentToNextSemester(this.testStudent);
        }

        /// <summary>
        /// Tests promoting a student to the next semester when the student does not have enough credits.
        /// </summary>
        [TestMethod]
        public void TestPromoteStudentToNextSemester_NotEnoughCredits_ReturnsFalse()
        {
            // Arrange
            this.testStudent.CurrentSemester = this.testSemester;
            this.mockSemesterDataServices.Setup(repo => repo.GetSemesterById(2)).Returns(new Semester { Id = 2, Number = 2, CreditThreshold = 60 });

            // Act
            var result = this.studentService.PromoteStudentToNextSemester(this.testStudent);

            // Assert
            Assert.IsFalse(result);
        }

        /// <summary>
        /// Tests promoting a student to the next semester when the next semester is not found.
        /// </summary>
        [TestMethod]
        public void TestPromoteStudentToNextSemester_NextSemesterNotFound_ReturnsFalse()
        {
            // Arrange
            this.testStudent.CurrentSemester = this.testSemester;
            this.testStudent.EnrolledCourses.Add(this.testCourse);
            this.mockSemesterDataServices.Setup(repo => repo.GetSemesterById(2)).Returns((Semester)null);

            // Act
            var result = this.studentService.PromoteStudentToNextSemester(this.testStudent);

            // Assert
            Assert.IsFalse(result);
        }

        /// <summary>
        /// Tests getting the credits per semester for a student.
        /// </summary>
        [TestMethod]
        public void TestGetCreditsPerSemester()
        {
            // Arrange
            this.testCourse.AvailableSemesters.Add(this.testSemester);
            this.testStudent.EnrolledCourses.Add(this.testCourse);

            // Act
            var result = this.studentService.GetCreditsPerSemester(this.testStudent);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(5, result[1]);
        }

        /// <summary>
        /// Tests getting credits per semester when the student has multiple courses in the same semester.
        /// </summary>
        [TestMethod]
        public void TestGetCreditsPerSemester_MultipleCoursesSameSemester()
        {
            // Arrange
            var semester = new Semester { Id = 1, Number = 1, CreditThreshold = 30 };
            var course1 = new Course { Id = 1, Name = "Course 1", Credits = 5, AvailableSemesters = new List<Semester> { semester } };
            var course2 = new Course { Id = 2, Name = "Course 2", Credits = 10, AvailableSemesters = new List<Semester> { semester } };
            var course3 = new Course { Id = 3, Name = "Course 3", Credits = 8, AvailableSemesters = new List<Semester> { semester } };
            var student = new Student
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                EnrolledCourses = new List<Course> { course1, course2, course3 },
            };

            // Act
            var result = this.studentService.GetCreditsPerSemester(student);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(23, result[1]);
        }

        /// <summary>
        /// Tests getting credits per semester when the student has enrolled courses in multiple semesters.
        /// </summary>
        [TestMethod]
        public void TestGetCreditsPerSemester_MultipleSemesters()
        {
            // Arrange
            var semester1 = new Semester { Id = 1, Number = 1, CreditThreshold = 30 };
            var semester2 = new Semester { Id = 2, Number = 2, CreditThreshold = 30 };
            var course1 = new Course { Id = 1, Name = "Course 1", Credits = 5, AvailableSemesters = new List<Semester> { semester1 } };
            var course2 = new Course { Id = 2, Name = "Course 2", Credits = 10, AvailableSemesters = new List<Semester> { semester1 } };
            var course3 = new Course { Id = 3, Name = "Course 3", Credits = 8, AvailableSemesters = new List<Semester> { semester2 } };
            var student = new Student
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                EnrolledCourses = new List<Course> { course1, course2, course3 },
            };

            // Act
            var result = this.studentService.GetCreditsPerSemester(student);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(15, result[1]);
            Assert.AreEqual(8, result[2]);
        }

        /// <summary>
        /// Tests calculating the total cost of courses for a student.
        /// </summary>
        [TestMethod]
        public void TestCalculateCoursesCostsForStudent()
        {
            // Arrange
            this.testStudent.EnrolledCourses.Add(this.testCourse);

            // Act
            var result = this.studentService.CalculateCoursesCostsForStudent(this.testStudent);

            // Assert
            Assert.AreEqual(500, result);
        }

        /// <summary>
        /// Tests the count students who passed.
        /// </summary>
        [TestMethod]
        public void TestCountStudentsWhoPassed()
        {
            // Arrange
            var exam = new Exam
            {
                ExamId = 1,
                StudentId = 1,
                CourseId = 1,
                Grade = 8,
                ExamDate = DateTime.Now,
                Student = this.testStudent,
                Course = this.testCourse,
            };
            this.testCourse.Exams.Add(exam);
            this.testStudent.EnrolledCourses.Add(this.testCourse);
            var students = new List<Student> { this.testStudent };
            this.mockStudentDataServices.Setup(repo => repo.GetAllStudents()).Returns(students);

            // Act
            var result = this.studentService.CountStudentsWhoPassed();

            // Assert
            Assert.AreEqual(1, result);
        }

        /// <summary>
        /// Tests the add student null student throws exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestAddStudent_NullStudent_ThrowsException()
        {
            // Act
            this.studentService.AddStudent(null);
        }

        /// <summary>
        /// Tests the update student null student throws exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestUpdateStudent_NullStudent_ThrowsException()
        {
            // Act
            this.studentService.UpdateStudent(null);
        }

        /// <summary>
        /// Tests the get enrolled courses null student throws exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestGetEnrolledCourses_NullStudent_ThrowsException()
        {
            // Act
            this.studentService.GetEnrolledCourses(null);
        }

        /// <summary>
        /// Tests the get exams for student null student throws exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestGetExamsForStudent_NullStudent_ThrowsException()
        {
            // Act
            this.studentService.GetExamsForStudent(null);
        }

        /// <summary>
        /// Tests the get current semester null student throws exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestGetCurrentSemester_NullStudent_ThrowsException()
        {
            // Act
            this.studentService.GetCurrentSemester(null);
        }

        /// <summary>
        /// Tests the enroll student in course null student throws exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestEnrollStudentInCourse_NullStudent_ThrowsException()
        {
            // Act
            this.studentService.EnrollStudentInCourse(null, this.testCourse);
        }

        /// <summary>
        /// Tests the enroll student in course null course throws exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestEnrollStudentInCourse_NullCourse_ThrowsException()
        {
            // Act
            this.studentService.EnrollStudentInCourse(this.testStudent, null);
        }

        /// <summary>
        /// Tests the enroll student in courses null student throws exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestEnrollStudentInCourses_NullStudent_ThrowsException()
        {
            // Act
            this.studentService.EnrollStudentInCourses(null, new List<Course> { this.testCourse });
        }

        /// <summary>
        /// Tests the enroll student in courses null courses throws exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestEnrollStudentInCourses_NullCourses_ThrowsException()
        {
            // Act
            this.studentService.EnrollStudentInCourses(this.testStudent, null);
        }

        /// <summary>
        /// Tests the enroll student in courses empty courses throws exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestEnrollStudentInCourses_EmptyCourses_ThrowsException()
        {
            // Act
            this.studentService.EnrollStudentInCourses(this.testStudent, new List<Course>());
        }

        /// <summary>
        /// Tests the enroll student in course missing prerequisites throws exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestEnrollStudentInCourse_MissingPrerequisites_ThrowsException()
        {
            // Arrange
            var prerequisiteCourse = new Course
            {
                Id = 2,
                Name = "Introduction to Programming",
                Description = "An introductory programming course.",
                Credits = 3,
                Cost = 300,
                MinThreshold = 30,
                MaxThreshold = 90,
            };

            this.testCourse.Prerequisites.Add(prerequisiteCourse);

            // Act
            this.studentService.EnrollStudentInCourse(this.testStudent, this.testCourse);
        }

        /// <summary>
        /// Tests the enroll student in course with prerequisites success.
        /// </summary>
        [TestMethod]
        public void TestEnrollStudentInCourse_WithPrerequisites_Success()
        {
            // Arrange
            var prerequisiteCourse = new Course
            {
                Id = 2,
                Name = "Introduction to Programming",
                Description = "An introductory programming course.",
                Credits = 3,
                Cost = 300,
                MinThreshold = 30,
                MaxThreshold = 90,
            };

            var prerequisiteExam = new Exam
            {
                ExamId = 1,
                StudentId = 1,
                CourseId = 2,
                Grade = 8,
                ExamDate = DateTime.Now,
                Student = this.testStudent,
                Course = prerequisiteCourse,
            };

            prerequisiteCourse.Exams.Add(prerequisiteExam);
            this.testStudent.EnrolledCourses.Add(prerequisiteCourse);
            this.testCourse.Prerequisites.Add(prerequisiteCourse);

            // Act
            this.studentService.EnrollStudentInCourse(this.testStudent, this.testCourse);

            // Assert
            this.mockStudentDataServices.Verify(repo => repo.UpdateStudent(this.testStudent), Times.Once);
            Assert.AreEqual(2, this.testStudent.EnrolledCourses.Count);
        }

        /// <summary>
        /// Tests the enroll student in course prerequisite not passed throws exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestEnrollStudentInCourse_PrerequisiteNotPassed_ThrowsException()
        {
            // Arrange
            var prerequisiteCourse = new Course
            {
                Id = 2,
                Name = "Introduction to Programming",
                Description = "An introductory programming course.",
                Credits = 3,
                Cost = 300,
                MinThreshold = 30,
                MaxThreshold = 90,
            };

            var prerequisiteExam = new Exam
            {
                ExamId = 1,
                StudentId = 1,
                CourseId = 2,
                Grade = 4, // Not passed
                ExamDate = DateTime.Now,
                Student = this.testStudent,
                Course = prerequisiteCourse,
            };

            prerequisiteCourse.Exams.Add(prerequisiteExam);
            this.testStudent.EnrolledCourses.Add(prerequisiteCourse);
            this.testCourse.Prerequisites.Add(prerequisiteCourse);

            // Act
            this.studentService.EnrollStudentInCourse(this.testStudent, this.testCourse);
        }

        /// <summary>
        /// Tests the remove student from course null student throws exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestRemoveStudentFromCourse_NullStudent_ThrowsException()
        {
            // Act
            this.studentService.RemoveStudentFromCourse(null, this.testCourse);
        }

        /// <summary>
        /// Tests the remove student from course null course throws exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestRemoveStudentFromCourse_NullCourse_ThrowsException()
        {
            // Act
            this.studentService.RemoveStudentFromCourse(this.testStudent, null);
        }

        /// <summary>
        /// Tests the calculate courses costs for student null student throws exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestCalculateCoursesCostsForStudent_NullStudent_ThrowsException()
        {
            // Act
            this.studentService.CalculateCoursesCostsForStudent(null);
        }

        /// <summary>
        /// Tests the get credits per semester null student throws exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestGetCreditsPerSemester_NullStudent_ThrowsException()
        {
            // Act
            this.studentService.GetCreditsPerSemester(null);
        }

        /// <summary>
        /// Tests the validate student valid student does not throw exception.
        /// </summary>
        [TestMethod]
        public void TestValidateStudent_ValidStudent_DoesNotThrowException()
        {
            // Act
            this.studentService.ValidateStudent(this.testStudent);

            // Assert
            Assert.IsTrue(true);
        }

        /// <summary>
        /// Tests the validate student null student throws exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestValidateStudent_NullStudent_ThrowsException()
        {
            // Act
            this.studentService.ValidateStudent(null);
        }

        /// <summary>
        /// Tests the validate student invalid student throws exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void TestValidateStudent_InvalidStudent_ThrowsException()
        {
            // Arrange
            this.testStudent.FirstName = null;

            // Act
            this.studentService.ValidateStudent(this.testStudent);
        }

        /// <summary>
        /// Tests the validate student no valid contact throws exception.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void TestValidateStudent_NoValidContact_ThrowsException()
        {
            // Arrange
            this.testStudent.Phones.Clear();
            this.testStudent.Emails.Clear();

            // Act
            this.studentService.ValidateStudent(this.testStudent);
        }

        /// <summary>
        /// Tests calculating the average money spent by students when there are no students.
        /// </summary>
        [TestMethod]
        public void TestCalculateAverageSpent_NoStudents()
        {
            // Arrange
            this.mockStudentDataServices.Setup(repo => repo.GetAllStudents()).Returns(new List<Student>());

            // Act
            var result = this.studentService.CalculateAverageSpent();

            // Assert
            Assert.AreEqual(0m, result);
        }

        /// <summary>
        /// Tests calculating the average money spent by students when there are students but no enrolled courses.
        /// </summary>
        [TestMethod]
        public void TestCalculateAverageSpent_StudentsNoCourses()
        {
            // Arrange
            var students = new List<Student>
            {
                new Student { Id = 1, FirstName = "John", LastName = "Doe", EnrolledCourses = new List<Course>() },
                new Student { Id = 2, FirstName = "Jane", LastName = "Doe", EnrolledCourses = new List<Course>() },
            };
            this.mockStudentDataServices.Setup(repo => repo.GetAllStudents()).Returns(students);

            // Act
            var result = this.studentService.CalculateAverageSpent();

            // Assert
            Assert.AreEqual(0m, result);
        }

        /// <summary>
        /// Tests calculating the average money spent by students when there are students with enrolled courses.
        /// </summary>
        [TestMethod]
        public void TestCalculateAverageSpent_StudentsWithCourses()
        {
            // Arrange
            var course1 = new Course { Id = 1, Name = "Course 1", Cost = 100m };
            var course2 = new Course { Id = 2, Name = "Course 2", Cost = 200m };
            var students = new List<Student>
            {
                new Student { Id = 1, FirstName = "John", LastName = "Doe", EnrolledCourses = new List<Course> { course1 } },
                new Student { Id = 2, FirstName = "Jane", LastName = "Doe", EnrolledCourses = new List<Course> { course2 } },
            };
            this.mockStudentDataServices.Setup(repo => repo.GetAllStudents()).Returns(students);

            // Act
            var result = this.studentService.CalculateAverageSpent();

            // Assert
            Assert.AreEqual(150m, result);
        }

        /// <summary>
        /// Tests calculating the average money spent by students when there are students with multiple enrolled courses.
        /// </summary>
        [TestMethod]
        public void TestCalculateAverageSpent_StudentsWithMultipleCourses()
        {
            // Arrange
            var course1 = new Course { Id = 1, Name = "Course 1", Cost = 100m };
            var course2 = new Course { Id = 2, Name = "Course 2", Cost = 200m };
            var course3 = new Course { Id = 3, Name = "Course 3", Cost = 300m };
            var students = new List<Student>
            {
                new Student { Id = 1, FirstName = "John", LastName = "Doe", EnrolledCourses = new List<Course> { course1, course2 } },
                new Student { Id = 2, FirstName = "Jane", LastName = "Doe", EnrolledCourses = new List<Course> { course3 } },
            };
            this.mockStudentDataServices.Setup(repo => repo.GetAllStudents()).Returns(students);

            // Act
            var result = this.studentService.CalculateAverageSpent();

            // Assert
            Assert.AreEqual(300m, result);
        }
    }
}