namespace DataMapper.SqlServerDAO
{
    using System.Collections.Generic;
    using System.Linq;
    using DomainModel;

    /// <summary>
    /// Provides SQL Server data services for managing exams.
    /// </summary>
    public class SQLExamDataServices : IExamDataServices
    {
        /// <summary>
        /// Adds a new exam.
        /// </summary>
        /// <param name="exam">The exam to add.</param>
        public void AddExam(Exam exam)
        {
            using (var context = new MyApplicationContext())
            {
                context.Exams.Add(exam);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Deletes an exam.
        /// </summary>
        /// <param name="exam">The exam to delete.</param>
        public void DeleteExam(Exam exam)
        {
            using (var context = new MyApplicationContext())
            {
                var examToDelete = new Exam { ExamId = exam.ExamId };
                context.Exams.Attach(examToDelete);
                context.Exams.Remove(examToDelete);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Gets all exams.
        /// </summary>
        /// <returns>A list of all exams.</returns>
        public IList<Exam> GetAllExams()
        {
            using (var context = new MyApplicationContext())
            {
                return context.Exams.ToList();
            }
        }

        /// <summary>
        /// Gets an exam by its ID.
        /// </summary>
        /// <param name="examId">The ID of the exam.</param>
        /// <returns>The exam with the specified ID.</returns>
        public Exam GetExamById(int examId)
        {
            using (var context = new MyApplicationContext())
            {
                return context.Exams.SingleOrDefault(e => e.ExamId == examId);
            }
        }

        /// <summary>
        /// Updates an existing exam.
        /// </summary>
        /// <param name="exam">The exam to update.</param>
        public void UpdateExam(Exam exam)
        {
            using (var context = new MyApplicationContext())
            {
                var examToUpdate = context.Exams.SingleOrDefault(e => e.ExamId == exam.ExamId);
                if (examToUpdate != null)
                {
                    examToUpdate.StudentId = exam.StudentId;
                    examToUpdate.CourseId = exam.CourseId;
                    examToUpdate.Grade = exam.Grade;
                    examToUpdate.ExamDate = exam.ExamDate;
                    context.SaveChanges();
                }
            }
        }
    }
}