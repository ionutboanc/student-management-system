using DomainModel.Models;
using System.Collections.Generic;

namespace DataMapper
{
    public interface IEnrollmentDataServices
    {
        IList<Enrollment> GetAllEnrollments();

        Enrollment GetEnrollmentById(int id);

        void AddEnrollment(Enrollment enrollment);

        void UpdateEnrollment(Enrollment enrollment);

        void DeleteEnrollment(Enrollment enrollment);
    }
}
