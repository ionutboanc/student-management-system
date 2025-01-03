using DomainModel.Models;
using System.Collections.Generic;

namespace DataMapper
{
    public interface IPrerequisiteDataServices
    {
        /// <summary>
        /// Adds a new prerequisite.
        /// </summary>
        /// <param name="prerequisite">The prerequisite to add.</param>
        void AddPrerequisite(Prerequisite prerequisite);

        /// <summary>
        /// Deletes an existing prerequisite.
        /// </summary>
        /// <param name="prerequisite">The prerequisite to delete.</param>
        void DeletePrerequisite(Prerequisite prerequisite);

        /// <summary>
        /// Updates an existing prerequisite.
        /// </summary>
        /// <param name="prerequisite">The prerequisite to update.</param>
        void UpdatePrerequisite(Prerequisite prerequisite);

        /// <summary>
        /// Gets all prerequisites.
        /// </summary>
        /// <returns>A list of all prerequisites.</returns>
        IList<Prerequisite> GetAllPrerequisites();

        /// <summary>
        /// Gets a prerequisite by its identifier.
        /// </summary>
        /// <param name="id">The prerequisite's identifier.</param>
        /// <returns>The prerequisite with the specified ID.</returns>
        Prerequisite GetPrerequisiteById(int id);
    }
}
