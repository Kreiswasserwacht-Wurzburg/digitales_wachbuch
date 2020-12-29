using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wasserwacht.DigitalGuardBook.Common.Data
{
    public class CommonDataContext : IdentityDbContext<Person, IdentityRole<Guid>, Guid>
    {
        public CommonDataContext(DbContextOptions options) : base(options)
        {
        }
    }

    public class CommonDataFactory : IDesignTimeDbContextFactory<CommonDataContext>
    {
        public CommonDataContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CommonDataContext>();
            optionsBuilder.UseSqlServer();

            return new CommonDataContext(optionsBuilder.Options);
        }
    }
}
