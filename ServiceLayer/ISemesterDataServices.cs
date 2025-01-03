using DomainModel.Models;
using System.Collections.Generic;

namespace DataMapper
{
    public interface ISemesterDataServices
    {
        IList<Semester> GetAllSemesters();

        Semester GetSemesterById(int id);

        void AddSemester(Semester semester);

        void UpdateSemester(Semester semester);

        void DeleteSemester(Semester semester);
    }
}
