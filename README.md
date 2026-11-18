# 🎓 Student Management System

Student Management System is the back-end of an application for running a university's academic records. It handles students, courses, semesters, exams and tuition, and makes sure every action follows the university's rules, such as course prerequisites, exam attempt limits and payments.

It is a university project from Transilvania University of Brașov, completed in January 2025.

## 💡 Why this project

Universities manage a large amount of connected information. A student enrolls in courses, courses depend on other courses, exams decide who moves on to the next semester, and every enrollment has a cost. When these rules are handled by hand or spread across spreadsheets, mistakes are easy to make and hard to spot.

This project brings all of that logic into one place. The focus was on building it the way professional business software is built: clearly separated layers, rules that are checked automatically, detailed logging and a large set of automated tests that prove everything works as intended.

## ✨ What it does

👩‍🎓 **Student records.** Stores each student's personal details, contact information, current semester and account balance.

📚 **Courses and prerequisites.** Courses have credits, a cost and optional prerequisites. The system blocks enrollment until the required courses are passed, and it detects circular dependencies, where two courses would require each other.

💳 **Enrollment and payments.** Enrolling in a course deducts its cost from the student's balance, with support for course discounts.

📝 **Exams and grades.** Records exam results, prevents a student from having two exams on the same day and limits each student to two attempts per course. If a student uses up both attempts, half of the course fee is refunded automatically.

📈 **Semester progression.** Tracks credits earned per semester and promotes a student to the next semester once they reach the required number of credits.

📊 **Reports.** Calculates statistics such as average grades per course, average spending per student, average number of courses per student and how many students passed all their courses.

## 🛠️ How it was built

The application is written in C# and organized into separate layers, each with a single responsibility. This is a common structure in enterprise software, because it keeps the code easy to test, maintain and extend.

| Layer | Responsibility |
|---|---|
| Domain Model | The core concepts: students, courses, semesters, exams and discounts |
| Data Access | Saving and loading data from a SQL Server database |
| Service Layer | The business rules, validation and logging |
| Tests | 213 automated tests covering the domain model and the business rules |

## 🧰 Technologies

| Area | Tools |
|---|---|
| Language | C# |
| Framework | .NET Framework 4.8 |
| Database | SQL Server, Entity Framework 6 |
| Logging | log4net |
| Testing | MSTest, Moq |
| Code quality | StyleCop |
