namespace DataMapper.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SwitchedToEnglish : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Inscrieres", "SemestruId", "dbo.Semestrus");
            DropForeignKey("dbo.Inscrieres", "StudentId", "dbo.Students");
            DropForeignKey("dbo.Inscrieres", "CursId", "dbo.Curs");
            DropForeignKey("dbo.Prerequisites", "PrerequisiteCurs_Id", "dbo.Curs");
            DropForeignKey("dbo.Prerequisites", "CursId", "dbo.Curs");
            DropForeignKey("dbo.CursuriSemestre", "CursId", "dbo.Curs");
            DropForeignKey("dbo.CursuriSemestre", "SemestruId", "dbo.Semestrus");
            DropIndex("dbo.Inscrieres", new[] { "StudentId" });
            DropIndex("dbo.Inscrieres", new[] { "CursId" });
            DropIndex("dbo.Inscrieres", new[] { "SemestruId" });
            DropIndex("dbo.Prerequisites", new[] { "CursId" });
            DropIndex("dbo.Prerequisites", new[] { "PrerequisiteCurs_Id" });
            DropIndex("dbo.CursuriSemestre", new[] { "CursId" });
            DropIndex("dbo.CursuriSemestre", new[] { "SemestruId" });
            CreateTable(
                "dbo.Courses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Description = c.String(nullable: false, maxLength: 500),
                        Credits = c.Int(nullable: false),
                        Cost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        MinThreshold = c.Decimal(nullable: false, precision: 18, scale: 2),
                        MaxThreshold = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id);
            
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
                "dbo.Enrollments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StudentId = c.Int(nullable: false),
                        CourseId = c.Int(nullable: false),
                        SemesterId = c.Int(nullable: false),
                        NumberOfAttempts = c.Int(nullable: false),
                        LastAttemptDate = c.DateTime(),
                        Grade = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Semesters", t => t.SemesterId, cascadeDelete: true)
                .ForeignKey("dbo.Students", t => t.StudentId, cascadeDelete: true)
                .ForeignKey("dbo.Courses", t => t.CourseId, cascadeDelete: true)
                .Index(t => t.StudentId)
                .Index(t => t.CourseId)
                .Index(t => t.SemesterId);
            
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
            
            AddColumn("dbo.Students", "FirstName", c => c.String(nullable: false, maxLength: 50));
            AddColumn("dbo.Students", "LastName", c => c.String(nullable: false, maxLength: 50));
            AddColumn("dbo.Students", "Address", c => c.String(nullable: false, maxLength: 200));
            AddColumn("dbo.Students", "NationalId", c => c.String(nullable: false));
            AddColumn("dbo.Students", "RegistrationNumber", c => c.String(nullable: false, maxLength: 20));
            AddColumn("dbo.Prerequisites", "CourseId", c => c.Int(nullable: false));
            AddColumn("dbo.Prerequisites", "PrerequisiteCourseId", c => c.Int(nullable: false));
            AddColumn("dbo.Prerequisites", "MinimumGrade", c => c.Int(nullable: false));
            CreateIndex("dbo.Prerequisites", "CourseId");
            CreateIndex("dbo.Prerequisites", "PrerequisiteCourseId");
            AddForeignKey("dbo.Prerequisites", "PrerequisiteCourseId", "dbo.Courses", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Prerequisites", "CourseId", "dbo.Courses", "Id");
            DropColumn("dbo.Students", "Nume");
            DropColumn("dbo.Students", "Prenume");
            DropColumn("dbo.Students", "Adresa");
            DropColumn("dbo.Students", "CNP");
            DropColumn("dbo.Students", "Matricola");
            DropColumn("dbo.Prerequisites", "CursId");
            DropColumn("dbo.Prerequisites", "PrerequisiteId");
            DropColumn("dbo.Prerequisites", "NotaMinima");
            DropColumn("dbo.Prerequisites", "PrerequisiteCurs_Id");
            DropTable("dbo.Curs");
            DropTable("dbo.Inscrieres");
            DropTable("dbo.Semestrus");
            DropTable("dbo.CursuriSemestre");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.CursuriSemestre",
                c => new
                    {
                        CursId = c.Int(nullable: false),
                        SemestruId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.CursId, t.SemestruId });
            
            CreateTable(
                "dbo.Semestrus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Numar = c.Int(nullable: false),
                        PragCredite = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Inscrieres",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StudentId = c.Int(nullable: false),
                        CursId = c.Int(nullable: false),
                        SemestruId = c.Int(nullable: false),
                        NumarPrezentari = c.Int(nullable: false),
                        DataUltimeiPrezentari = c.DateTime(),
                        Nota = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Curs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nume = c.String(nullable: false, maxLength: 100),
                        Descriere = c.String(nullable: false, maxLength: 500),
                        Credite = c.Int(nullable: false),
                        Cost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PragMin = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PragMax = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Prerequisites", "PrerequisiteCurs_Id", c => c.Int());
            AddColumn("dbo.Prerequisites", "NotaMinima", c => c.Int(nullable: false));
            AddColumn("dbo.Prerequisites", "PrerequisiteId", c => c.Int(nullable: false));
            AddColumn("dbo.Prerequisites", "CursId", c => c.Int(nullable: false));
            AddColumn("dbo.Students", "Matricola", c => c.String(nullable: false, maxLength: 20));
            AddColumn("dbo.Students", "CNP", c => c.String(nullable: false));
            AddColumn("dbo.Students", "Adresa", c => c.String(nullable: false, maxLength: 200));
            AddColumn("dbo.Students", "Prenume", c => c.String(nullable: false, maxLength: 50));
            AddColumn("dbo.Students", "Nume", c => c.String(nullable: false, maxLength: 50));
            DropForeignKey("dbo.Prerequisites", "CourseId", "dbo.Courses");
            DropForeignKey("dbo.Prerequisites", "PrerequisiteCourseId", "dbo.Courses");
            DropForeignKey("dbo.Enrollments", "CourseId", "dbo.Courses");
            DropForeignKey("dbo.Enrollments", "StudentId", "dbo.Students");
            DropForeignKey("dbo.Enrollments", "SemesterId", "dbo.Semesters");
            DropForeignKey("dbo.CourseSemesters", "SemesterId", "dbo.Semesters");
            DropForeignKey("dbo.CourseSemesters", "CourseId", "dbo.Courses");
            DropIndex("dbo.CourseSemesters", new[] { "SemesterId" });
            DropIndex("dbo.CourseSemesters", new[] { "CourseId" });
            DropIndex("dbo.Prerequisites", new[] { "PrerequisiteCourseId" });
            DropIndex("dbo.Prerequisites", new[] { "CourseId" });
            DropIndex("dbo.Enrollments", new[] { "SemesterId" });
            DropIndex("dbo.Enrollments", new[] { "CourseId" });
            DropIndex("dbo.Enrollments", new[] { "StudentId" });
            DropColumn("dbo.Prerequisites", "MinimumGrade");
            DropColumn("dbo.Prerequisites", "PrerequisiteCourseId");
            DropColumn("dbo.Prerequisites", "CourseId");
            DropColumn("dbo.Students", "RegistrationNumber");
            DropColumn("dbo.Students", "NationalId");
            DropColumn("dbo.Students", "Address");
            DropColumn("dbo.Students", "LastName");
            DropColumn("dbo.Students", "FirstName");
            DropTable("dbo.CourseSemesters");
            DropTable("dbo.Enrollments");
            DropTable("dbo.Semesters");
            DropTable("dbo.Courses");
            CreateIndex("dbo.CursuriSemestre", "SemestruId");
            CreateIndex("dbo.CursuriSemestre", "CursId");
            CreateIndex("dbo.Prerequisites", "PrerequisiteCurs_Id");
            CreateIndex("dbo.Prerequisites", "CursId");
            CreateIndex("dbo.Inscrieres", "SemestruId");
            CreateIndex("dbo.Inscrieres", "CursId");
            CreateIndex("dbo.Inscrieres", "StudentId");
            AddForeignKey("dbo.CursuriSemestre", "SemestruId", "dbo.Semestrus", "Id", cascadeDelete: true);
            AddForeignKey("dbo.CursuriSemestre", "CursId", "dbo.Curs", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Prerequisites", "CursId", "dbo.Curs", "Id");
            AddForeignKey("dbo.Prerequisites", "PrerequisiteCurs_Id", "dbo.Curs", "Id");
            AddForeignKey("dbo.Inscrieres", "CursId", "dbo.Curs", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Inscrieres", "StudentId", "dbo.Students", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Inscrieres", "SemestruId", "dbo.Semestrus", "Id", cascadeDelete: true);
        }
    }
}
