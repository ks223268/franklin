using System;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

using Microsoft.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;

using Franklin.Data.Entities;

namespace Franklin.Data
{
    public class FranklinDbContext : DbContext
    {
        string _connectionString;

        public FranklinDbContext(DbContextOptions option) : base(option) {            
        }

        /*

        public FranklinDbContext(string connectionString) {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {

            optionsBuilder.UseSqlServer(_connectionString);
        }

        *
        */
        public DbSet<MarketSecurity> Securities { get; set; }
        public DbSet<ClientOrder> ClientOrders { get; set; }

    }
}
