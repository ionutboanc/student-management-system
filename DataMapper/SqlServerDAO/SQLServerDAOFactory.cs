namespace DataMapper.SqlServerDAO
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Provides a factory for creating SQL Server data services.
    /// </summary>
    internal class SQLServerDAOFactory : IDAOFactory
    {
        /// <summary>
        /// Gets the course data services.
        /// </summary>
        public ICourseDataServices CourseDataServices
        {
            get
            {
                return new SQLCourseDataServices();
            }
        }

        /// <summary>
        /// Gets the student data services.
        /// </summary>
        public IStudentDataServices StudentDataServices
        {
            get
            {
                return new SQLStudentDataServices();
            }
        }

        /// <summary>
        /// Gets the semester data services.
        /// </summary>
        public ISemesterDataServices SemesterDataServices
        {
            get
            {
                return new SQLSemesterDataServices();
            }
        }

        /// <summary>
        /// Gets the exam data services.
        /// </summary>
        public IExamDataServices ExamDataServices
        {
            get
            {
                return new SQLExamDataServices();
            }
        }

        /// <summary>
        /// Gets the discount data services.
        /// </summary>
        public IDiscountDataServices DiscountDataServices
        {
            get
            {
                return new SQLDiscountDataServices();
            }
        }
    }
}