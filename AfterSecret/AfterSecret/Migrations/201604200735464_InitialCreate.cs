namespace AfterSecret.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AccessToken",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Token = c.String(maxLength: 1024),
                        ExpireTime = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ErrCode = c.String(maxLength: 20),
                        ErrMsg = c.String(maxLength: 200),
                        EditTime = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        IsValidate = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AgentCodeList",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AgentCode = c.String(nullable: false, maxLength: 50),
                        OpenId = c.String(maxLength: 200),
                        IsValidate = c.Boolean(nullable: false),
                        EditTime = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Item",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Remark = c.String(maxLength: 200),
                        Seats = c.Int(nullable: false),
                        UnitPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Total = c.Int(nullable: false),
                        Order = c.Int(nullable: false),
                        NeedInvite = c.Boolean(nullable: false),
                        ImgSrc = c.String(nullable: false, maxLength: 200),
                        IsValidate = c.Boolean(nullable: false),
                        EditTime = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.JsApiTicket",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Ticket = c.String(maxLength: 1024),
                        ExpireTime = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ErrCode = c.String(maxLength: 20),
                        ErrMsg = c.String(maxLength: 200),
                        IsValidate = c.Boolean(nullable: false),
                        EditTime = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Order",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RegisterMemberId = c.Int(nullable: false),
                        OpenId = c.String(nullable: false, maxLength: 200),
                        OpenIdForPay = c.String(nullable: false, maxLength: 200),
                        Order_No = c.String(nullable: false, maxLength: 50),
                        ChargeId = c.String(nullable: false, maxLength: 100),
                        AppId = c.String(nullable: false, maxLength: 100),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Client_Ip = c.String(nullable: false, maxLength: 50),
                        Currency = c.String(nullable: false, maxLength: 20),
                        Subject = c.String(nullable: false, maxLength: 32),
                        Body = c.String(nullable: false, maxLength: 128),
                        Channel = c.String(nullable: false, maxLength: 20),
                        ExpireTime = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        PaidTime = c.DateTime(precision: 7, storeType: "datetime2"),
                        CreatedTime = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        OrderStatus = c.Int(nullable: false),
                        FailureCode = c.String(),
                        FailureMsg = c.String(),
                        IsValidate = c.Boolean(nullable: false),
                        EditTime = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RegisterMember", t => t.RegisterMemberId)
                .Index(t => t.RegisterMemberId);
            
            CreateTable(
                "dbo.Purchase",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OrderId = c.Int(nullable: false),
                        ItemId = c.Int(nullable: false),
                        TicketCode = c.String(maxLength: 50),
                        IsValidate = c.Boolean(nullable: false),
                        EditTime = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Item", t => t.ItemId)
                .ForeignKey("dbo.Order", t => t.OrderId)
                .Index(t => t.OrderId)
                .Index(t => t.ItemId);
            
            CreateTable(
                "dbo.Ticket",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PurchaseId = c.Int(nullable: false),
                        RegisterMemberId = c.Int(nullable: false),
                        QRCodePath = c.String(nullable: false, maxLength: 128),
                        IsValidate = c.Boolean(nullable: false),
                        EditTime = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Purchase", t => t.PurchaseId)
                .ForeignKey("dbo.RegisterMember", t => t.RegisterMemberId)
                .Index(t => t.PurchaseId)
                .Index(t => t.RegisterMemberId);
            
            CreateTable(
                "dbo.RegisterMember",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AgentCode = c.String(nullable: false, maxLength: 50),
                        OpenId = c.String(nullable: false, maxLength: 200),
                        FirstName = c.String(nullable: false, maxLength: 100),
                        LastName = c.String(nullable: false, maxLength: 100),
                        Gender = c.String(nullable: false, maxLength: 20),
                        Nationality = c.String(nullable: false, maxLength: 100),
                        Mobile = c.String(nullable: false, maxLength: 25),
                        Email = c.String(nullable: false, maxLength: 100),
                        WeChatID = c.String(maxLength: 100),
                        Occupation = c.String(maxLength: 100),
                        IsValidate = c.Boolean(nullable: false),
                        EditTime = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Order", "RegisterMemberId", "dbo.RegisterMember");
            DropForeignKey("dbo.Ticket", "RegisterMemberId", "dbo.RegisterMember");
            DropForeignKey("dbo.Ticket", "PurchaseId", "dbo.Purchase");
            DropForeignKey("dbo.Purchase", "OrderId", "dbo.Order");
            DropForeignKey("dbo.Purchase", "ItemId", "dbo.Item");
            DropIndex("dbo.Ticket", new[] { "RegisterMemberId" });
            DropIndex("dbo.Ticket", new[] { "PurchaseId" });
            DropIndex("dbo.Purchase", new[] { "ItemId" });
            DropIndex("dbo.Purchase", new[] { "OrderId" });
            DropIndex("dbo.Order", new[] { "RegisterMemberId" });
            DropTable("dbo.RegisterMember");
            DropTable("dbo.Ticket");
            DropTable("dbo.Purchase");
            DropTable("dbo.Order");
            DropTable("dbo.JsApiTicket");
            DropTable("dbo.Item");
            DropTable("dbo.AgentCodeList");
            DropTable("dbo.AccessToken");
        }
    }
}
