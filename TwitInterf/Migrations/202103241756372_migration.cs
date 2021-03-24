namespace TwitInterf.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TwitterTextDbs", "UsersDb_Id", c => c.Int());
            CreateIndex("dbo.TwitterTextDbs", "UsersDb_Id");
            AddForeignKey("dbo.TwitterTextDbs", "UsersDb_Id", "dbo.UsersDbs", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TwitterTextDbs", "UsersDb_Id", "dbo.UsersDbs");
            DropIndex("dbo.TwitterTextDbs", new[] { "UsersDb_Id" });
            DropColumn("dbo.TwitterTextDbs", "UsersDb_Id");
        }
    }
}
