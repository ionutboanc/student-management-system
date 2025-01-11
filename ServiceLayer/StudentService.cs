using DataMapper;
using DomainModel;
using DomainModel.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ServiceLayer
{
    public class StudentService : IStudentDataServices
    {
        public StudentService() { }

        public void AddStudent(Student student)
        {
            ValidateStudent(student);
            DAOFactoryMethod.CurrentDAOFactory.StudentDataServices.AddStudent(student);
        }

        public void DeleteStudent(Student student)
        {
            ValidateStudent(student);
            DAOFactoryMethod.CurrentDAOFactory.StudentDataServices.DeleteStudent(student);
        }

        public IList<Student> GetAllStudents()
        {
            return DAOFactoryMethod.CurrentDAOFactory.StudentDataServices.GetAllStudents();
        }

        public void UpdateStudent(Student student)
        {
            ValidateStudent(student);
            DAOFactoryMethod.CurrentDAOFactory.StudentDataServices.UpdateStudent(student);
        }

        public Student GetStudentById(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id), "Student ID must be greater than zero.");
            }
            return DAOFactoryMethod.CurrentDAOFactory.StudentDataServices.GetStudentById(id);
        }

        public void EnrollStudentInCourse(int studentId, int courseId, int semesterId)
        {
            var student = GetStudentById(studentId);
            var course = DAOFactoryMethod.CurrentDAOFactory.CourseDataServices.GetCourseById(courseId);
            var semester = DAOFactoryMethod.CurrentDAOFactory.SemesterDataServices.GetSemesterById(semesterId);

            if (student.Enrollments.Any(e => e.CourseId == courseId))
            {
                throw new InvalidOperationException("Student is already enrolled in this course.");
            }

            if (!semester.AvailableCourses.Contains(course))
            {
                throw new InvalidOperationException("Course is not available in the specified semester.");
            }

            ValidatePrerequisites(student, course);

            var enrollment = new Enrollment
            {
                StudentId = studentId,
                CourseId = courseId,
                SemesterId = semesterId,
                NumberOfAttempts = 0,
                Grade = 0,
                LastAttemptDate = null
            };

            student.Enrollments.Add(enrollment);
            DAOFactoryMethod.CurrentDAOFactory.StudentDataServices.UpdateStudent(student);
        }

        public int GetTotalCredits(int studentId, int semesterId)
        {
            var student = GetStudentById(studentId);
            return student.Enrollments
                .Where(e => e.SemesterId == semesterId && e.Grade >= 5)
                .Sum(e => e.Course.Credits);
        }

        public void PromoteStudent(int studentId)
        {
            var student = GetStudentById(studentId);
            var currentSemester = student.Enrollments.Max(e => e.Semester.Number);
            var nextSemester = currentSemester + 1;

            var totalCredits = GetTotalCredits(studentId, currentSemester);
            var semester = DAOFactoryMethod.CurrentDAOFactory.SemesterDataServices.GetSemesterById(nextSemester);
            if (nextSemester == null)
            {
                throw new InvalidOperationException("Next semester not found. Promotion is not possible.");
            }
            if (totalCredits < semester.CreditThreshold)
            {
                throw new InvalidOperationException("Student does not meet the credit requirements for promotion.");
            }
            student.semesterId = nextSemester;
            // Logic to promote the student to the next semester
        }

        public decimal CalculateTotalCost(int studentId)
        {
            var student = GetStudentById(studentId);
            var totalCost = student.Enrollments.Sum(e => e.Course.GetCost());

            return totalCost;
        }

        private void ValidateStudent(Student student)
        {
            if (student == null)
            {
                throw new ArgumentNullException(nameof(student), "Student cannot be null.");
            }

            var context = new ValidationContext(student, null, null);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(student, context, results, true);

            if (!isValid)
            {
                throw new ValidationException("Student validation failed: " + string.Join(", ", results.Select(r => r.ErrorMessage)));
            }

            if (!student.HasValidContact())
            {
                throw new ValidationException("Student must have at least one valid contact method (phone or email).");
            }
        }

        private void ValidatePrerequisites(Student student, Course course)
        {
            foreach (var prerequisite in course.Prerequisites)
            {
                var enrollment = student.Enrollments.FirstOrDefault(e => e.CourseId == prerequisite.PrerequisiteCourseId);
                if (enrollment == null || enrollment.Grade < prerequisite.MinimumGrade)
                {
                    throw new InvalidOperationException($"Student has not met the prerequisite requirements for course {course.Name}.");
                }
            }
        }
    }
}