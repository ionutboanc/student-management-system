using System;
using System.Collections.Generic;
using System.Linq;
using DomainModel.Models;

namespace DataMapper.SqlServerDAO
{
    /// <summary>
    /// Provides data services for prerequisites.
    /// </summary>
    internal class SQLPrerequisiteDataServices : IPrerequisiteDataServices
    {
        /// <summary>
        /// Adds a new prerequisite.
        /// </summary>
        /// <param name="prerequisite">The prerequisite to add.</param>
        public void AddPrerequisite(Prerequisite prerequisite)
        {
            using (var context = new MyApplicationContext())
            {
                context.Prerequisites.Add(prerequisite);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Deletes an existing prerequisite.
        /// </summary>
        /// <param name="prerequisite">The prerequisite to delete.</param>
        public void DeletePrerequisite(Prerequisite prerequisite)
        {
            using (var context = new MyApplicationContext())
            {
                var existingPrerequisite = new Prerequisite { Id = prerequisite.Id };
                context.Prerequisites.Attach(existingPrerequisite);
                context.Prerequisites.Remove(existingPrerequisite);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Gets all prerequisites.
        /// </summary>
        /// <returns>A list of all prerequisites.</returns>
        public IList<Prerequisite> GetAllPrerequisites()
        {
            using (var context = new MyApplicationContext())
            {
                return context.Prerequisites.ToList();
            }
        }

        /// <summary>
        /// Gets a prerequisite by its ID.
        /// </summary>
        /// <param name="id">The ID of the prerequisite.</param>
        /// <returns>The prerequisite with the specified ID.</returns>
        public Prerequisite GetPrerequisiteById(int id)
        {
            using (var context = new MyApplicationContext())
            {
                return context.Prerequisites.SingleOrDefault(prerequisite => prerequisite.Id == id);
            }
        }

        /// <summary>
        /// Updates an existing prerequisite.
        /// </summary>
        /// <param name="prerequisite">The prerequisite to update.</param>
        public void UpdatePrerequisite(Prerequisite prerequisite)
        {
            using (var context = new MyApplicationContext())
            {
                var existingPrerequisite = context.Prerequisites.SingleOrDefault(p => p.Id == prerequisite.Id);
                if (existingPrerequisite != null)
                {
                    existingPrerequisite.CourseId = prerequisite.CourseId;
                    existingPrerequisite.PrerequisiteCourseId = prerequisite.PrerequisiteCourseId;
                    existingPrerequisite.MinimumGrade = prerequisite.MinimumGrade;

                    context.SaveChanges();
                }
            }
        }
    }
}