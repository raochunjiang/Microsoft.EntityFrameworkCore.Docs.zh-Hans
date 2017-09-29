using EFCoreSamples.Models;
using System;
using System.Linq;

namespace EFCoreSamples
{
    class Program
    {
        static void Main(string[] args)
        {
            // query
            using (var db = new BloggingContext())
            {
                var blogs = (from b in db.Blogs
                            where b.Rating > 3
                            orderby b.Url
                            select b).ToList();
            }

            // save
            using (var db = new BloggingContext())
            {
                var blog = new Blog { Url = "http://sample.com" };
                db.Blogs.Add(blog);
                db.SaveChanges();
            }
        }
    }
}
