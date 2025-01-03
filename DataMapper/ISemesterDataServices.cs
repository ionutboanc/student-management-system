using DomainModel.Models;
using System.Collections.Generic;

namespace DataMapper
{
    public interface ISemesterDataServices
    {
        IList<Semester> GetAllSemesters();
        void AddSemester(Semester semestru);
        void DeleteSemester(Semester semestru);
        void UpdateSemester(Semester semestru);

        Semester GetSemesterById(int id);
    }
}
