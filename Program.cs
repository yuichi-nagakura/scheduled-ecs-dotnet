using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using static scheduled_ecs_dotnet.BloggingContext;

namespace scheduled_ecs_dotnet
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using (var context = new BloggingContext())
            {
                Console.WriteLine("Migrating database");
                context.Database.Migrate();

                Console.WriteLine($"Database path: {context.DbPath}.");

                // Create
                Console.WriteLine("Inserting new blogs");
                await context.AddAsync(new Blog { Url = "http://blogs.msdn.com/adonet" });
                await context.AddRangeAsync(
                     new Blog { Url = "https://yahoo.co.jp", },
                     new Blog { Url = "https://google.com", },
                     new Blog { Url = "https://facebook.com", }
                 );
                await context.SaveChangesAsync();

                // Read 
                Console.WriteLine("Querying for a blog ---");
                var blogs = context.Blogs.OrderBy(b => b.BlogId);
                foreach (var blog in blogs)
                {
                    Console.WriteLine($"BlogId: {blog.BlogId}, BlogUrl: {blog.Url}");
                }

                // Raw Query
                var rawBlogs = context.RawBlogs.FromSqlRaw("SELECT BlogId, Url FROM blogs");
                foreach (var blog in rawBlogs)
                {
                    Console.WriteLine($"BlogId: {blog.BlogId}, Url: {blog.Url}");
                }

                // Delete
                Console.WriteLine("Delete blogs ---");
                context.Blogs.RemoveRange(await context.Blogs.ToListAsync());
                await context.SaveChangesAsync();
            }
        }
    }
}
