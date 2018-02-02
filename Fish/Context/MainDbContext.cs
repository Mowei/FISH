using System;
using Microsoft.EntityFrameworkCore;
using FISH.DataAccess.Context;
using FISH.Models;
using FISH.Utility;

namespace FISH.Entities.Context
{
    public partial class MainDbContext : EntityContextIdentityBase<MainDbContext, ApplicationUser, ApplicationRole, Guid>
    {
        public MainDbContext(DbContextOptions<MainDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(CommEnvironment.Connectstring);
            /*
            #warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
            optionsBuilder.UseSqlServer(@"Data Source=;Initial Catalog=;Persist Security Info=True;User ID=;Password=;");
            */
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);



        }
    }
}