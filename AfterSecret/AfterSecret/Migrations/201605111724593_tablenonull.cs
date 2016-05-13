namespace AfterSecret.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tablenonull : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Ticket", "TableNo", c => c.String(maxLength: 4));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Ticket", "TableNo", c => c.String(nullable: false, maxLength: 4));
        }
    }
}
