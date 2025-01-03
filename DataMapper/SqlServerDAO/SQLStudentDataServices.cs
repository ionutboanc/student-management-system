using DomainModel.Models;
using System.Collections.Generic;
using System.Linq;

namespace DataMapper.SqlServerDAO
{
    internal class SQLStudentDataServices : IStudentDataServices
    {
        public IList<Student> GetAllStudents()
        {
            using (var context = new MyApplicationContext())
            {
                return context.Students.ToList();
            }
        }

        public Student GetStudentById(int id)
        {
            using (var context = new MyApplicationContext())
            {
                return context.Students.SingleOrDefault(s => s.Id == id);
            }
        }

        public void AddStudent(Student student)
        {
            using (var context = new MyApplicationContext())
            {
                context.Students.Add(student);
                context.SaveChanges();
            }
        }

        public void DeleteStudent(Student student)
        {
            using (var context = new MyApplicationContext())
            {
                var studentToDelete = new Student { Id = student.Id };
                context.Students.Attach(studentToDelete);
                context.Students.Remove(studentToDelete);
                context.SaveChanges();
            }
        }

        public void UpdateStudent(Student student)
        {
            using (var context = new MyApplicationContext())
            {
                context.Entry(student).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            }
        }
    }
}
