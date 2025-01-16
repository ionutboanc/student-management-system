namespace DataMapper
{
    using System.Collections.Generic;
    using DomainModel;

    /// <summary>
    /// Provides an interface for exam data services.
    /// </summary>
    public interface IExamDataServices
    {
        /// <summary>
        /// Adds a new exam.
        /// </summary>
        /// <param name="exam">The exam to add.</param>
        void AddExam(Exam exam);

        /// <summary>
        /// Updates an existing exam.
        /// </summary>
        /// <param name="exam">The exam to update.</param>
        void UpdateExam(Exam exam);

        /// <summary>
        /// Deletes an exam.
        /// </summary>
        /// <param name="exam">The exam to delete.</param>
        void DeleteExam(Exam exam);

        /// <summary>
        /// Gets an exam by its ID.
        /// </summary>
        /// <param name="examId">The ID of the exam.</param>
        /// <returns>The exam with the specified ID.</returns>
        Exam GetExamById(int examId);

        /// <summary>
        /// Gets all exams.
        /// </summary>
        /// <returns>A list of all exams.</returns>
        IList<Exam> GetAllExams();
    }
}