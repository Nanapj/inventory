using Inventory.Core;
using Inventory.Domain.Interface;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Data
{
    public class InventoryContext : IdentityDbContext<ApplicationUser>, IInventoryContext
    {
        public InventoryContext()
            : base("DefaultConnection")
        {

        }

        public static InventoryContext Create()
        {
            return new InventoryContext();
        }

        public DbSet<Warehouse> Warehouse { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<Note> Note { get; set; }
        public DbSet<NoteDetail> NoteDetail {get; set;}
        public DbSet<Unit> Unit { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Note>()
                .HasOptional(x => x.Source)
                .WithMany(y => y.SourceNote)
                .Map(
                    m =>
                    {
                        m.MapKey("Source_Id");
                    }
                )
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<Note>()
                .HasOptional(x => x.Destination)
                .WithMany(y => y.DestinationNote)
                .Map(m =>
                {
                    m.MapKey("Destination_Id");
                })
                .WillCascadeOnDelete(false);              
        }

        //public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    }
}
