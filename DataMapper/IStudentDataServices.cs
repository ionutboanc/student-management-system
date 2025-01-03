using DomainModel.Models;
using System.Collections.Generic;

namespace DataMapper
{
    public interface IStudentDataServices
    {
        IList<Student> GetAllStudents();
        void AddStudent(Student student);
        void DeleteStudent(Student student);
        void UpdateStudent(Student student);
        Student GetStudentById(int id);
    }
}
