// <copyright file="{fileName}" company="Transilvania University of Brasov">
// Copyright (c) 2025 Boanc Ionut-Silviu. All rights reserved.
// </copyright>

namespace ServiceLayer
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using DataMapper;
    using DomainModel;
    using DomainModel.Models;
    using log4net;

    /// <summary>
    /// Provides services for managing courses.
    /// </summary>
    public class CourseService : ICourseService
    {
        /// <summary>
        /// Logger instance for logging.
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(CourseService));

        /// <summary>
        /// Initializes a new instance of the <see cref="CourseService"/> class.
        /// </summary>
        public CourseService()
        {
            Logger.Info("CourseService initialized.");
        }

        /// <summary>
        /// Adds a new course.
        /// </summary>
        /// <param name="course">The course to add.</param>
        public void AddCourse(Course course)
        {
            Logger.Info("Adding a new course.");
            this.ValidateCourse(course);
            DAOFactoryMethod.CurrentDAOFactory.CourseDataServices.AddCourse(course);
        }

        /// <summary>
        /// Updates an existing course.
        /// </summary>
        /// <param name="course">The course to update.</param>
        public void UpdateCourse(Course course)
        {
            Logger.Info("Updating an existing course.");
            this.ValidateCourse(course);
            DAOFactoryMethod.CurrentDAOFactory.CourseDataServices.UpdateCourse(course);
        }

        /// <summary>
        /// Deletes a course.
        /// </summary>
        /// <param name="course">The course to delete.</param>
        public void DeleteCourse(Course course)
        {
            Logger.Info("Deleting a course.");
            if (course == null)
            {
                Logger.Error("Course not found.", new ArgumentException("Course not found."));
                throw new ArgumentException("Course not found.");
            }

            DAOFactoryMethod.CurrentDAOFactory.CourseDataServices.DeleteCourse(course);
        }

        /// <summary>
        /// Gets a course by its ID.
        /// </summary>
        /// <param name="courseId">The ID of the course.</param>
        /// <returns>The course with the specified ID.</returns>
        public Course GetCourseById(int courseId)
        {
            Logger.Info($"Getting course by ID: {courseId}.");
            if (courseId <= 0)
            {
                Logger.Error("Course ID must be greater than zero.", new ArgumentOutOfRangeException(nameof(courseId), "Course ID must be greater than zero."));
                throw new ArgumentOutOfRangeException(nameof(courseId), "Course ID must be greater than zero.");
            }

            return DAOFactoryMethod.CurrentDAOFactory.CourseDataServices.GetCourseById(courseId);
        }

        /// <summary>
        /// Gets all courses.
        /// </summary>
        /// <returns>A list of all courses.</returns>
        public IList<Course> GetAllCourses()
        {
            Logger.Info("Getting all courses.");
            return DAOFactoryMethod.CurrentDAOFactory.CourseDataServices.GetAllCourses();
        }

        /// <summary>
        /// Gets the prerequisites for a course.
        /// </summary>
        /// <param name="course">The course to get prerequisites for.</param>
        /// <returns>A list of prerequisite courses.</returns>
        public IList<Course> GetPrerequisites(Course course)
        {
            Logger.Info("Getting prerequisites for a course.");
            if (course == null)
            {
                Logger.Error("Course not found.", new ArgumentException("Course not found."));
                throw new ArgumentException("Course not found.");
            }

            return course.Prerequisites;
        }

        /// <summary>
        /// Gets the discount for a course.
        /// </summary>
        /// <param name="course">The course to get the discount for.</param>
        /// <returns>The discount for the course.</returns>
        public Discount GetDiscount(Course course)
        {
            Logger.Info("Getting discount for a course.");
            if (course == null)
            {
                Logger.Error("Course not found.", new ArgumentException("Course not found."));
                throw new ArgumentException("Course not found.");
            }

            return course.Discount;
        }

        /// <summary>
        /// Validates a course.
        /// </summary>
        /// <param name="course">The course to validate.</param>
        public void ValidateCourse(Course course)
        {
            Logger.Info("Validating a course.");
            if (course == null)
            {
                Logger.Error("Course cannot be null.", new ArgumentNullException(nameof(course), "Course cannot be null."));
                throw new ArgumentNullException(nameof(course), "Course cannot be null.");
            }

            var context = new ValidationContext(course, null, null);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(course, context, results, true);

            if (!isValid)
            {
                Logger.Error("Course validation failed.", new ValidationException("Course validation failed: " + string.Join(", ", results.Select(r => r.ErrorMessage))));
                throw new ValidationException("Course validation failed: " + string.Join(", ", results.Select(r => r.ErrorMessage)));
            }

            if (!course.IsCostValid())
            {
                Logger.Error("Course cost is not within the valid range.", new ValidationException("Course cost is not within the valid range."));
                throw new ValidationException("Course cost is not within the valid range.");
            }

            this.ValidateNoCircularDependencies(course);
        }

        /// <summary>
        /// Gets the students enrolled in a course.
        /// </summary>
        /// <param name="course">The course to get students for.</param>
        /// <returns>A list of students enrolled in the course.</returns>
        public IList<Student> GetEnrolledStudents(Course course)
        {
            Logger.Info("Getting students enrolled in a course.");
            if (course == null)
            {
                Logger.Error("Course not found.", new ArgumentException("Course not found."));
                throw new ArgumentException("Course not found.");
            }

            return course.Exams.Select(e => e.Student).Distinct().ToList();
        }

        /// <summary>
        /// Gets the exams for a course.
        /// </summary>
        /// <param name="course">The course to get exams for.</param>
        /// <returns>A list of exams for the course.</returns>
        public IList<Exam> GetExamsForCourse(Course course)
        {
            Logger.Info("Getting exams for a course.");
            if (course == null)
            {
                Logger.Error("Course not found.", new ArgumentException("Course not found."));
                throw new ArgumentException("Course not found.");
            }

            return course.Exams;
        }

        /// <summary>
        /// Gets the semesters for a course.
        /// </summary>
        /// <param name="course">The course to get semesters for.</param>
        /// <returns>A list of semesters for the course.</returns>
        public IList<Semester> GetSemestersForCourse(Course course)
        {
            Logger.Info("Getting semesters for a course.");
            if (course == null)
            {
                Logger.Error("Course not found.", new ArgumentException("Course not found."));
                throw new ArgumentException("Course not found.");
            }

            return course.AvailableSemesters;
        }

        /// <summary>
        /// Gets the courses for a semester.
        /// </summary>
        /// <param name="semester">The semester to get courses for.</param>
        /// <returns>A list of courses for the semester.</returns>
        public IList<Course> GetCoursesBySemester(Semester semester)
        {
            Logger.Info("Getting courses for a semester.");
            if (semester == null)
            {
                Logger.Error("Semester not found.", new ArgumentException("Semester not found."));
                throw new ArgumentException("Semester not found.");
            }

            return semester.AvailableCourses;
        }

        /// <summary>
        /// Calculates the average grades above five for a course.
        /// </summary>
        /// <param name="course">The course to calculate the average grades for.</param>
        /// <returns>The average grades above five.</returns>
        public double CalculateAverageGradesAboveFive(Course course)
        {
            Logger.Info("Calculating average grades above five for a course.");
            if (course == null)
            {
                Logger.Error("Course cannot be null.", new ArgumentNullException(nameof(course), "Course cannot be null."));
                throw new ArgumentNullException(nameof(course), "Course cannot be null.");
            }

            var grades = course.Exams.Where(e => e.Grade > 5).Select(e => e.Grade);
            return grades.Any() ? grades.Average() : 0.0;
        }

        /// <summary>
        /// Calculates the average number of courses per student.
        /// </summary>
        /// <returns>The average number of courses per student.</returns>
        public double CalculateAverageNumberOfCoursesPerStudent()
        {
            Logger.Info("Calculating average number of courses per student.");
            var students = DAOFactoryMethod.CurrentDAOFactory.StudentDataServices.GetAllStudents();
            var totalCourses = students.Sum(s => s.EnrolledCourses.Count);
            return students.Any() ? (double)totalCourses / students.Count() : 0.0;
        }

        /// <summary>
        /// Gets the number of students enrolled in a course.
        /// </summary>
        /// <param name="course">The course to get the number of enrolled students for.</param>
        /// <returns>The number of students enrolled in the course.</returns>
        public int GetNumberOfEnrolledStudents(Course course)
        {
            Logger.Info("Getting number of students enrolled in a course.");
            if (course == null)
            {
                Logger.Error("Course not found.", new ArgumentException("Course not found."));
                throw new ArgumentException("Course not found.");
            }

            return course.Exams.Select(e => e.Student).Distinct().Count();
        }

        /// <summary>
        /// Validates that there are no circular dependencies in the course prerequisites.
        /// </summary>
        /// <param name="course">The course to validate.</param>
        private void ValidateNoCircularDependencies(Course course)
        {
            Logger.Info("Validating no circular dependencies in course prerequisites.");
            var visited = new HashSet<int>();
            if (this.HasCircularDependency(course, visited))
            {
                Logger.Error("Circular dependency detected in course prerequisites.", new InvalidOperationException("Circular dependency detected in course prerequisites."));
                throw new InvalidOperationException("Circular dependency detected in course prerequisites.");
            }
        }

        /// <summary>
        /// Checks if there is a circular dependency in the course prerequisites.
        /// </summary>
        /// <param name="course">The course to check.</param>
        /// <param name="visited">The set of visited course IDs.</param>
        /// <returns>True if there is a circular dependency, otherwise false.</returns>
        private bool HasCircularDependency(Course course, HashSet<int> visited)
        {
            if (visited.Contains(course.Id))
            {
                return true;
            }

            visited.Add(course.Id);

            foreach (var prerequisite in course.Prerequisites)
            {
                if (this.HasCircularDependency(prerequisite, visited))
                {
                    return true;
                }
            }

            visited.Remove(course.Id);
            return false;
        }
    }
}