using Azure.Core.Pipeline;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using server.Models;
using server.StaticData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server.Data
{
    internal class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
            : base(new DbContextOptions <ApplicationDbContext>())
        {
        }

        public DbSet<Weather> Weathers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseSqlServer(SD.ConnectionString);
        }
    }
}
