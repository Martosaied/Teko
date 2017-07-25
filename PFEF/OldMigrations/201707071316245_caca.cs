namespace PFEF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class caca : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Contenidos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nombre = c.String(),
                        Descripcion = c.String(),
                        Ruta = c.String(),
                        Profesor = c.String(),
                        Cursada = c.Int(nullable: false),
                        UsuariosId = c.Int(nullable: false),
                        EscuelasId = c.Int(),
                        MateriasId = c.Int(),
                        NivelesEducativosId = c.Int(),
                        TiposContenidosId = c.Int(),
                        IPop = c.Int(),
                        IDes = c.Int(),
                        FechaSubida = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Escuelas", t => t.EscuelasId)
                .ForeignKey("dbo.Materias", t => t.MateriasId)
                .ForeignKey("dbo.NivelesEducativos", t => t.NivelesEducativosId)
                .ForeignKey("dbo.TiposContenidos", t => t.TiposContenidosId)
                .ForeignKey("dbo.Usuarios", t => t.UsuariosId, cascadeDelete: true)
                .Index(t => t.UsuariosId)
                .Index(t => t.EscuelasId)
                .Index(t => t.MateriasId)
                .Index(t => t.NivelesEducativosId)
                .Index(t => t.TiposContenidosId);
            
            CreateTable(
                "dbo.Escuelas",
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
                "dbo.NivelesEducativos",
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
                "dbo.Usuarios",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nombre = c.String(),
                        Apellido = c.String(),
                        RutaFoto = c.String(),
                        InstitucionActual = c.String(),
                        Descripcion = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Favoritos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdContenido_Id = c.Int(),
                        IdUsuario_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Contenidos", t => t.IdContenido_Id)
                .ForeignKey("dbo.Usuarios", t => t.IdUsuario_Id)
                .Index(t => t.IdContenido_Id)
                .Index(t => t.IdUsuario_Id);
            
            CreateTable(
                "dbo.Intereses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdMaterias_Id = c.Int(),
                        IdUsuario_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Materias", t => t.IdMaterias_Id)
                .ForeignKey("dbo.Usuarios", t => t.IdUsuario_Id)
                .Index(t => t.IdMaterias_Id)
                .Index(t => t.IdUsuario_Id);
            
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
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        IdUserInfo = c.Int(nullable: false),
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Intereses", "IdUsuario_Id", "dbo.Usuarios");
            DropForeignKey("dbo.Intereses", "IdMaterias_Id", "dbo.Materias");
            DropForeignKey("dbo.Favoritos", "IdUsuario_Id", "dbo.Usuarios");
            DropForeignKey("dbo.Favoritos", "IdContenido_Id", "dbo.Contenidos");
            DropForeignKey("dbo.Contenidos", "UsuariosId", "dbo.Usuarios");
            DropForeignKey("dbo.Contenidos", "TiposContenidosId", "dbo.TiposContenidos");
            DropForeignKey("dbo.Contenidos", "NivelesEducativosId", "dbo.NivelesEducativos");
            DropForeignKey("dbo.Contenidos", "MateriasId", "dbo.Materias");
            DropForeignKey("dbo.Contenidos", "EscuelasId", "dbo.Escuelas");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Intereses", new[] { "IdUsuario_Id" });
            DropIndex("dbo.Intereses", new[] { "IdMaterias_Id" });
            DropIndex("dbo.Favoritos", new[] { "IdUsuario_Id" });
            DropIndex("dbo.Favoritos", new[] { "IdContenido_Id" });
            DropIndex("dbo.Contenidos", new[] { "TiposContenidosId" });
            DropIndex("dbo.Contenidos", new[] { "NivelesEducativosId" });
            DropIndex("dbo.Contenidos", new[] { "MateriasId" });
            DropIndex("dbo.Contenidos", new[] { "EscuelasId" });
            DropIndex("dbo.Contenidos", new[] { "UsuariosId" });
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Intereses");
            DropTable("dbo.Favoritos");
            DropTable("dbo.Usuarios");
            DropTable("dbo.TiposContenidos");
            DropTable("dbo.NivelesEducativos");
            DropTable("dbo.Materias");
            DropTable("dbo.Escuelas");
            DropTable("dbo.Contenidos");
        }
    }
}
