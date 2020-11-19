using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using NLog.Web;
using System.IO;
using System.Linq;
namespace BlogsConsole
{
    public class BloggingContext : DbContext
    {
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }

        public void AddBlog(Blog blog)
        {
            this.Blogs.Add(blog);
            this.SaveChanges();

        }

        public void displayPost(int choice)
        {
            //have a diffrent option for displaying for each choice, i can probably use the query thing to get all blogs 
            //set choice to blog id but if they choose to display all posts set choice to something else, as the number of blogs will increase 
            if (choice == -1)
            {
                var query = Posts.OrderBy(b => b.Title);

                Console.WriteLine("All posts in the database:");
                foreach (var item in query)
                {
                    item.displayPost();
                }
            }
            var query2 = Blogs.OrderBy(b => b.BlogId);
            foreach (var item in query2)
            {
                if (item.BlogId == choice)
                {
                    Posts.Find(choice); //i need a way to display this
                }
            }
        }

        public void AddPost(Post post)
        {
            this.Posts.Add(post);
            this.SaveChanges();
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .Build();
            optionsBuilder.UseSqlServer(@config["BloggingContext:ConnectionString"]);
        }
    }
}
