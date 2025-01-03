using System.Collections.Generic;
using System.Linq;
using DomainModel.Models;

namespace DataMapper.SqlServerDAO
{
    /// <summary>
    /// Provides data services for semesters.
    /// </summary>
    internal class SQLSemesterDataServices : ISemesterDataServices
    {
        /// <summary>
        /// Gets all semesters.
        /// </summary>
        /// <returns>A list of all semesters.</returns>
        public IList<Semester> GetAllSemesters()
        {
            using (var context = new MyApplicationContext())
            {
                return context.Semesters.ToList();
            }
        }

        /// <summary>
        /// Gets a semester by its ID.
        /// </summary>
        /// <param name="id">The ID of the semester.</param>
        /// <returns>The semester with the specified ID.</returns>
        public Semester GetSemesterById(int id)
        {
            using (var context = new MyApplicationContext())
            {
                return context.Semesters.SingleOrDefault(s => s.Id == id);
            }
        }

        /// <summary>
        /// Adds a new semester.
        /// </summary>
        /// <param name="semester">The semester to add.</param>
        public void AddSemester(Semester semester)
        {
            using (var context = new MyApplicationContext())
            {
                context.Semesters.Add(semester);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Deletes a semester.
        /// </summary>
        /// <param name="semester">The semester to delete.</param>
        public void DeleteSemester(Semester semester)
        {
            using (var context = new MyApplicationContext())
            {
                var semesterToDelete = new Semester { Id = semester.Id };
                context.Semesters.Attach(semesterToDelete);
                context.Semesters.Remove(semesterToDelete);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Updates a semester.
        /// </summary>
        /// <param name="semester">The semester to update.</param>
        public void UpdateSemester(Semester semester)
        {
            using (var context = new MyApplicationContext())
            {
                context.Entry(semester).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            }
        }
    }
}