namespace DecouplingAspNetIdentity.Repositories.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class intial_models : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ExternalLogins",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LoginProvider = c.String(maxLength: 128),
                        ProviderKey = c.String(maxLength: 128),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserName = c.String(nullable: false, maxLength: 25),
                        PasswordHash = c.String(maxLength: 256),
                        SecurityStamp = c.String(maxLength: 256),
                        PasswordResetToken = c.String(maxLength: 256),
                        Email = c.String(nullable: false, maxLength: 256),
                        AccessFailedCount = c.Int(nullable: false),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PhoneNumber = c.String(nullable: false, maxLength: 20),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        ProviderId = c.Guid(nullable: false),
                        LastLoginDate = c.DateTime(),
                        FirstName = c.String(nullable: false, maxLength: 20),
                        SurName = c.String(nullable: false, maxLength: 20),
                        External = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true)
                .Index(t => t.Email, unique: true);
            
            CreateTable(
                "dbo.Claims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true);
            
            CreateTable(
                "dbo.UserRole",
                c => new
                    {
                        RoleId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.RoleId, t.UserId })
                .ForeignKey("dbo.Roles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.RoleId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ExternalLogins", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserRole", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserRole", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.Claims", "UserId", "dbo.Users");
            DropIndex("dbo.UserRole", new[] { "UserId" });
            DropIndex("dbo.UserRole", new[] { "RoleId" });
            DropIndex("dbo.Roles", new[] { "Name" });
            DropIndex("dbo.Claims", new[] { "UserId" });
            DropIndex("dbo.Users", new[] { "Email" });
            DropIndex("dbo.Users", new[] { "UserName" });
            DropIndex("dbo.ExternalLogins", new[] { "UserId" });
            DropTable("dbo.UserRole");
            DropTable("dbo.Roles");
            DropTable("dbo.Claims");
            DropTable("dbo.Users");
            DropTable("dbo.ExternalLogins");
        }
    }
}
