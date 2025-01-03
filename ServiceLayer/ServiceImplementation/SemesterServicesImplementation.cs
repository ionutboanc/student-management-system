using DataMapper;
using DomainModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.ServiceImplementation
{
    internal class SemesterServicesImplementation : ISemesterDataServices
    {
        public SemesterServicesImplementation() { }

        public void AddSemester(Semester semester)
        {
            DAOFactoryMethod.CurrentDAOFactory.SemesterDataServices.AddSemester(semester);
        }

        public void DeleteSemester(Semester semester)
        {
            DAOFactoryMethod.CurrentDAOFactory.SemesterDataServices.DeleteSemester(semester);
        }

        public IList<Semester> GetAllSemesters()
        {
            return DAOFactoryMethod.CurrentDAOFactory.SemesterDataServices.GetAllSemesters();
        }

        public void UpdateSemester(Semester semester)
        {
            DAOFactoryMethod.CurrentDAOFactory.SemesterDataServices.UpdateSemester(semester);
        }

        public Semester GetSemesterById(int id)
        {
            return DAOFactoryMethod.CurrentDAOFactory.SemesterDataServices.GetSemesterById(id);
        }


    }
}
