namespace MMTShopApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        CatID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 30),
                        SkuValue = c.String(nullable: false, maxLength: 5),
                    })
                .PrimaryKey(t => t.CatID);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CatID = c.Int(nullable: false),
                        SKU = c.String(nullable: false, maxLength: 5),
                        Name = c.String(nullable: false, maxLength: 30),
                        Description = c.String(),
                        Price = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Categories", t => t.CatID, cascadeDelete: true)
                .Index(t => t.CatID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Products", "CatID", "dbo.Categories");
            DropIndex("dbo.Products", new[] { "CatID" });
            DropTable("dbo.Products");
            DropTable("dbo.Categories");
        }
    }
}
