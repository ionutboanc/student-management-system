// <copyright file="{fileName}" company="Transilvania University of Brasov">
// Copyright (c) 2025 Boanc Ionut-Silviu. All rights reserved.
// </copyright>

namespace DataMapper
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Provides an interface for DAO factories.
    /// </summary>
    public interface IDAOFactory
    {
        /// <summary>
        /// Gets the course data services.
        /// </summary>
        ICourseDataServices CourseDataServices { get; }

        /// <summary>
        /// Gets the student data services.
        /// </summary>
        IStudentDataServices StudentDataServices { get; }

        /// <summary>
        /// Gets the semester data services.
        /// </summary>
        ISemesterDataServices SemesterDataServices { get; }

        /// <summary>
        /// Gets the exam data services.
        /// </summary>
        IExamDataServices ExamDataServices { get; }

        /// <summary>
        /// Gets the discount data services.
        /// </summary>
        IDiscountDataServices DiscountDataServices { get; }
    }
}