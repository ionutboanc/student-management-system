using DomainModel.Models;
using System.Collections.Generic;

namespace DataMapper
{
    public interface ICourseDataServices
    {
        IList<Course> GetAllCourses();

        Course GetCourseById(int id);

        void AddCourse(Course course);

        void UpdateCourse(Course course);

        void DeleteCourse(Course course);
    }
}
