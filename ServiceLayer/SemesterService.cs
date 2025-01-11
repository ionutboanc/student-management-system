using DataMapper;
using DomainModel.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ServiceLayer
{
    public class SemesterService : ISemesterDataServices
    {
        public SemesterService() { }

        public void AddSemester(Semester semester)
        {
            ValidateSemester(semester);
            DAOFactoryMethod.CurrentDAOFactory.SemesterDataServices.AddSemester(semester);
        }

        public void DeleteSemester(Semester semester)
        {
            if (semester == null)
            {
                throw new ArgumentNullException(nameof(semester), "Semester cannot be null.");
            }
            DAOFactoryMethod.CurrentDAOFactory.SemesterDataServices.DeleteSemester(semester);
        }

        public IList<Semester> GetAllSemesters()
        {
            return DAOFactoryMethod.CurrentDAOFactory.SemesterDataServices.GetAllSemesters();
        }

        public void UpdateSemester(Semester semester)
        {
            ValidateSemester(semester);
            DAOFactoryMethod.CurrentDAOFactory.SemesterDataServices.UpdateSemester(semester);
        }

        public Semester GetSemesterById(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id), "Semester ID must be greater than zero.");
            }
            return DAOFactoryMethod.CurrentDAOFactory.SemesterDataServices.GetSemesterById(id);
        }

        /// <summary>
        /// Add a course to the semester's list of available courses.
        /// </summary>
        public void AddCourseToSemester(Semester semester, Course course)
        {
            ValidateSemester(semester);
            ValidateCourse(course);

            if (semester.AvailableCourses.Contains(course))
            {
                throw new InvalidOperationException("Course is already part of the semester.");
            }

            semester.AvailableCourses.Add(course);
            UpdateSemester(semester);
        }

        /// <summary>
        /// Remove a course from the semester's list of available courses.
        /// </summary>
        public void RemoveCourseFromSemester(Semester semester, Course course)
        {
            ValidateSemester(semester);

            if (!semester.AvailableCourses.Contains(course))
            {
                throw new KeyNotFoundException("Course not found in the semester.");
            }

            semester.AvailableCourses.Remove(course);
            UpdateSemester(semester);
        }

        /// <summary>
        /// Get a list of courses available in the semester.
        /// </summary>
        public IList<Course> GetCoursesInSemester(Semester semester)
        {
            ValidateSemester(semester);
            return semester.AvailableCourses;
        }

        /// <summary>
        /// Calculate the total credit hours for all courses in the semester.
        /// </summary>
        public int CalculateTotalCredits(Semester semester)
        {
            ValidateSemester(semester);
            return semester.AvailableCourses.Sum(course => course.Credits);
        }

        /// <summary>
        /// Check if adding a course exceeds the credit threshold.
        /// </summary>
        public bool CanAddCourse(Semester semester, Course course)
        {
            ValidateSemester(semester);
            ValidateCourse(course);

            var totalCredits = CalculateTotalCredits(semester) + course.Credits;
            return totalCredits <= semester.CreditThreshold;
        }

        /// <summary>
        /// Validate the semester entity.
        /// </summary>
        private void ValidateSemester(Semester semester)
        {
            if (semester == null)
            {
                throw new ArgumentNullException(nameof(semester), "Semester cannot be null.");
            }

            var context = new ValidationContext(semester, null, null);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(semester, context, results, true);

            if (!isValid)
            {
                throw new ValidationException("Semester validation failed: " + string.Join(", ", results.Select(r => r.ErrorMessage)));
            }

            if (semester.CreditThreshold < 0)
            {
                throw new ValidationException("Credit threshold must be non-negative.");
            }

            ValidateCourses(semester);
        }

        /// <summary>
        /// Validate the courses associated with a semester.
        /// </summary>
        private void ValidateCourses(Semester semester)
        {
            foreach (var course in semester.AvailableCourses)
            {
                ValidateCourse(course);
            }
        }

        /// <summary>
        /// Validate a single course entity.
        /// </summary>
        private void ValidateCourse(Course course)
        {
            if (course == null)
            {
                throw new ArgumentNullException(nameof(course), "Course cannot be null.");
            }

            var context = new ValidationContext(course, null, null);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(course, context, results, true);

            if (!isValid)
            {
                throw new ValidationException("Course validation failed: " + string.Join(", ", results.Select(r => r.ErrorMessage)));
            }

            if (!course.IsCostValid())
            {
                throw new ValidationException($"Course cost for '{course.Name}' is not within the valid range.");
            }
        }
    }
}
