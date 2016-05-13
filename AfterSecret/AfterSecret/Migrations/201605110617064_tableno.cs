namespace AfterSecret.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tableno : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Ticket", "TableNo", c => c.String(nullable: false, maxLength: 4));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Ticket", "TableNo");
        }
    }
}
