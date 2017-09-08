namespace Teko.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Refactor : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Archivos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Ruta = c.String(),
                        IdContenido_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Contenidos", t => t.IdContenido_Id)
                .Index(t => t.IdContenido_Id);
            
            CreateTable(
                "dbo.Contenidos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nombre = c.String(),
                        Descripcion = c.String(),
                        Profesor = c.String(),
                        Cursada = c.Int(nullable: false),
                        UsuariosId = c.String(maxLength: 128),
                        EscuelasId = c.Int(),
                        MateriasId = c.Int(),
                        TiposContenidosId = c.Int(),
                        IPop = c.Int(),
                        IDes = c.Int(),
                        FechaSubida = c.DateTime(),
                        ValoracionPromedio = c.Double(nullable: false),
                        NivelesEducativos_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Escuelas", t => t.EscuelasId)
                .ForeignKey("dbo.NivelesEducativos", t => t.NivelesEducativos_Id)
                .ForeignKey("dbo.Materias", t => t.MateriasId)
                .ForeignKey("dbo.TiposContenidos", t => t.TiposContenidosId)
                .ForeignKey("dbo.AspNetUsers", t => t.UsuariosId)
                .Index(t => t.UsuariosId)
                .Index(t => t.EscuelasId)
                .Index(t => t.MateriasId)
                .Index(t => t.TiposContenidosId)
                .Index(t => t.NivelesEducativos_Id);
            
            CreateTable(
                "dbo.Escuelas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nombre = c.String(),
                        NivEduEscuela_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.NivelesEducativos", t => t.NivEduEscuela_Id)
                .Index(t => t.NivEduEscuela_Id);
            
            CreateTable(
                "dbo.NivelesEducativos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nombre = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Materias",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nombre = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TiposContenidos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nombre = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Nombre = c.String(),
                        Apellido = c.String(),
                        RutaFoto = c.String(),
                        Descripcion = c.String(),
                        FechaNacimiento = c.DateTime(),
                        InstitucionActualId = c.Int(),
                        ContenidosFav = c.String(),
                        PerfilCompleto = c.Boolean(nullable: false),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Escuelas", t => t.InstitucionActualId)
                .Index(t => t.InstitucionActualId)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.Valoraciones",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Valoracion = c.Int(nullable: false),
                        Contenido_Id = c.Int(),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Contenidos", t => t.Contenido_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.Contenido_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.Visitas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Contador = c.Int(nullable: false),
                        LastUpdate = c.DateTime(nullable: false),
                        Contenido_Id = c.Int(),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Contenidos", t => t.Contenido_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.Contenido_Id)
                .Index(t => t.User_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Visitas", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Visitas", "Contenido_Id", "dbo.Contenidos");
            DropForeignKey("dbo.Valoraciones", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Valoraciones", "Contenido_Id", "dbo.Contenidos");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Archivos", "IdContenido_Id", "dbo.Contenidos");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "InstitucionActualId", "dbo.Escuelas");
            DropForeignKey("dbo.Contenidos", "UsuariosId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Contenidos", "TiposContenidosId", "dbo.TiposContenidos");
            DropForeignKey("dbo.Contenidos", "MateriasId", "dbo.Materias");
            DropForeignKey("dbo.Escuelas", "NivEduEscuela_Id", "dbo.NivelesEducativos");
            DropForeignKey("dbo.Contenidos", "NivelesEducativos_Id", "dbo.NivelesEducativos");
            DropForeignKey("dbo.Contenidos", "EscuelasId", "dbo.Escuelas");
            DropIndex("dbo.Visitas", new[] { "User_Id" });
            DropIndex("dbo.Visitas", new[] { "Contenido_Id" });
            DropIndex("dbo.Valoraciones", new[] { "User_Id" });
            DropIndex("dbo.Valoraciones", new[] { "Contenido_Id" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUsers", new[] { "InstitucionActualId" });
            DropIndex("dbo.Escuelas", new[] { "NivEduEscuela_Id" });
            DropIndex("dbo.Contenidos", new[] { "NivelesEducativos_Id" });
            DropIndex("dbo.Contenidos", new[] { "TiposContenidosId" });
            DropIndex("dbo.Contenidos", new[] { "MateriasId" });
            DropIndex("dbo.Contenidos", new[] { "EscuelasId" });
            DropIndex("dbo.Contenidos", new[] { "UsuariosId" });
            DropIndex("dbo.Archivos", new[] { "IdContenido_Id" });
            DropTable("dbo.Visitas");
            DropTable("dbo.Valoraciones");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.TiposContenidos");
            DropTable("dbo.Materias");
            DropTable("dbo.NivelesEducativos");
            DropTable("dbo.Escuelas");
            DropTable("dbo.Contenidos");
            DropTable("dbo.Archivos");
        }
    }
}
