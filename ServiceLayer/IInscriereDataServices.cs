using DomainModel.Models;
using System.Collections.Generic;

namespace DataMapper
{
    public interface IInscriereDataServices
    {
        IList<Inscriere> GetAllInscrieri();

        Inscriere GetInscriereById(int id);

        void AddInscriere(Inscriere inscriere);

        void UpdateInscriere(Inscriere inscriere);

        void DeleteInscriere(Inscriere inscriere);
    }
}
