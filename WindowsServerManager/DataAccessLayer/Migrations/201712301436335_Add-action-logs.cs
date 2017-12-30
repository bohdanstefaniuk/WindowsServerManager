namespace DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addactionlogs : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ActionLogs",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Action = c.String(),
                        Controller = c.String(),
                        ActionParams = c.String(),
                        OriginalUrl = c.String(),
                        UserHost = c.String(),
                        UserAgent = c.String(),
                        HttpMethod = c.String(),
                        Form = c.String(),
                        Query = c.String(),
                        Referer = c.String(),
                        Headers = c.String(),
                        Cookies = c.String(),
                        UserName = c.String(),
                        StartExecution = c.DateTime(nullable: false),
                        FinishExecution = c.DateTime(nullable: false),
                        ExecutionTime = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ActionLogs");
        }
    }
}
