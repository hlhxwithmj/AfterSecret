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

        public DbSet<Charge> Charge { get; set; }

        public DbSet<Order> Order { get; set; }

        public DbSet<Invitation> Invitation { get; set; }

        public DbSet<Item> Item { get; set; }
        public DbSet<Purchase> Purchase { get; set; }

        public DbSet<Ticket> Ticket { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            modelBuilder.Entity<Charge>().Property(x => x.Amount).HasPrecision(18, 2);
        }
    }

    public class DbInitializer : CreateDatabaseIfNotExists<ASDbContext>
    {
        protected override void Seed(ASDbContext db)
        {
            db.Item.Add(new Item()
            {
                Factor = 5,
                Name = "5人卡座",
                Remain = 100,
                Remark = "5个座位",
                Total = 100,
                UnitPrice = 100,
                Order = 10
            });

            db.Item.Add(new Item()
            {
                Factor = 3,
                Name = "3人卡座",
                Remain = 100,
                Remark = "3个座位",
                Total = 200,
                UnitPrice = 200,
                Order = 20
            });

            db.Item.Add(new Item()
            {
                Factor = 1,
                Name = "葡萄酒",
                Remain = 500,
                Remark = "原产地法国",
                Total = 500,
                UnitPrice = 80,
                Order = 30
            });

            db.Item.Add(new Item()
            {
                Factor = 1,
                Name = "鸡尾酒",
                Remain = 700,
                Remark = "现场制作",
                Total = 700,
                UnitPrice = 120,
                Order = 40
            });
            for (int i = 0; i < 20; i++)
            {
                Random generator = new Random();
                generator.Next(1, int.MaxValue).ToString("D10");
                db.AgentCodeList.Add(new AgentCodeList() { AgentCode = SubscribeConfig._seedUser_Prefix + generator.ToString() });
            }

            db.SaveChanges();
        }
    }
}