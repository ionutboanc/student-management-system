using DataMapper;
using DomainModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.ServiceImplementation
{
    internal class SemestruServicesImplementation : ISemestruDataServices
    {
        public SemestruServicesImplementation() { }

        public void AddSemestru(Semestru semestru)
        {
            DAOFactoryMethod.CurrentDAOFactory.SemestruDataServices.AddSemestru(semestru);
        }

        public void DeleteSemestru(Semestru semestru)
        {
            DAOFactoryMethod.CurrentDAOFactory.SemestruDataServices.DeleteSemestru(semestru);
        }

        public IList<Semestru> GetAllSemestre()
        {
            return DAOFactoryMethod.CurrentDAOFactory.SemestruDataServices.GetAllSemestre();
        }

        public void UpdateSemestru(Semestru semestru)
        {
            DAOFactoryMethod.CurrentDAOFactory.SemestruDataServices.UpdateSemestru(semestru);
        }

        public Semestru GetSemestruById(int id)
        {
            return DAOFactoryMethod.CurrentDAOFactory.SemestruDataServices.GetSemestruById(id);
        }
    }
}
