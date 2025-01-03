namespace DataMapper
{
using System.Collections.Generic;
using DomainModel.Models;

public interface IEnrollmentDataServices
    {
        /// <summary>
        /// Adds a new enrollment.
        /// </summary>
        /// <param name="enrollment">The enrollment to add.</param>
        void AddEnrollment(Enrollment enrollment);

        /// <summary>
        /// Deletes an existing enrollment.
        /// </summary>
        /// <param name="enrollment">The enrollment to delete.</param>
        void DeleteEnrollment(Enrollment enrollment);

        /// <summary>
        /// Updates an existing enrollment.
        /// </summary>
        /// <param name="enrollment">The enrollment to update.</param>
        void UpdateEnrollment(Enrollment enrollment);

        /// <summary>
        /// Gets all enrollment.
        /// </summary>
        /// <returns>A list of all enrollment.</returns>
        IList<Enrollment> GetAllEnrollments();

        /// <summary>
        /// Gets an enrollment by its identifier.
        /// </summary>
        /// <param name="id">The enrollment's identifier.</param>
        /// <returns>The enrollment with the specified ID.</returns>
        Enrollment GetEnrollmentById(int id);
    }
}
