namespace DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatedb : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ActionLogs", "ExecutionTime", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ActionLogs", "ExecutionTime", c => c.Long(nullable: false));
        }
    }
}
