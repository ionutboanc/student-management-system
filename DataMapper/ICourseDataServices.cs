using DomainModel.Models;
using System.Collections.Generic;

namespace DataMapper
{
    public interface ICourseDataServices
    {
        IList<Course> GetAllCourses();
        void AddCourse(Course course);
        void DeleteCourse(Course course);
        void UpdateCourse(Course course);
        Course GetCourseById(int id);
    }
}
