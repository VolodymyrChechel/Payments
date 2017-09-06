namespace Payments.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class transactionnewfields : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Accounts",
                c => new
                    {
                        AccountNumber = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Sum = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsBlocked = c.Boolean(nullable: false),
                        Currency = c.Int(nullable: false),
                        ClientProfileId = c.String(maxLength: 128),
                        CreditSum = c.Decimal(precision: 18, scale: 2),
                        CreditRate = c.Double(),
                        CreditDate = c.DateTime(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.AccountNumber)
                .ForeignKey("dbo.ClientProfiles", t => t.ClientProfileId)
                .Index(t => t.ClientProfileId);
            
            CreateTable(
                "dbo.Cards",
                c => new
                    {
                        CardNumber = c.String(nullable: false, maxLength: 128),
                        ExpiryDate = c.DateTime(nullable: false),
                        CVV = c.String(),
                        Holder = c.String(),
                        CreditCardTypes = c.Int(nullable: false),
                        AccountAccountNumber = c.Int(),
                    })
                .PrimaryKey(t => t.CardNumber)
                .ForeignKey("dbo.Accounts", t => t.AccountAccountNumber)
                .Index(t => t.AccountAccountNumber);
            
            CreateTable(
                "dbo.ClientProfiles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        IsBlocked = c.Boolean(nullable: false),
                        FirstName = c.String(nullable: false),
                        SecondName = c.String(nullable: false),
                        Patronymic = c.String(nullable: false),
                        Country = c.String(),
                        City = c.String(),
                        Address = c.String(),
                        Birthday = c.DateTime(nullable: false),
                        PhoneNumber = c.String(nullable: false),
                        VAT = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
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
                "dbo.Transactions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TransactionSum = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TransactionType = c.Int(nullable: false),
                        TransactionStatus = c.Int(nullable: false),
                        Recipient = c.String(),
                        Comment = c.String(),
                        Account_AccountNumber = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Accounts", t => t.Account_AccountNumber)
                .Index(t => t.Account_AccountNumber);
            
            CreateTable(
                "dbo.UnblockAccountRequests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Status = c.Int(nullable: false),
                        RequestTime = c.DateTime(nullable: false),
                        Account_AccountNumber = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Accounts", t => t.Account_AccountNumber)
                .Index(t => t.Account_AccountNumber);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.UnblockAccountRequests", "Account_AccountNumber", "dbo.Accounts");
            DropForeignKey("dbo.Transactions", "Account_AccountNumber", "dbo.Accounts");
            DropForeignKey("dbo.ClientProfiles", "Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Accounts", "ClientProfileId", "dbo.ClientProfiles");
            DropForeignKey("dbo.Cards", "AccountAccountNumber", "dbo.Accounts");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.UnblockAccountRequests", new[] { "Account_AccountNumber" });
            DropIndex("dbo.Transactions", new[] { "Account_AccountNumber" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.ClientProfiles", new[] { "Id" });
            DropIndex("dbo.Cards", new[] { "AccountAccountNumber" });
            DropIndex("dbo.Accounts", new[] { "ClientProfileId" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.UnblockAccountRequests");
            DropTable("dbo.Transactions");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.ClientProfiles");
            DropTable("dbo.Cards");
            DropTable("dbo.Accounts");
        }
    }
}
