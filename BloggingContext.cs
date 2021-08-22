using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace scheduled_ecs_dotnet
{
    public class BloggingContext : DbContext
    {
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<RawBlog> RawBlogs { get; set; }

        public string DbPath { get; set; }

        public BloggingContext()
        {
            var folder = Environment.SpecialFolder.Personal;
            var path = Environment.GetFolderPath(folder);
            DbPath = $"{path}{System.IO.Path.DirectorySeparatorChar}blogging.db";
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // base.OnModelCreating(builder);
            builder.Entity<RawBlog>()
                      .HasKey(x => new { x.BlogId });
        }

        public class Blog
        {
            public int BlogId { get; set; }
            public string Url { get; set; }
            public List<Post> Posts = new List<Post>();
        }

        public class Post
        {
            public int PostId { get; set; }
            public string Title { get; set; }
            public string Content { get; set; }

            public int BlogId { get; set; }
            public Blog Blog { get; set; }
        }

        public class RawBlog
        {
            public int BlogId { get; set; }
            public string Url { get; set; }
        }
    }
}
