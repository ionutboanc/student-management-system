using DomainModel.Models;
using System.Collections.Generic;

namespace DataMapper
{
    public interface IPrerequisiteDataServices
    {
        IList<Prerequisite> GetAllPrerequisites();

        Prerequisite GetPrerequisiteById(int id);

        void AddPrerequisite(Prerequisite prerequisite);

        void UpdatePrerequisite(Prerequisite prerequisite);

        void DeletePrerequisite(Prerequisite prerequisite);
    }
}
