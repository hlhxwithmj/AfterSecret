using AfterSecret.Lib;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace AfterSecret.Models.DAL
{
    public class ASDbContext : DbContext
    {
        public ASDbContext()
            : base("AfterSecret")
        {

        }
        public DbSet<AgentCodeList> AgentCodeList { get; set; }

        public DbSet<RegisterMember> RegisterMember { get; set; }

        public DbSet<AccessToken> AccessToken { get; set; }

        public DbSet<Order> Order { get; set; }

        public DbSet<Item> Item { get; set; }
        public DbSet<Purchase> Purchase { get; set; }

        public DbSet<Ticket> Ticket { get; set; }
        public DbSet<JsApiTicket> JsApiTicket { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            modelBuilder.Entity<Order>().Property(x => x.Amount).HasPrecision(18, 2);
        }
    }

    public class DbInitializer : CreateDatabaseIfNotExists<ASDbContext>
    {
        protected override void Seed(ASDbContext db)
        {
            db.Database.ExecuteSqlCommand("CREATE UNIQUE INDEX unique_index ON AgentCodeList(AgentCode)");
            db.Database.ExecuteSqlCommand("CREATE UNIQUE INDEX unique_index ON [Order](Order_No)");
            db.Database.ExecuteSqlCommand("CREATE UNIQUE INDEX unique_index ON [RegisterMember](OpenId)");
            db.Database.ExecuteSqlCommand("CREATE UNIQUE INDEX unique_index ON [Purchase](TicketCode)");
            db.Database.ExecuteSqlCommand("CREATE UNIQUE INDEX unique_index ON dbo.Ticket(PurchaseId,RegisterMemberId)");
            //db.Item.Add(new Item()
            //{
            //    Seats = 1,
            //    Name = "Event Ticket",
            //    Remark = "",
            //    Total = 500,
            //    UnitPrice = 20000,
            //    Order = 10,
            //    NeedInvite = true,
            //    ImgSrc = SubscribeConfig.DOMAIN + "/static/image/bg.jpg"
            //});

            //db.Item.Add(new Item()
            //{
            //    Seats = 4,
            //    Name = "Table for 4 + 1 bottle of Wine",
            //    Remark = "(including 10 event tickets)",
            //    Total = 300,
            //    UnitPrice = 100000,
            //    Order = 20,
            //    NeedInvite = true,
            //    ImgSrc = SubscribeConfig.DOMAIN + "/static/image/bg.jpg"
            //});

            //db.Item.Add(new Item()
            //{
            //    Seats = 8,
            //    Name = "Table for 8 + 1 bottle of Wine",
            //    Remark = "(including 8 event tickets)",
            //    Total = 500,
            //    UnitPrice = 200000,
            //    Order = 30,
            //    ImgSrc = SubscribeConfig.DOMAIN + "/static/image/bg.jpg"
            //});

            //db.Item.Add(new Item()
            //{
            //    Seats = 10,
            //    Name = "Table for 10 + 1 bottle of Wine",
            //    Remark = "(including 18 event tickets)",
            //    Total = 700,
            //    UnitPrice = 300000,
            //    Order = 40,
            //    ImgSrc = SubscribeConfig.DOMAIN + "/static/image/bg-star.jpg"
            //});  
       
            db.Item.Add(new Item()
            {
                Seats = 1,
                Name = "Event Ticket",
                Remark = "",
                Total = 500,
                UnitPrice = 20,
                Order = 10,
                NeedInvite = true,
                ImgSrc = SubscribeConfig.DOMAIN + "/static/image/bg.jpg"
            });

            db.Item.Add(new Item()
            {
                Seats = 4,
                Name = "Table for 4 + 1 bottle of Wine",
                Remark = "(including 10 event tickets)",
                Total = 300,
                UnitPrice = 30,
                Order = 20,
                NeedInvite = true,
                ImgSrc = SubscribeConfig.DOMAIN + "/static/image/bg.jpg"
            });

            db.Item.Add(new Item()
            {
                Seats = 8,
                Name = "Table for 8 + 1 bottle of Wine",
                Remark = "(including 8 event tickets)",
                Total = 500,
                UnitPrice = 50,
                Order = 30,
                NeedInvite = true,
                ImgSrc = SubscribeConfig.DOMAIN + "/static/image/bg.jpg"
            });

            db.Item.Add(new Item()
            {
                Seats = 10,
                Name = "Table for 10 + 1 bottle of Wine",
                Remark = "(including 18 event tickets)",
                Total = 700,
                UnitPrice = 80,
                Order = 40,
                NeedInvite = true,
                ImgSrc = SubscribeConfig.DOMAIN + "/static/image/bg-star.jpg"
            });
            for (int i = 0; i < 20; i++)
            {
                Random generator = new Random(i);
                var n = generator.Next(1, int.MaxValue).ToString("D10");
                db.AgentCodeList.Add(new AgentCodeList() { AgentCode = SubscribeConfig._seedUser_Prefix + n });
            }

            db.SaveChanges();
            //db.Database.ExecuteSqlCommand("ALTER DATABASE secret SET ALLOW_SNAPSHOT_ISOLATION ON");
        }
    }
}