// <copyright file="{fileName}" company="Transilvania University of Brasov">
// Copyright (c) 2025 Boanc Ionut-Silviu. All rights reserved.
// </copyright>

namespace ServiceLayer
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using DataMapper;
    using DomainModel;
    using DomainModel.Models;
    using log4net;

    /// <summary>
    /// Provides services for managing students.
    /// </summary>
    public class StudentService : IStudentService
    {
        /// <summary>
        /// Logger instance for logging.
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(StudentService));

        /// <summary>
        /// Initializes a new instance of the <see cref="StudentService"/> class.
        /// </summary>
        public StudentService()
        {
            Logger.Info("StudentService initialized.");
        }

        /// <summary>
        /// Adds a new student.
        /// </summary>
        /// <param name="student">The student to add.</param>
        public void AddStudent(Student student)
        {
            Logger.Info("Adding a new student.");
            this.ValidateStudent(student);
            DAOFactoryMethod.CurrentDAOFactory.StudentDataServices.AddStudent(student);
        }

        /// <summary>
        /// Updates an existing student.
        /// </summary>
        /// <param name="student">The student to update.</param>
        public void UpdateStudent(Student student)
        {
            Logger.Info("Updating an existing student.");
            this.ValidateStudent(student);
            DAOFactoryMethod.CurrentDAOFactory.StudentDataServices.UpdateStudent(student);
        }

        /// <summary>
        /// Deletes a student.
        /// </summary>
        /// <param name="student">The student to delete.</param>
        public void DeleteStudent(Student student)
        {
            Logger.Info("Deleting a student.");
            if (student == null)
            {
                Logger.Error("Student cannot be null.", new ArgumentNullException(nameof(student), "Student cannot be null."));
                throw new ArgumentNullException(nameof(student), "Student cannot be null.");
            }

            DAOFactoryMethod.CurrentDAOFactory.StudentDataServices.DeleteStudent(student);
        }

        /// <summary>
        /// Gets a student by their ID.
        /// </summary>
        /// <param name="studentId">The ID of the student.</param>
        /// <returns>The student with the specified ID.</returns>
        public Student GetStudentById(int studentId)
        {
            Logger.Info($"Getting student by ID: {studentId}.");
            if (studentId <= 0)
            {
                Logger.Error("Student ID must be greater than zero.", new ArgumentOutOfRangeException(nameof(studentId), "Student ID must be greater than zero."));
                throw new ArgumentOutOfRangeException(nameof(studentId), "Student ID must be greater than zero.");
            }

            return DAOFactoryMethod.CurrentDAOFactory.StudentDataServices.GetStudentById(studentId);
        }

        /// <summary>
        /// Gets all students.
        /// </summary>
        /// <returns>A list of all students.</returns>
        public IList<Student> GetAllStudents()
        {
            Logger.Info("Getting all students.");
            return DAOFactoryMethod.CurrentDAOFactory.StudentDataServices.GetAllStudents();
        }

        /// <summary>
        /// Validates a student.
        /// </summary>
        /// <param name="student">The student to validate.</param>
        public void ValidateStudent(Student student)
        {
            Logger.Info("Validating a student.");
            if (student == null)
            {
                Logger.Error("Student cannot be null.", new ArgumentNullException(nameof(student), "Student cannot be null."));
                throw new ArgumentNullException(nameof(student), "Student cannot be null.");
            }

            student.Validate();

            if (!student.HasValidContact())
            {
                Logger.Error("Student must have at least one valid contact method (phone or email).", new ValidationException("Student must have at least one valid contact method (phone or email)."));
                throw new ValidationException("Student must have at least one valid contact method (phone or email).");
            }
        }

        /// <summary>
        /// Gets a student by their national ID.
        /// </summary>
        /// <param name="nationalID">The national ID of the student.</param>
        /// <returns>The student with the specified national ID.</returns>
        public Student GetStudentByNationalID(string nationalID)
        {
            Logger.Info($"Getting student by national ID: {nationalID}.");
            return DAOFactoryMethod.CurrentDAOFactory.StudentDataServices.GetAllStudents().FirstOrDefault(s => s.NationalId == nationalID);
        }

        /// <summary>
        /// Gets the courses a student is enrolled in.
        /// </summary>
        /// <param name="student">The student to get courses for.</param>
        /// <returns>A list of courses the student is enrolled in.</returns>
        public IList<Course> GetEnrolledCourses(Student student)
        {
            Logger.Info("Getting enrolled courses for a student.");
            if (student == null)
            {
                Logger.Error("Student cannot be null.", new ArgumentNullException(nameof(student), "Student cannot be null."));
                throw new ArgumentNullException(nameof(student), "Student cannot be null.");
            }

            return student.EnrolledCourses;
        }

        /// <summary>
        /// Gets the exams for a student.
        /// </summary>
        /// <param name="student">The student to get exams for.</param>
        /// <returns>A list of exams for the student.</returns>
        public IList<Exam> GetExamsForStudent(Student student)
        {
            Logger.Info("Getting exams for a student.");
            if (student == null)
            {
                Logger.Error("Student cannot be null.", new ArgumentNullException(nameof(student), "Student cannot be null."));
                throw new ArgumentNullException(nameof(student), "Student cannot be null.");
            }

            return student.EnrolledCourses.SelectMany(e => e.Exams).Where(ex => ex.StudentId == student.Id).ToList();
        }

        /// <summary>
        /// Gets the current semester for a student.
        /// </summary>
        /// <param name="student">The student to get the current semester for.</param>
        /// <returns>The current semester for the student.</returns>
        public Semester GetCurrentSemester(Student student)
        {
            Logger.Info("Getting current semester for a student.");
            if (student == null)
            {
                Logger.Error("Student cannot be null.", new ArgumentNullException(nameof(student), "Student cannot be null."));
                throw new ArgumentNullException(nameof(student), "Student cannot be null.");
            }

            return student.CurrentSemester;
        }

        /// <summary>
        /// Enrolls a student in a course.
        /// </summary>
        /// <param name="student">The student to enroll.</param>
        /// <param name="course">The course to enroll the student in.</param>
        public void EnrollStudentInCourse(Student student, Course course)
        {
            Logger.Info("Enrolling student in a course.");
            if (student == null)
            {
                Logger.Error("Student cannot be null.", new ArgumentNullException(nameof(student), "Student cannot be null."));
                throw new ArgumentNullException(nameof(student), "Student cannot be null.");
            }

            if (course == null)
            {
                Logger.Error("Course cannot be null.", new ArgumentNullException(nameof(course), "Course cannot be null."));
                throw new ArgumentNullException(nameof(course), "Course cannot be null.");
            }

            if (student.EnrolledCourses.Any(e => e.Id == course.Id))
            {
                Logger.Error("Student is already enrolled in this course.", new InvalidOperationException("Student is already enrolled in this course."));
                throw new InvalidOperationException("Student is already enrolled in this course.");
            }

            this.ValidatePrerequisites(student, course);

            student.EnrolledCourses.Add(course);
            student.DeductBalance(course.GetCost());
            DAOFactoryMethod.CurrentDAOFactory.StudentDataServices.UpdateStudent(student);
        }

        /// <summary>
        /// Enrolls a student in multiple courses.
        /// </summary>
        /// <param name="student">The student to enroll.</param>
        /// <param name="courses">The list of courses to enroll the student in.</param>
        public void EnrollStudentInCourses(Student student, List<Course> courses)
        {
            Logger.Info("Enrolling student in multiple courses.");
            if (student == null)
            {
                Logger.Error("Student cannot be null.", new ArgumentNullException(nameof(student), "Student cannot be null."));
                throw new ArgumentNullException(nameof(student), "Student cannot be null.");
            }

            if (courses == null || !courses.Any())
            {
                Logger.Error("Courses cannot be null or empty.", new ArgumentNullException(nameof(courses), "Courses cannot be null or empty."));
                throw new ArgumentNullException(nameof(courses), "Courses cannot be null or empty.");
            }

            foreach (var course in courses)
            {
                this.EnrollStudentInCourse(student, course);
            }

            DAOFactoryMethod.CurrentDAOFactory.StudentDataServices.UpdateStudent(student);
        }

        /// <summary>
        /// Removes a student from a course.
        /// </summary>
        /// <param name="student">The student to remove.</param>
        /// <param name="course">The course to remove the student from.</param>
        public void RemoveStudentFromCourse(Student student, Course course)
        {
            Logger.Info("Removing student from a course.");
            if (student == null)
            {
                Logger.Error("Student cannot be null.", new ArgumentNullException(nameof(student), "Student cannot be null."));
                throw new ArgumentNullException(nameof(student), "Student cannot be null.");
            }

            if (course == null)
            {
                Logger.Error("Course cannot be null.", new ArgumentNullException(nameof(course), "Course cannot be null."));
                throw new ArgumentNullException(nameof(course), "Course cannot be null.");
            }

            var enrolledCourse = student.EnrolledCourses.FirstOrDefault(c => c.Id == course.Id);

            if (enrolledCourse == null)
            {
                Logger.Error("Student is not enrolled in this course.", new InvalidOperationException("Student is not enrolled in this course."));
                throw new InvalidOperationException("Student is not enrolled in this course.");
            }

            student.EnrolledCourses.Remove(enrolledCourse);
            DAOFactoryMethod.CurrentDAOFactory.StudentDataServices.UpdateStudent(student);
        }

        /// <summary>
        /// Promotes a student to the next semester.
        /// </summary>
        /// <param name="student">The student to promote.</param>
        /// <returns>True if the student was promoted, otherwise false.</returns>
        public bool PromoteStudentToNextSemester(Student student)
        {
            Logger.Info("Promoting student to the next semester.");
            if (student == null)
            {
                Logger.Error("Student cannot be null.", new ArgumentNullException(nameof(student), "Student cannot be null."));
                throw new ArgumentNullException(nameof(student), "Student cannot be null.");
            }

            var currentSemester = student.CurrentSemester;
            if (currentSemester == null)
            {
                Logger.Error("Current semester for student is not set.", new ArgumentException("Current semester for student is not set."));
                throw new ArgumentException("Current semester for student is not set.");
            }

            var exams = student.EnrolledCourses
                              .SelectMany(course => course.Exams)
                              .Where(exam => exam.Grade >= 5)
                              .ToList();

            int totalCredits = exams.Sum(exam => exam.Course.Credits);

            if (totalCredits >= currentSemester.CreditThreshold)
            {
                var nextSemester = DAOFactoryMethod.CurrentDAOFactory.SemesterDataServices.GetSemesterById(currentSemester.Number + 1);
                if (nextSemester != null)
                {
                    student.CurrentSemester = nextSemester;
                    student.CurrentSemesterId = nextSemester.Id;
                    student.EnrolledCourses.Clear();
                    DAOFactoryMethod.CurrentDAOFactory.StudentDataServices.UpdateStudent(student);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the credits per semester for a student.
        /// </summary>
        /// <param name="student">The student to get credits for.</param>
        /// <returns>A dictionary with the semester ID as the key and the total credits as the value.</returns>
        public Dictionary<int, int> GetCreditsPerSemester(Student student)
        {
            Logger.Info("Getting credits per semester for a student.");
            if (student == null)
            {
                Logger.Error("Student cannot be null.", new ArgumentNullException(nameof(student), "Student cannot be null."));
                throw new ArgumentNullException(nameof(student), "Student cannot be null.");
            }

            return student.EnrolledCourses
                .GroupBy(e => e.AvailableSemesters.First().Id)
                .ToDictionary(g => g.Key, g => g.Sum(e => e.Credits));
        }

        /// <summary>
        /// Calculates the total cost of courses for a student.
        /// </summary>
        /// <param name="student">The student to calculate the cost for.</param>
        /// <returns>The total cost of courses for the student.</returns>
        public decimal CalculateCoursesCostsForStudent(Student student)
        {
            Logger.Info("Calculating total cost of courses for a student.");
            if (student == null)
            {
                Logger.Error("Student cannot be null.", new ArgumentNullException(nameof(student), "Student cannot be null."));
                throw new ArgumentNullException(nameof(student), "Student cannot be null.");
            }

            return student.EnrolledCourses.Sum(c => c.GetCost());
        }

        /// <summary>
        /// Calculates the average amount spent by students.
        /// </summary>
        /// <returns>The average amount spent by students.</returns>
        public decimal CalculateAverageSpent()
        {
            Logger.Info("Calculating average amount spent by students.");
            var students = DAOFactoryMethod.CurrentDAOFactory.StudentDataServices.GetAllStudents();
            if (students == null || !students.Any())
            {
                return 0m;
            }

            var totalSpent = students.Sum(student => student.EnrolledCourses.Sum(course => course.GetCost()));
            return totalSpent / students.Count;
        }

        /// <summary>
        /// Counts the number of students who have passed all their courses.
        /// </summary>
        /// <returns>The number of students who have passed all their courses.</returns>
        public int CountStudentsWhoPassed()
        {
            Logger.Info("Counting students who have passed all their courses.");
            return DAOFactoryMethod.CurrentDAOFactory.StudentDataServices.GetAllStudents()
                .Count(s => s.EnrolledCourses.All(e => e.Exams.All(ex => ex.Grade >= 5)));
        }

        /// <summary>
        /// Validates the prerequisites for a course.
        /// </summary>
        /// <param name="student">The student to validate prerequisites for.</param>
        /// <param name="course">The course to validate prerequisites for.</param>
        private void ValidatePrerequisites(Student student, Course course)
        {
            Logger.Info("Validating prerequisites for a course.");
            foreach (var prerequisite in course.Prerequisites)
            {
                var enrollment = student.EnrolledCourses.FirstOrDefault(e => e.Id == prerequisite.Id);
                if (enrollment == null)
                {
                    // Check if the student has completed the prerequisite course in the past
                    var completedCourse = student.Exams.FirstOrDefault(ex => ex.CourseId == prerequisite.Id && ex.Grade >= 5);
                    if (completedCourse == null)
                    {
                        Logger.Error($"Student has not met the prerequisite requirements for course {course.Name}.", new InvalidOperationException($"Student has not met the prerequisite requirements for course {course.Name}."));
                        throw new InvalidOperationException($"Student has not met the prerequisite requirements for course {course.Name}.");
                    }
                }
                else if (enrollment.Exams.All(ex => ex.Grade < 5))
                {
                    Logger.Error($"Student has not met the prerequisite requirements for course {course.Name}.", new InvalidOperationException($"Student has not met the prerequisite requirements for course {course.Name}."));
                    throw new InvalidOperationException($"Student has not met the prerequisite requirements for course {course.Name}.");
                }
            }
        }
    }
}