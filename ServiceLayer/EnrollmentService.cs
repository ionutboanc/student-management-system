using DataMapper;
using DomainModel;
using DomainModel.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ServiceLayer
{
    public class EnrollmentService : IEnrollmentDataServices
    {

        public EnrollmentService() { }

        public void AddEnrollment(Enrollment enrollment)
        {
            ValidateEnrollment(enrollment);
            DAOFactoryMethod.CurrentDAOFactory.EnrollmentDataServices.AddEnrollment(enrollment);
        }

        public void DeleteEnrollment(Enrollment enrollment)
        {
            if (enrollment == null)
            {
                throw new ArgumentNullException(nameof(enrollment), "Enrollment cannot be null.");
            }
            DAOFactoryMethod.CurrentDAOFactory.EnrollmentDataServices.DeleteEnrollment(enrollment);
        }

        public IList<Enrollment> GetAllEnrollments()
        {
            return DAOFactoryMethod.CurrentDAOFactory.EnrollmentDataServices.GetAllEnrollments();
        }

        public void UpdateEnrollment(Enrollment enrollment)
        {
            ValidateEnrollment(enrollment);
            DAOFactoryMethod.CurrentDAOFactory.EnrollmentDataServices.UpdateEnrollment(enrollment);
        }

        public Enrollment GetEnrollmentById(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id), "Enrollment ID must be greater than zero.");
            }
            return DAOFactoryMethod.CurrentDAOFactory.EnrollmentDataServices.GetEnrollmentById(id);
        }

        public void RecordExamGrade(int enrollmentId, int grade, DateTime date)
        {
            var enrollment = GetEnrollmentById(enrollmentId);

            if (enrollment.NumberOfAttempts >= Constants.MaxNumberOfAttempts)
            {
                throw new InvalidOperationException("Maximum number of exam attempts reached.");
            }

            if (enrollment.Grade >= 5)
            {
                throw new InvalidOperationException("Student has already passed the exam.");
            }

            if (enrollment.LastAttemptDate.HasValue && date <= enrollment.LastAttemptDate)
            {
                throw new InvalidOperationException("Exam date cannot be earlier or the same than the last attempt date.");
            }

            enrollment.NumberOfAttempts++;
            enrollment.Grade = grade;
            enrollment.LastAttemptDate = date;

            DAOFactoryMethod.CurrentDAOFactory.EnrollmentDataServices.UpdateEnrollment(enrollment);
        }
        public void PromoteStudent(Enrollment enrollment)
        {
            var student = enrollment.Student;
            var currentSemester = student.Enrollments.Max(e => e.Semester.Number);
            var nextSemester = currentSemester + 1;

            var totalCredits = GetTotalCredits(enrollment.Student, currentSemester);
            var semester = DAOFactoryMethod.CurrentDAOFactory.SemesterDataServices.GetSemesterById(nextSemester);

            if (totalCredits < semester.CreditThreshold)
            {
                throw new InvalidOperationException("Student does not meet the credit requirements for promotion.");
            }

            // Logic to promote the student to the next semester
        }
        private void ValidateEnrollment(Enrollment enrollment)
        {
            if (enrollment == null)
            {
                throw new ArgumentNullException(nameof(enrollment), "Enrollment cannot be null.");
            }

            var context = new ValidationContext(enrollment, null, null);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(enrollment, context, results, true);

            if (!isValid)
            {
                throw new ValidationException("Enrollment validation failed: " + string.Join(", ", results.Select(r => r.ErrorMessage)));
            }
        }
    }
}