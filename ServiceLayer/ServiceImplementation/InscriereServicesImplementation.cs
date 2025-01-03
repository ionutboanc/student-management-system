using DataMapper;
using DomainModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.ServiceImplementation
{
    internal class InscriereServicesImplementation : IInscriereDataServices
    {
        public InscriereServicesImplementation() { }

        public void AddInscriere(Inscriere inscriere)
        {
            DAOFactoryMethod.CurrentDAOFactory.InscriereDataServices.AddInscriere(inscriere);
        }

        public void DeleteInscriere(Inscriere inscriere)
        {
            DAOFactoryMethod.CurrentDAOFactory.InscriereDataServices.DeleteInscriere(inscriere);
        }

        public IList<Inscriere> GetAllInscrieri()
        {
            return DAOFactoryMethod.CurrentDAOFactory.InscriereDataServices.GetAllInscrieri();
        }

        public void UpdateInscriere(Inscriere inscriere)
        {
            DAOFactoryMethod.CurrentDAOFactory.InscriereDataServices.UpdateInscriere(inscriere);
        }

        public Inscriere GetInscriereById(int id)
        {
            return DAOFactoryMethod.CurrentDAOFactory.InscriereDataServices.GetInscriereById(id);
        }
    }
}
