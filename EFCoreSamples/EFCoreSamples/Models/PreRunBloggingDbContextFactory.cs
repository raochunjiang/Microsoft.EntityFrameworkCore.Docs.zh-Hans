using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Text;

namespace EFCoreSamples.Models
{
    public class PreRunBloggingDbContextFactory : IDesignTimeDbContextFactory<BloggingContext>
    {
        public BloggingContext CreateDbContext(string[] args)
        {
            var options = new DbContextOptionsBuilder().UseSqlServer(@"Server=raochunjiang;Database=EFCoreSamples;Trusted_Connection=True;");
            return new BloggingContext(options.Options);
        }
    }
}
