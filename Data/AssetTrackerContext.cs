using Gmt_Asset_Tracker.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gmt_Asset_Tracker.Data
{
    public class AssetTrackerContext : DbContext
    {
        public AssetTrackerContext(DbContextOptions<AssetTrackerContext> options) : base(options)
        {

        }

        public DbSet<Asset> Assets { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Asset_State> Asset_States { get; set; }
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<Physical_check> Physical_Checks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
        }
    }
}
