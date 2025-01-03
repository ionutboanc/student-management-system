namespace DataMapper.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Refactored : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Products", "Category_Id", "dbo.Categories");
            DropForeignKey("dbo.Orders", "Customer_Id", "dbo.Customers");
            DropForeignKey("dbo.OrderLines", "Order_Id", "dbo.Orders");
            DropForeignKey("dbo.OrderLines", "Product_Id", "dbo.Products");
            DropIndex("dbo.Products", new[] { "Category_Id" });
            DropIndex("dbo.OrderLines", new[] { "Order_Id" });
            DropIndex("dbo.OrderLines", new[] { "Product_Id" });
            DropIndex("dbo.Orders", new[] { "Customer_Id" });
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Semestrus", t => t.SemestruId, cascadeDelete: true)
                .ForeignKey("dbo.Students", t => t.StudentId, cascadeDelete: true)
                .ForeignKey("dbo.Curs", t => t.CursId, cascadeDelete: true)
                .Index(t => t.StudentId)
                .Index(t => t.CursId)
                .Index(t => t.SemestruId);
            
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
                "dbo.Students",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nume = c.String(nullable: false, maxLength: 50),
                        Prenume = c.String(nullable: false, maxLength: 50),
                        Adresa = c.String(nullable: false, maxLength: 200),
                        CNP = c.String(nullable: false),
                        Matricola = c.String(nullable: false, maxLength: 20),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Prerequisites",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CursId = c.Int(nullable: false),
                        PrerequisiteId = c.Int(nullable: false),
                        NotaMinima = c.Int(nullable: false),
                        PrerequisiteCurs_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Curs", t => t.PrerequisiteCurs_Id)
                .ForeignKey("dbo.Curs", t => t.CursId)
                .Index(t => t.CursId)
                .Index(t => t.PrerequisiteCurs_Id);
            
            CreateTable(
                "dbo.CursuriSemestre",
                c => new
                    {
                        CursId = c.Int(nullable: false),
                        SemestruId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.CursId, t.SemestruId })
                .ForeignKey("dbo.Curs", t => t.CursId, cascadeDelete: true)
                .ForeignKey("dbo.Semestrus", t => t.SemestruId, cascadeDelete: true)
                .Index(t => t.CursId)
                .Index(t => t.SemestruId);
            
            DropTable("dbo.Categories");
            DropTable("dbo.Products");
            DropTable("dbo.Customers");
            DropTable("dbo.OrderLines");
            DropTable("dbo.Orders");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Customer_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.OrderLines",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Quantity = c.Double(nullable: false),
                        Order_Id = c.Int(nullable: false),
                        Product_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Address = c.String(maxLength: 200),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        MeasurementUnit = c.String(nullable: false, maxLength: 20),
                        UnitPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Category_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Description = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("dbo.CursuriSemestre", "SemestruId", "dbo.Semestrus");
            DropForeignKey("dbo.CursuriSemestre", "CursId", "dbo.Curs");
            DropForeignKey("dbo.Prerequisites", "CursId", "dbo.Curs");
            DropForeignKey("dbo.Prerequisites", "PrerequisiteCurs_Id", "dbo.Curs");
            DropForeignKey("dbo.Inscrieres", "CursId", "dbo.Curs");
            DropForeignKey("dbo.Inscrieres", "StudentId", "dbo.Students");
            DropForeignKey("dbo.Inscrieres", "SemestruId", "dbo.Semestrus");
            DropIndex("dbo.CursuriSemestre", new[] { "SemestruId" });
            DropIndex("dbo.CursuriSemestre", new[] { "CursId" });
            DropIndex("dbo.Prerequisites", new[] { "PrerequisiteCurs_Id" });
            DropIndex("dbo.Prerequisites", new[] { "CursId" });
            DropIndex("dbo.Inscrieres", new[] { "SemestruId" });
            DropIndex("dbo.Inscrieres", new[] { "CursId" });
            DropIndex("dbo.Inscrieres", new[] { "StudentId" });
            DropTable("dbo.CursuriSemestre");
            DropTable("dbo.Prerequisites");
            DropTable("dbo.Students");
            DropTable("dbo.Semestrus");
            DropTable("dbo.Inscrieres");
            DropTable("dbo.Curs");
            CreateIndex("dbo.Orders", "Customer_Id");
            CreateIndex("dbo.OrderLines", "Product_Id");
            CreateIndex("dbo.OrderLines", "Order_Id");
            CreateIndex("dbo.Products", "Category_Id");
            AddForeignKey("dbo.OrderLines", "Product_Id", "dbo.Products", "Id", cascadeDelete: true);
            AddForeignKey("dbo.OrderLines", "Order_Id", "dbo.Orders", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Orders", "Customer_Id", "dbo.Customers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Products", "Category_Id", "dbo.Categories", "Id", cascadeDelete: true);
        }
    }
}
