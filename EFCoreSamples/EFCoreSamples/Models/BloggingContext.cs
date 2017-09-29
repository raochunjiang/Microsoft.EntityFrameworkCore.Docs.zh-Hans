using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace EFCoreSamples.Models
{
    public class BloggingContext:DbContext
    {
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }

        public int TenantId { get; set; }

        public BloggingContext(DbContextOptions options)
            :base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer(@"Server=raochunjiang;Database=EFCoreSamples;Trusted_Connection=True;");
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Post>()
                .HasQueryFilter(p=> p.IsDeleted && p.TenantId==this.TenantId)
                .HasOne(p => p.Blog).WithMany(b => b.Posts)
                .HasForeignKey(p => p.BlogId);

            modelBuilder.Entity<Post>().ToTable("Posts");
            modelBuilder.Entity<Blog>().ToTable("Blogs");

            modelBuilder.ApplyConfiguration<Blog>(new BlogConfiguration());
        }

        [DbFunction]
        public static int PostReadCount(int blogId)
        {
            throw new NotImplementedException();
        }
    }
}
