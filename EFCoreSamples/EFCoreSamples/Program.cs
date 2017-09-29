using EFCoreSamples.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace EFCoreSamples
{
    class Program
    {
        static void Main(string[] args)
        {

            var services = new ServiceCollection();
            services.AddDbContext<BloggingContext>
                (
                    options => options.UseSqlServer(@"Server=raochunjiang;Database=EFCoreSamples;Trusted_Connection=True;"),ServiceLifetime.Transient
                );
            var provider = services.BuildServiceProvider();
            // query
            using (var db = provider.GetService<BloggingContext>())
            {
                var blogs = (from b in db.Blogs
                            where b.Rating > 3
                            orderby b.Url
                            select b).ToList();
            }

            // save
            using (var db = provider.GetService<BloggingContext>())
            {
                var blog = new Blog { Url = "http://sample.com" };
                db.Blogs.Add(blog);
                db.SaveChanges();
            }

            using (var db = provider.GetService<BloggingContext>())
            {
                var query =
                    from p in db.Posts
                    where BloggingContext.PostReadCount(p.Id) > 5
                    select p;
            }
        }
    }
}
