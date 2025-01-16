#pragma warning disable // Suppress all warnings

namespace DataMapper.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Final : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Courses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Description = c.String(maxLength: 255),
                        Credits = c.Int(nullable: false),
                        Cost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DiscountId = c.Int(),
                        MinThreshold = c.Decimal(nullable: false, precision: 18, scale: 2),
                        MaxThreshold = c.Decimal(nullable: false, precision: 18, scale: 2),
                        BalanceRefunded = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Discounts", t => t.DiscountId)
                .Index(t => t.DiscountId);
            
            CreateTable(
                "dbo.Semesters",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Number = c.Int(nullable: false),
                        CreditThreshold = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Exams",
                c => new
                    {
                        ExamId = c.Int(nullable: false, identity: true),
                        StudentId = c.Int(nullable: false),
                        SemesterId = c.Int(nullable: false),
                        CourseId = c.Int(nullable: false),
                        Grade = c.Int(nullable: false),
                        ExamDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ExamId)
                .ForeignKey("dbo.Courses", t => t.CourseId)
                .ForeignKey("dbo.Semesters", t => t.SemesterId)
                .ForeignKey("dbo.Students", t => t.StudentId, cascadeDelete: true)
                .Index(t => t.StudentId)
                .Index(t => t.SemesterId)
                .Index(t => t.CourseId);
            
            CreateTable(
                "dbo.Students",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false, maxLength: 40),
                        LastName = c.String(nullable: false, maxLength: 40),
                        Address = c.String(nullable: false, maxLength: 255),
                        NationalId = c.String(nullable: false),
                        Balance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RegistrationNumber = c.String(nullable: false, maxLength: 20),
                        CurrentSemesterId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Semesters", t => t.CurrentSemesterId)
                .Index(t => t.CurrentSemesterId);
            
            CreateTable(
                "dbo.Discounts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Percentage = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.StudentCourses",
                c => new
                    {
                        StudentId = c.Int(nullable: false),
                        CourseId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.StudentId, t.CourseId })
                .ForeignKey("dbo.Students", t => t.StudentId, cascadeDelete: true)
                .ForeignKey("dbo.Courses", t => t.CourseId, cascadeDelete: true)
                .Index(t => t.StudentId)
                .Index(t => t.CourseId);
            
            CreateTable(
                "dbo.CourseSemesters",
                c => new
                    {
                        CourseId = c.Int(nullable: false),
                        SemesterId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.CourseId, t.SemesterId })
                .ForeignKey("dbo.Courses", t => t.CourseId, cascadeDelete: true)
                .ForeignKey("dbo.Semesters", t => t.SemesterId, cascadeDelete: true)
                .Index(t => t.CourseId)
                .Index(t => t.SemesterId);
            
            CreateTable(
                "dbo.CoursePrerequisites",
                c => new
                    {
                        CourseId = c.Int(nullable: false),
                        PrerequisiteId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.CourseId, t.PrerequisiteId })
                .ForeignKey("dbo.Courses", t => t.CourseId)
                .ForeignKey("dbo.Courses", t => t.PrerequisiteId)
                .Index(t => t.CourseId)
                .Index(t => t.PrerequisiteId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CoursePrerequisites", "PrerequisiteId", "dbo.Courses");
            DropForeignKey("dbo.CoursePrerequisites", "CourseId", "dbo.Courses");
            DropForeignKey("dbo.Courses", "DiscountId", "dbo.Discounts");
            DropForeignKey("dbo.CourseSemesters", "SemesterId", "dbo.Semesters");
            DropForeignKey("dbo.CourseSemesters", "CourseId", "dbo.Courses");
            DropForeignKey("dbo.Exams", "StudentId", "dbo.Students");
            DropForeignKey("dbo.StudentCourses", "CourseId", "dbo.Courses");
            DropForeignKey("dbo.StudentCourses", "StudentId", "dbo.Students");
            DropForeignKey("dbo.Students", "CurrentSemesterId", "dbo.Semesters");
            DropForeignKey("dbo.Exams", "SemesterId", "dbo.Semesters");
            DropForeignKey("dbo.Exams", "CourseId", "dbo.Courses");
            DropIndex("dbo.CoursePrerequisites", new[] { "PrerequisiteId" });
            DropIndex("dbo.CoursePrerequisites", new[] { "CourseId" });
            DropIndex("dbo.CourseSemesters", new[] { "SemesterId" });
            DropIndex("dbo.CourseSemesters", new[] { "CourseId" });
            DropIndex("dbo.StudentCourses", new[] { "CourseId" });
            DropIndex("dbo.StudentCourses", new[] { "StudentId" });
            DropIndex("dbo.Students", new[] { "CurrentSemesterId" });
            DropIndex("dbo.Exams", new[] { "CourseId" });
            DropIndex("dbo.Exams", new[] { "SemesterId" });
            DropIndex("dbo.Exams", new[] { "StudentId" });
            DropIndex("dbo.Courses", new[] { "DiscountId" });
            DropTable("dbo.CoursePrerequisites");
            DropTable("dbo.CourseSemesters");
            DropTable("dbo.StudentCourses");
            DropTable("dbo.Discounts");
            DropTable("dbo.Students");
            DropTable("dbo.Exams");
            DropTable("dbo.Semesters");
            DropTable("dbo.Courses");
        }
    }
}
