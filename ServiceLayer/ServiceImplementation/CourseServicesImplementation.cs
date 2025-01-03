using DataMapper;
using DomainModel.Models;
using System.Collections.Generic;

namespace ServiceLayer.ServiceImplementation
{
    internal class CourseServicesImplementation : ICourseDataServices
    {
        public CourseServicesImplementation() { }

        public void AddCourse(Course course)
        {
            DAOFactoryMethod.CurrentDAOFactory.CourseDataServices.AddCourse(course);
        }

        public void DeleteCourse(Course course)
        {
            DAOFactoryMethod.CurrentDAOFactory.CourseDataServices.DeleteCourse(course);
        }

        public Course GetCourseById(int id)
        {
            return DAOFactoryMethod.CurrentDAOFactory.CourseDataServices.GetCourseById(id);
        }

        public void UpdateCourse(Course course)
        {
            DAOFactoryMethod.CurrentDAOFactory.CourseDataServices.UpdateCourse(course);
        }

        public IList<Course> GetAllCourses()
        {
            return DAOFactoryMethod.CurrentDAOFactory.CourseDataServices.GetAllCourses();
        }
    }

    }
