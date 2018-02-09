namespace OBL.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewMig : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Genres", "Description", c => c.String(maxLength: 500));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Genres", "Description");
        }
    }
}
