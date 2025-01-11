using DataMapper;
using DomainModel.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ServiceLayer
{
    public class CourseService : ICourseDataServices
    {
        public CourseService() { }

        public void AddCourse(Course course)
        {
            ValidateCourse(course);
            DAOFactoryMethod.CurrentDAOFactory.CourseDataServices.AddCourse(course);
        }

        public void DeleteCourse(Course course)
        {
            if (course == null)
            {
                throw new ArgumentNullException(nameof(course), "Course cannot be null.");
            }
            DAOFactoryMethod.CurrentDAOFactory.CourseDataServices.DeleteCourse(course);
        }

        public IList<Course> GetAllCourses()
        {
            return DAOFactoryMethod.CurrentDAOFactory.CourseDataServices.GetAllCourses();
        }

        public void UpdateCourse(Course course)
        {
            ValidateCourse(course);
            DAOFactoryMethod.CurrentDAOFactory.CourseDataServices.UpdateCourse(course);
        }

        public Course GetCourseById(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id), "Course ID must be greater than zero.");
            }
            return DAOFactoryMethod.CurrentDAOFactory.CourseDataServices.GetCourseById(id);
        }

        public decimal GetCost(Course course)
        {
            ValidateCourse(course);
            return course.GetCost();
        }

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
                throw new ValidationException("Course cost is not within the valid range.");
            }

            ValidateNoCircularDependencies(course);
        }

        private void ValidateNoCircularDependencies(Course course)
        {
            var visited = new HashSet<int>();
            if (HasCircularDependency(course, visited))
            {
                throw new InvalidOperationException("Circular dependency detected in course prerequisites.");
            }
        }

        private bool HasCircularDependency(Course course, HashSet<int> visited)
        {
            if (visited.Contains(course.Id))
            {
                return true;
            }

            visited.Add(course.Id);

            foreach (var prerequisite in course.Prerequisites)
            {
                if (HasCircularDependency(prerequisite.PrerequisiteCourse, visited))
                {
                    return true;
                }
            }

            visited.Remove(course.Id);
            return false;
        }
    }
}