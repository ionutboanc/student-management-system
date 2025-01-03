using DataMapper;
using DomainModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.ServiceImplementation
{
    internal class StudentServicesImplementation : IStudentDataServices
    {
        public StudentServicesImplementation() { }

        public void AddStudent(Student student)
        {
            DAOFactoryMethod.CurrentDAOFactory.StudentDataServices.AddStudent(student);
        }

        public void DeleteStudent(Student student)
        {
            DAOFactoryMethod.CurrentDAOFactory.StudentDataServices.DeleteStudent(student);
        }

        public IList<Student> GetAllStudents()
        {
            return DAOFactoryMethod.CurrentDAOFactory.StudentDataServices.GetAllStudenti();
        }

        public void UpdateStudent(Student student)
        {
            DAOFactoryMethod.CurrentDAOFactory.StudentDataServices.UpdateStudent(student);
        }

        public Student GetStudentById(int id)
        {
            return DAOFactoryMethod.CurrentDAOFactory.StudentDataServices.GetStudentById(id);
        }
    }
}
