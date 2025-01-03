using DomainModel.Models;
using System.Collections.Generic;

namespace DataMapper
{
    public interface ISemestruDataServices
    {
        IList<Semestru> GetAllSemestre();

        Semestru GetSemestruById(int id);

        void AddSemestru(Semestru semestru);

        void UpdateSemestru(Semestru semestru);

        void DeleteSemestru(Semestru semestru);
    }
}
