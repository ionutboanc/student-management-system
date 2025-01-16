using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using DataMapper;
using DomainModel.Models;
using log4net;

namespace ServiceLayer
{
    /// <summary>
    /// Provides services for managing semesters.
    /// </summary>
    public class SemesterService : ISemesterService
    {
        /// <summary>
        /// Logger instance for logging.
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(SemesterService));

        /// <summary>
        /// Initializes a new instance of the <see cref="SemesterService"/> class.
        /// </summary>
        public SemesterService()
        {
            Logger.Info("SemesterService initialized.");
        }

        /// <summary>
        /// Adds a new semester.
        /// </summary>
        /// <param name="semester">The semester to add.</param>
        public void AddSemester(Semester semester)
        {
            Logger.Info("Adding a new semester.");
            this.Validate(semester);
            DAOFactoryMethod.CurrentDAOFactory.SemesterDataServices.AddSemester(semester);
        }

        /// <summary>
        /// Updates an existing semester.
        /// </summary>
        /// <param name="semester">The semester to update.</param>
        public void UpdateSemester(Semester semester)
        {
            Logger.Info("Updating an existing semester.");
            this.Validate(semester);
            DAOFactoryMethod.CurrentDAOFactory.SemesterDataServices.UpdateSemester(semester);
        }

        /// <summary>
        /// Deletes a semester.
        /// </summary>
        /// <param name="semester">The semester to delete.</param>
        public void DeleteSemester(Semester semester)
        {
            Logger.Info("Deleting a semester.");
            if (semester == null)
            {
                Logger.Error("Semester not found.", new ArgumentException("Semester not found.", nameof(semester)));
                throw new ArgumentException("Semester not found.", nameof(semester));
            }

            DAOFactoryMethod.CurrentDAOFactory.SemesterDataServices.DeleteSemester(semester);
        }

        /// <summary>
        /// Gets a semester by its ID.
        /// </summary>
        /// <param name="semesterId">The ID of the semester.</param>
        /// <returns>The semester with the specified ID.</returns>
        public Semester GetSemesterById(int semesterId)
        {
            Logger.Info($"Getting semester by ID: {semesterId}.");
            if (semesterId < 0)
            {
                Logger.Error("Semester ID must be positive.", new ArgumentOutOfRangeException(nameof(semesterId), "Semester ID must be positive."));
                throw new ArgumentOutOfRangeException(nameof(semesterId), "Semester ID must be positive.");
            }
            return DAOFactoryMethod.CurrentDAOFactory.SemesterDataServices.GetSemesterById(semesterId);
        }

        /// <summary>
        /// Gets the students in a semester.
        /// </summary>
        /// <param name="semester">The semester to get students for.</param>
        /// <returns>A list of students in the semester.</returns>
        public IList<Student> GetStudentsInSemester(Semester semester)
        {
            Logger.Info("Getting students in a semester.");
            if (semester == null)
            {
                Logger.Error("Semester not found.", new ArgumentException("Semester not found."));
                throw new ArgumentException("Semester not found.");
            }
            return semester.Students;
        }

        /// <summary>
        /// Gets all semesters.
        /// </summary>
        /// <returns>A list of all semesters.</returns>
        public IList<Semester> GetAllSemesters()
        {
            Logger.Info("Getting all semesters.");
            return DAOFactoryMethod.CurrentDAOFactory.SemesterDataServices.GetAllSemesters();
        }

        /// <summary>
        /// Gets the courses in a semester.
        /// </summary>
        /// <param name="semester">The semester to get courses for.</param>
        /// <returns>A list of courses in the semester.</returns>
        public IList<Course> GetCoursesInSemester(Semester semester)
        {
            Logger.Info("Getting courses in a semester.");
            if (semester == null)
            {
                Logger.Error("Semester not found.", new ArgumentException("Semester not found."));
                throw new ArgumentException("Semester not found.");
            }
            return semester.AvailableCourses;
        }

        /// <summary>
        /// Gets the students in a semester by semester ID.
        /// </summary>
        /// <param name="semesterId">The ID of the semester.</param>
        /// <returns>A list of students in the semester.</returns>
        public IEnumerable<Student> GetStudentsInSemester(int semesterId)
        {
            Logger.Info($"Getting students in semester by ID: {semesterId}.");
            var semester = this.GetSemesterById(semesterId);
            if (semester == null)
            {
                Logger.Error("Semester not found.", new ArgumentException("Semester not found.", nameof(semesterId)));
                throw new ArgumentException("Semester not found.", nameof(semesterId));
            }
            return semester.Students;
        }

        /// <summary>
        /// Gets the previous semester.
        /// </summary>
        /// <param name="currentSemester">The current semester.</param>
        /// <returns>The previous semester.</returns>
        public Semester GetPreviousSemester(Semester currentSemester)
        {
            Logger.Info("Getting the previous semester.");
            if (currentSemester == null)
            {
                Logger.Error("Current semester not found.", new ArgumentException("Current semester not found."));
                throw new ArgumentException("Current semester not found.");
            }
            return DAOFactoryMethod.CurrentDAOFactory.SemesterDataServices.GetAllSemesters()
                .FirstOrDefault(s => s.Number == currentSemester.Number - 1);
        }

        /// <summary>
        /// Gets the next semester.
        /// </summary>
        /// <param name="currentSemester">The current semester.</param>
        /// <returns>The next semester.</returns>
        public Semester GetNextSemester(Semester currentSemester)
        {
            Logger.Info("Getting the next semester.");
            if (currentSemester == null)
            {
                Logger.Error("Current semester not found.", new ArgumentException("Current semester not found."));
                throw new ArgumentException("Current semester not found.");
            }
            return DAOFactoryMethod.CurrentDAOFactory.SemesterDataServices.GetAllSemesters()
                .FirstOrDefault(s => s.Number == currentSemester.Number + 1);
        }

        /// <summary>
        /// Validates a semester.
        /// </summary>
        /// <param name="semester">The semester to validate.</param>
        private void Validate(Semester semester)
        {
            Logger.Info("Validating a semester.");
            if (semester == null)
            {
                Logger.Error("Semester cannot be null.", new ArgumentNullException(nameof(semester), "Semester cannot be null."));
                throw new ArgumentNullException(nameof(semester), "Semester cannot be null.");
            }

            semester.Validate();

            this.ValidateCourses(semester);
        }

        /// <summary>
        /// Validates the courses in a semester.
        /// </summary>
        /// <param name="semester">The semester to validate courses for.</param>
        private void ValidateCourses(Semester semester)
        {
            Logger.Info("Validating courses in a semester.");
            foreach (var course in semester.AvailableCourses)
            {
                this.ValidateCourse(course);
            }
        }

        /// <summary>
        /// Validates a course.
        /// </summary>
        /// <param name="course">The course to validate.</param>
        private void ValidateCourse(Course course)
        {
            Logger.Info("Validating a course.");
            if (course == null)
            {
                Logger.Error("Course cannot be null.", new ArgumentNullException(nameof(course), "Course cannot be null."));
                throw new ArgumentNullException(nameof(course), "Course cannot be null.");
            }

            course.Validate();

            if (!course.IsCostValid())
            {
                Logger.Error($"Course cost for '{course.Name}' is not within the valid range.", new ValidationException($"Course cost for '{course.Name}' is not within the valid range."));
                throw new ValidationException($"Course cost for '{course.Name}' is not within the valid range.");
            }
        }
    }
}