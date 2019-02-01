using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using SkyEntity;


namespace SkyDal
{
    public class SkyWebContext : DbContext
    {
        public SkyWebContext()
            : base("SkyWebContext")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        public DbSet<Setting> Settings { get; set; }
     
        public DbSet<Category> Categorys { get; set; }
    
        public DbSet<Notice> Notices { get; set; }

        public DbSet<SysUser> SysUsers { get; set; }

        public DbSet<WechatReply> WechatReplys { get; set; }

        public DbSet<Guanggao> Guanggaos { get; set; }

        public DbSet<Ren> Rens { get; set; }

        public DbSet<Book> Books { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<RenwuDaka> RenwuDakas { get; set; }

        public DbSet<Renwu> Renwus { get; set; }

        public DbSet<ChanpinOrder> ChanpinOrders { get; set; }

    }
}
