using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataMapper
{
    public interface IDAOFactory
    {
        ICourseDataServices CourseDataServices
        {
            get;
        }

        IStudentDataServices StudentDataServices
        {
            get;
        }

        ISemesterDataServices SemesterDataServices
        {
            get;
        }

        IEnrollmentDataServices EnrollmentDataServices
        {
            get;
        }

        IPrerequisiteDataServices PrerequisiteDataServices
        {
            get;
        }
    }
}
