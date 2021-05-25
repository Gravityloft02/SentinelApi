using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SentinelApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SentinelApi.Data
{
    public class ApiDbContext : IdentityDbContext<ApplicationUser>
    {
        public  ApiDbContext(DbContextOptions<ApiDbContext>options) : base(options)
        {

        }

      
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
