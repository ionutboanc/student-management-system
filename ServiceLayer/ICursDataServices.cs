using DomainModel.Models;
using System.Collections.Generic;

namespace DataMapper
{
    public interface ICursDataServices
    {
        IList<Curs> GetAllCursuri();

        Curs GetCursById(int id);

        void AddCurs(Curs curs);

        void UpdateCurs(Curs curs);

        void DeleteCurs(Curs curs);
    }
}
