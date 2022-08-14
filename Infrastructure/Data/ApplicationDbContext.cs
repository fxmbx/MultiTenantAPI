using System;
using Core.Contracts;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        private string TenantId { get; set; }
        private readonly ITenantService tenantService;
        public ApplicationDbContext(DbContextOptions options, ITenantService tenantService) : base(options)
        {
            this.tenantService = tenantService;
            TenantId = this.tenantService.GetTenant()?.Data?.TID;
        }
        public DbSet<Product> products;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Product>().HasQueryFilter(x => x.TenantId == TenantId);
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var tenantConnectionString = this.tenantService.GetConnectionString().Data;
            if (!string.IsNullOrEmpty(tenantConnectionString))
            {
                var DbProvider = this.tenantService.GetDatabaseProvider().Data;
                if(DbProvider.ToLower() == "mssql")
                {
                    optionsBuilder.UseSqlServer(this.tenantService.GetConnectionString().Data);
                }

            }
        }
       public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach(var a in ChangeTracker.Entries<IMustHaveTenant>().ToList())
            {
                switch (a.State)
                {
                    case EntityState.Added:
                       
                    case EntityState.Modified:
                        a.Entity.TenantId = TenantId;
                        break;
                }
            }
            var result = await base.SaveChangesAsync(cancellationToken);
            return result;
        }
    }
}

