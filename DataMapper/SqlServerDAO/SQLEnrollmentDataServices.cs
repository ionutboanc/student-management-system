using System;
using System.Collections.Generic;
using System.Linq;
using DomainModel.Models;

namespace DataMapper.SqlServerDAO
{
    /// <summary>
    /// Provides data services for enrollments.
    /// </summary>
    internal class SQLEnrollmentDataServices : IEnrollmentDataServices
    {
        /// <summary>
        /// Adds an enrollment.
        /// </summary>
        /// <param name="enrollment">The enrollment to add.</param>
        public void AddEnrollment(Enrollment enrollment)
        {
            using (var context = new MyApplicationContext())
            {
                context.Enrollments.Add(enrollment);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Deletes an enrollment.
        /// </summary>
        /// <param name="enrollment">The enrollment to delete.</param>
        public void DeleteEnrollment(Enrollment enrollment)
        {
            using (var context = new MyApplicationContext())
            {
                var existingEnrollment = new Enrollment { Id = enrollment.Id };
                context.Enrollments.Attach(existingEnrollment);
                context.Enrollments.Remove(existingEnrollment);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Gets all enrollments.
        /// </summary>
        /// <returns>A list of all enrollments.</returns>
        public IList<Enrollment> GetAllEnrollments()
        {
            using (var context = new MyApplicationContext())
            {
                return context.Enrollments.ToList();
            }
        }

        /// <summary>
        /// Gets an enrollment by its ID.
        /// </summary>
        /// <param name="id">The ID of the enrollment.</param>
        /// <returns>The enrollment with the specified ID.</returns>
        public Enrollment GetEnrollmentById(int id)
        {
            using (var context = new MyApplicationContext())
            {
                return context.Enrollments.SingleOrDefault(enrollment => enrollment.Id == id);
            }
        }

        /// <summary>
        /// Updates an enrollment.
        /// </summary>
        /// <param name="enrollment">The enrollment to update.</param>
        public void UpdateEnrollment(Enrollment enrollment)
        {
            using (var context = new MyApplicationContext())
            {
                var existingEnrollment = context.Enrollments.SingleOrDefault(e => e.Id == enrollment.Id);
                if (existingEnrollment != null)
                {
                    existingEnrollment.StudentId = enrollment.StudentId;
                    existingEnrollment.CourseId = enrollment.CourseId;
                    existingEnrollment.SemesterId = enrollment.SemesterId;
                    existingEnrollment.NumberOfAttempts = enrollment.NumberOfAttempts;
                    existingEnrollment.LastAttemptDate = enrollment.LastAttemptDate;
                    existingEnrollment.Grade = enrollment.Grade;

                    context.SaveChanges();
                }
            }
        }
    }
}