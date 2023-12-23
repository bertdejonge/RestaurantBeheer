using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RestaurantProject.Datalayer.Models;

namespace RestaurantProject.Datalayer.Data {
    public class RestaurantDbContext : DbContext {

        // TABLES
        public DbSet<ReservationEF> Reservations { get; set; }
        public DbSet<RestaurantEF> Restaurants { get; set;}
        public DbSet<TableEF> Tables { get; set; }
        public DbSet<UserEF> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseSqlServer(@"Data Source=HP_BERT\SQLEXPRESS;Initial Catalog=Restaurant_Db;Integrated Security=True;TrustServerCertificate=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            // modelBuilder.Entity<Reservation>()
            // .HasKey(r => r.ReservationID);

            // modelBuilder.Entity<Reservation>()
            //.HasOne(r => r.Restaurant)
            //.WithMany()
            //.HasForeignKey(r => r.Restaurant);

            // modelBuilder.Entity<Reservation>()
            // .HasOne(r => r.User)
            // .WithMany()
            // .HasForeignKey(r => r.User);

            //modelBuilder.Entity<RestaurantEF>()
            //.HasMany(r => r.Reservations)
            //.WithOne(r => r.Restaurant)
            //.OnDelete(DeleteBehavior.Restrict);

            //modelBuilder.Entity<UserEF>()
            //.HasMany(u => u.Reservations)
            //.WithOne(r => r.User)
            //.OnDelete(DeleteBehavior.Restrict);

        }
    }
}
