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

        public DbSet<Invitation> Invitation { get; set; }

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
            db.Database.ExecuteSqlCommand("CREATE UNIQUE INDEX unique_index ON dbo.Ticket(InviteeId)");
            db.Database.ExecuteSqlCommand("CREATE UNIQUE INDEX unique_index1 ON dbo.Invitation(TicketCode)");
            db.Database.ExecuteSqlCommand("CREATE UNIQUE INDEX unique_index2 ON dbo.Invitation(TableCode)");
            db.Item.Add(new Item()
            {
                Seats = 1,
                Name = "Entry Ticket",
                Remark = "(Incl. 1 drink)",
                Total = 999,
                UnitPrice = 20,
                Order = 10,
                InvitationType = Constant.InvitationType.Ticket,
                ImgSrc = SubscribeConfig.DOMAIN + "/static/image/entry.png"
            });

            db.Item.Add(new Item()
            {
                Seats = 4,
                Name = "Guest Table (4 pax)",
                Remark = "(Incl. 2 bottles of Champagne + 1 spirit)",
                Total = 999,
                UnitPrice = 30,
                Order = 20,
                InvitationType = Constant.InvitationType.Table,
                ImgSrc = SubscribeConfig.DOMAIN + "/static/image/guest.png"
            });

            db.Item.Add(new Item()
            {
                Seats = 8,
                Name = "VIP Table (8 pax)",
                Remark = "(Incl. 4 bottles of Champagne + 1 spirit)",
                Total = 999,
                UnitPrice = 50,
                Order = 30,
                InvitationType = Constant.InvitationType.Table,
                ImgSrc = SubscribeConfig.DOMAIN + "/static/image/vip.png"
            });

            db.Item.Add(new Item()
            {
                Seats = 8,
                Name = "VVIP Table (8 pax)",
                Remark = "(Incl. 6 bottles of Champagne + 1 spirit)",
                Total = 999,
                UnitPrice = 80,
                Order = 40,
                InvitationType = Constant.InvitationType.Table,
                ImgSrc = SubscribeConfig.DOMAIN + "/static/image/vvip.png"
            });
            db.Item.Add(new Item()
            {
                Seats = 12,
                Name = "VVVIP Table (12 pax)",
                Remark = "(Incl. 10 bottles of Champagne + 2 spirits)",
                Total = 999,
                UnitPrice = 80,
                Order = 50,
                InvitationType = Constant.InvitationType.Table,
                ImgSrc = SubscribeConfig.DOMAIN + "/static/image/vvvip.png"
            });
            for (int i = 0; i < 100; i++)
            {
                Random generator = new Random(i);
                var n = generator.Next(1, 99999).ToString("D5");
                db.AgentCodeList.Add(new AgentCodeList() { AgentCode = SubscribeConfig._seedUser_Prefix + n });
            }

            db.SaveChanges();
            //db.Database.ExecuteSqlCommand("ALTER DATABASE secret SET ALLOW_SNAPSHOT_ISOLATION ON");
        }
    }
}