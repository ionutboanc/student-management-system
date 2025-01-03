using DataMapper;
using DomainModel.Models;
using System.Collections.Generic;

namespace ServiceLayer.ServiceImplementation
{
    internal class CursServicesImplementation : ICursDataServices
    {
        public CursServicesImplementation() { }

        public void AddCurs(Curs curs)
        {
            DAOFactoryMethod.CurrentDAOFactory.CursDataServices.AddCurs(curs);
        }

        public void DeleteCurs(Curs curs)
        {
            DAOFactoryMethod.CurrentDAOFactory.CursDataServices.DeleteCurs(curs);
        }

        public IList<Curs> GetAllCursuri()
        {
            return DAOFactoryMethod.CurrentDAOFactory.CursDataServices.GetAllCursuri();
        }


        public void UpdateCurs(Curs curs)
        {
            DAOFactoryMethod.CurrentDAOFactory.CursDataServices.UpdateCurs(curs);
        }

        public Curs GetCursById(int id)
        {
            return DAOFactoryMethod.CurrentDAOFactory.CursDataServices.GetCursById(id);
        }

    }
}
