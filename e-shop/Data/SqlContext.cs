using e_shop.Entities;
using Microsoft.EntityFrameworkCore;

namespace e_shop.Data
{
    public class SqlContext : DbContext
    {
        public SqlContext(DbContextOptions<SqlContext> options) : base(options)
        {
        }

        protected SqlContext()
        {
        }

        //lägg in listor
        public virtual DbSet<CostumersAddressEntity> CostumersAddresses { get; set; }
        public virtual DbSet<PassWordsEntity> PassWords { get; set; }
        public virtual DbSet<CostumersEntity> Costumers { get; set; }
        public virtual DbSet<StatusEntity> Statuses { get; set; }
        public virtual DbSet<ProductInventoryEntity> ProductInventorys { get; set; }
        public virtual DbSet<CategoryEntity> Categories { get; set; }
        public virtual DbSet<ProductsEntity> Products { get; set; }
        public virtual DbSet<OrdersEntity> Orders { get; set; }
        public virtual DbSet<OrderRowEntity> OrderRows { get; set; }
    }
}
