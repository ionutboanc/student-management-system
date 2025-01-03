using DataMapper;
using DomainModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.ServiceImplementation
{
    internal class PrerequisiteServicesImplementation : IPrerequisiteDataServices
    {
        public PrerequisiteServicesImplementation() { }

        public void AddPrerequisite(Prerequisite prerequisite)
        {
            DAOFactoryMethod.CurrentDAOFactory.PrerequisiteDataServices.AddPrerequisite(prerequisite);
        }

        public void DeletePrerequisite(Prerequisite prerequisite)
        {
            DAOFactoryMethod.CurrentDAOFactory.PrerequisiteDataServices.DeletePrerequisite(prerequisite);
        }

        public IList<Prerequisite> GetAllPrerequisites()
        {
            return DAOFactoryMethod.CurrentDAOFactory.PrerequisiteDataServices.GetAllPrerequisites();
        }

        public void UpdatePrerequisite(Prerequisite prerequisite)
        {
            DAOFactoryMethod.CurrentDAOFactory.PrerequisiteDataServices.UpdatePrerequisite(prerequisite);
        }

        public Prerequisite GetPrerequisiteById(int id)
        {
            return DAOFactoryMethod.CurrentDAOFactory.PrerequisiteDataServices.GetPrerequisiteById(id);
        }   
    }
}
