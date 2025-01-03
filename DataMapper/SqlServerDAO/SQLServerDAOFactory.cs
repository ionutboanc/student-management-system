namespace DataMapper.SqlServerDAO
{
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
    class SQLServerDAOFactory : IDAOFactory
    {
        public ICourseDataServices CourseDataServices
        {
            get
            {
                return new SQLCourseDataServices();
            }
        }

        public IStudentDataServices StudentDataServices
        {
            get
            {
                return new SQLStudentDataServices();
            }
        }

        public ISemesterDataServices SemesterDataServices
        {
            get
            {
                return new SQLSemesterDataServices();
            }
        }

        public IEnrollmentDataServices EnrollmentDataServices
        {
            get
            {
                return new SQLEnrollmentDataServices();
            }
        }

        public IPrerequisiteDataServices PrerequisiteDataServices
        {
            get
            {
                return new SQLPrerequisiteDataServices();
            }
        }
    }
}
