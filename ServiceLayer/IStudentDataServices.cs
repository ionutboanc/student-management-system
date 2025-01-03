using DomainModel.Models;
using System.Collections.Generic;

namespace DataMapper
{
    public interface IStudentDataServices
    {
        IList<Student> GetAllStudents();

        Student GetStudentById(int id);

        void AddStudent(Student student);

        void UpdateStudent(Student student);

        void DeleteStudent(Student student);
    }
}
