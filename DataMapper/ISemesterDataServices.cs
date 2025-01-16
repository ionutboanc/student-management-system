// <copyright file="{fileName}" company="Transilvania University of Brasov">
// Copyright (c) 2025 Boanc Ionut-Silviu. All rights reserved.
// </copyright>

namespace DataMapper
{
    using System.Collections.Generic;
    using DomainModel.Models;

    /// <summary>
    /// Provides an interface for semester data services.
    /// </summary>
    public interface ISemesterDataServices
    {
        /// <summary>
        /// Gets all semesters.
        /// </summary>
        /// <returns>A list of all semesters.</returns>
        IList<Semester> GetAllSemesters();

        /// <summary>
        /// Adds a new semester.
        /// </summary>
        /// <param name="semestru">The semester to add.</param>
        void AddSemester(Semester semestru);

        /// <summary>
        /// Deletes a semester.
        /// </summary>
        /// <param name="semestru">The semester to delete.</param>
        void DeleteSemester(Semester semestru);

        /// <summary>
        /// Updates an existing semester.
        /// </summary>
        /// <param name="semestru">The semester to update.</param>
        void UpdateSemester(Semester semestru);

        /// <summary>
        /// Gets a semester by its ID.
        /// </summary>
        /// <param name="id">The ID of the semester.</param>
        /// <returns>The semester with the specified ID.</returns>
        Semester GetSemesterById(int id);
    }
}