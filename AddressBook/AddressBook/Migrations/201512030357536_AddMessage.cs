namespace AddressBook.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMessage : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.Int(nullable: false),
                        Header = c.String(nullable: false),
                        Body = c.String(nullable: false),
                        isHTML = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.AspNetUsers", "TempPassword", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "TempPassword");
            DropTable("dbo.Messages");
        }
    }
}
