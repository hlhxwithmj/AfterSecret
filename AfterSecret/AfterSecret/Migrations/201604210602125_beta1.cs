namespace AfterSecret.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class beta1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Ticket", "PurchaseId", "dbo.Purchase");
            DropIndex("dbo.Ticket", new[] { "PurchaseId" });
            RenameColumn(table: "dbo.Ticket", name: "RegisterMemberId", newName: "InviteeId");
            RenameIndex(table: "dbo.Ticket", name: "IX_RegisterMemberId", newName: "IX_InviteeId");
            CreateTable(
                "dbo.Invitation",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        InviterId = c.Int(nullable: false),
                        TicketCode = c.String(maxLength: 50),
                        TableCode = c.String(maxLength: 50),
                        IsValidate = c.Boolean(nullable: false),
                        EditTime = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RegisterMember", t => t.InviterId)
                .Index(t => t.InviterId);
            
            AddColumn("dbo.Item", "InvitationType", c => c.Int(nullable: false));
            AddColumn("dbo.Purchase", "Quantity", c => c.Int(nullable: false));
            AddColumn("dbo.Ticket", "InvitationId", c => c.Int(nullable: false));
            AddColumn("dbo.Ticket", "InvitationType", c => c.Int(nullable: false));
            CreateIndex("dbo.Ticket", "InvitationId");
            AddForeignKey("dbo.Ticket", "InvitationId", "dbo.Invitation", "Id");
            DropColumn("dbo.Item", "NeedInvite");
            DropColumn("dbo.Purchase", "TicketCode");
            DropColumn("dbo.Ticket", "PurchaseId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Ticket", "PurchaseId", c => c.Int(nullable: false));
            AddColumn("dbo.Purchase", "TicketCode", c => c.String(maxLength: 50));
            AddColumn("dbo.Item", "NeedInvite", c => c.Boolean(nullable: false));
            DropForeignKey("dbo.Ticket", "InvitationId", "dbo.Invitation");
            DropForeignKey("dbo.Invitation", "InviterId", "dbo.RegisterMember");
            DropIndex("dbo.Ticket", new[] { "InvitationId" });
            DropIndex("dbo.Invitation", new[] { "InviterId" });
            DropColumn("dbo.Ticket", "InvitationType");
            DropColumn("dbo.Ticket", "InvitationId");
            DropColumn("dbo.Purchase", "Quantity");
            DropColumn("dbo.Item", "InvitationType");
            DropTable("dbo.Invitation");
            RenameIndex(table: "dbo.Ticket", name: "IX_InviteeId", newName: "IX_RegisterMemberId");
            RenameColumn(table: "dbo.Ticket", name: "InviteeId", newName: "RegisterMemberId");
            CreateIndex("dbo.Ticket", "PurchaseId");
            AddForeignKey("dbo.Ticket", "PurchaseId", "dbo.Purchase", "Id");
        }
    }
}
