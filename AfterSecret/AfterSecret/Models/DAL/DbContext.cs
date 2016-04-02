using System;
using System.Collections.Generic;
using System.Data.Entity;
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

        public DbSet<Member> Member { get; set; }
    }
}