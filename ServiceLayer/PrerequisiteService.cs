using DataMapper;
using DomainModel.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ServiceLayer
{
    public class PrerequisiteService : IPrerequisiteDataServices
    {
        public PrerequisiteService() { }

        public void AddPrerequisite(Prerequisite prerequisite)
        {
            ValidatePrerequisite(prerequisite);
            DAOFactoryMethod.CurrentDAOFactory.PrerequisiteDataServices.AddPrerequisite(prerequisite);
            Logger.LogInfo($"Prerequisite added: {prerequisite.Id}");
        }

        public void DeletePrerequisite(Prerequisite prerequisite)
        {
            if (prerequisite == null)
            {
                Logger.LogError("Prerequisite cannot be null.", new ArgumentNullException(nameof(prerequisite)));
                throw new ArgumentNullException(nameof(prerequisite), "Prerequisite cannot be null.");
            }
            DAOFactoryMethod.CurrentDAOFactory.PrerequisiteDataServices.DeletePrerequisite(prerequisite);
            Logger.LogInfo($"Prerequisite deleted: {prerequisite.Id}");
        }

        public IList<Prerequisite> GetAllPrerequisites()
        {
            return DAOFactoryMethod.CurrentDAOFactory.PrerequisiteDataServices.GetAllPrerequisites();
        }

        public void UpdatePrerequisite(Prerequisite prerequisite)
        {
            ValidatePrerequisite(prerequisite);
            DAOFactoryMethod.CurrentDAOFactory.PrerequisiteDataServices.UpdatePrerequisite(prerequisite);
        }

        public Prerequisite GetPrerequisiteById(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id), "Prerequisite ID must be greater than zero.");
            }
            return DAOFactoryMethod.CurrentDAOFactory.PrerequisiteDataServices.GetPrerequisiteById(id);
        }

        public IList<Prerequisite> GetPrerequisitesForCourse(int courseId)
        {
            return DAOFactoryMethod.CurrentDAOFactory.PrerequisiteDataServices.GetAllPrerequisites()
                .Where(p => p.CourseId == courseId).ToList();
        }

        public void ValidatePrerequisiteStructure(int courseId)
        {
            var visited = new HashSet<int>();
            var stack = new HashSet<int>();

            if (HasCircularDependency(courseId, visited, stack))
            {
                throw new ValidationException("Circular dependency detected in prerequisites.");
            }
        }

        private bool HasCircularDependency(int courseId, HashSet<int> visited, HashSet<int> stack)
        {
            if (stack.Contains(courseId))
            {
                return true;
            }

            if (visited.Contains(courseId))
            {
                return false;
            }

            visited.Add(courseId);
            stack.Add(courseId);

            var prerequisites = GetPrerequisitesForCourse(courseId);
            foreach (var prerequisite in prerequisites)
            {
                if (HasCircularDependency(prerequisite.PrerequisiteCourseId, visited, stack))
                {
                    return true;
                }
            }

            stack.Remove(courseId);
            return false;
        }

        public bool CheckStudentEligibilityForCourse(int studentId, int courseId)
        {
            var student = DAOFactoryMethod.CurrentDAOFactory.StudentDataServices.GetStudentById(studentId);
            if (student == null)
            {
                throw new ArgumentException("Student not found.", nameof(studentId));
            }

            var prerequisites = GetPrerequisitesForCourse(courseId);
            foreach (var prerequisite in prerequisites)
            {
                var enrollment = student.Enrollments.FirstOrDefault(e => e.CourseId == prerequisite.PrerequisiteCourseId);
                if (enrollment == null || enrollment.Grade < prerequisite.MinimumGrade)
                {
                    return false;
                }
            }

            return true;
        }

        private void ValidatePrerequisite(Prerequisite prerequisite)
        {
            if (prerequisite == null)
            {
                throw new ArgumentNullException(nameof(prerequisite), "Prerequisite cannot be null.");
            }

            var context = new ValidationContext(prerequisite, null, null);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(prerequisite, context, results, true);

            if (!isValid)
            {
                throw new ValidationException("Prerequisite validation failed: " + string.Join(", ", results.Select(r => r.ErrorMessage)));
            }

            ValidatePrerequisiteCourses(prerequisite);
        }

        private void ValidatePrerequisiteCourses(Prerequisite prerequisite)
        {
            var course = DAOFactoryMethod.CurrentDAOFactory.CourseDataServices.GetCourseById(prerequisite.CourseId);
            var prerequisiteCourse = DAOFactoryMethod.CurrentDAOFactory.CourseDataServices.GetCourseById(prerequisite.PrerequisiteCourseId);

            if (course == null)
            {
                throw new ValidationException("The course for the prerequisite does not exist.");
            }

            if (prerequisiteCourse == null)
            {
                throw new ValidationException("The prerequisite course does not exist.");
            }

            if (course.AvailableSemesters.Min(s => s.Number) <= prerequisiteCourse.AvailableSemesters.Max(s => s.Number))
            {
                throw new ValidationException("Prerequisite course must be available in a semester before the course.");
            }
        }
    }
}