namespace ServiceLayer
{
    using System.Collections.Generic;
    using DomainModel.Models;

    /// <summary>
    /// Provides an interface for semester services.
    /// </summary>
    public interface ISemesterService
    {
        /// <summary>
        /// Adds a new semester.
        /// </summary>
        /// <param name="semester">The semester to add.</param>
        void AddSemester(Semester semester);

        /// <summary>
        /// Updates an existing semester.
        /// </summary>
        /// <param name="semester">The semester to update.</param>
        void UpdateSemester(Semester semester);

        /// <summary>
        /// Deletes a semester.
        /// </summary>
        /// <param name="semester">The semester to delete.</param>
        void DeleteSemester(Semester semester);

        /// <summary>
        /// Gets a semester by its ID.
        /// </summary>
        /// <param name="semesterId">The ID of the semester.</param>
        /// <returns>The semester with the specified ID.</returns>
        Semester GetSemesterById(int semesterId);

        /// <summary>
        /// Gets all semesters.
        /// </summary>
        /// <returns>A list of all semesters.</returns>
        IList<Semester> GetAllSemesters();

        /// <summary>
        /// Gets the previous semester.
        /// </summary>
        /// <param name="currentSemester">The current semester.</param>
        /// <returns>The previous semester.</returns>
        Semester GetPreviousSemester(Semester currentSemester);

        /// <summary>
        /// Gets the next semester.
        /// </summary>
        /// <param name="currentSemester">The current semester.</param>
        /// <returns>The next semester.</returns>
        Semester GetNextSemester(Semester currentSemester);

        /// <summary>
        /// Gets the courses in a semester.
        /// </summary>
        /// <param name="semester">The semester to get courses for.</param>
        /// <returns>A list of courses in the semester.</returns>
        IList<Course> GetCoursesInSemester(Semester semester);

        /// <summary>
        /// Gets the students in a semester.
        /// </summary>
        /// <param name="semester">The semester to get students for.</param>
        /// <returns>A list of students in the semester.</returns>
        IList<Student> GetStudentsInSemester(Semester semester);
    }
}