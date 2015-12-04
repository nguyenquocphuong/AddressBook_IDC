namespace AddressBook.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AppendMessageModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Messages", "Sender", c => c.String());
            AddColumn("dbo.Messages", "Recipients", c => c.String());
            AddColumn("dbo.Messages", "Status", c => c.Int(nullable: false));
            AddColumn("dbo.Messages", "NumberOfTries", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Messages", "NumberOfTries");
            DropColumn("dbo.Messages", "Status");
            DropColumn("dbo.Messages", "Recipients");
            DropColumn("dbo.Messages", "Sender");
        }
    }
}
