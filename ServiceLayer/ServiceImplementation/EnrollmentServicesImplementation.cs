using DataMapper;
using DomainModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.ServiceImplementation
{
    internal class EnrollmentServicesImplementation : IEnrollmentDataServices
    {
        public EnrollmentServicesImplementation() { }

        public void AddEnrollment(Enrollment enrollment)
        {
            DAOFactoryMethod.CurrentDAOFactory.EnrollmentDataServices.AddEnrollment(enrollment);
        }

        public void DeleteEnrollment(Enrollment enrollment)
        {
            DAOFactoryMethod.CurrentDAOFactory.EnrollmentDataServices.DeleteEnrollment(enrollment);
        }

        public IList<Enrollment> GetAllEnrollments()
        {
            return DAOFactoryMethod.CurrentDAOFactory.EnrollmentDataServices.GetAllEnrollments();
        }

        public void UpdateEnrollment(Enrollment enrollment)
        {
            DAOFactoryMethod.CurrentDAOFactory.EnrollmentDataServices.UpdateEnrollment(enrollment);
        }

        public Enrollment GetEnrollmentById(int id)
        {
            return DAOFactoryMethod.CurrentDAOFactory.EnrollmentDataServices.GetEnrollmentById(id);
        }
    }
}
