using System;
using NLog.Web;
using System.IO;
using System.Linq;

namespace BlogsConsole
{
    class Program
    {
        // create static instance of Logger
        private static NLog.Logger logger = NLogBuilder.ConfigureNLog(Directory.GetCurrentDirectory() + "\\nlog.config").GetCurrentClassLogger();
        static void Main(string[] args)
        {
            logger.Info("Program started");
            string userchoice = "";
            var db = new BloggingContext();
            try
            {
                do
                {
                    Console.WriteLine("Please type 1 to create a blog, 2 to display all blogs, 3 to post to a blog, or 4 to display posts");
                    userchoice = Console.ReadLine();

                    if (userchoice == "1")
                    {
                        // Create and save a new Blog
                        Console.Write("Enter a name for a new Blog: ");
                        var name = Console.ReadLine();

                        var blog = new Blog { Name = name };
                        // ill need to assign a blogid to the create blog bit of code and then connect it with the add post method

                        db.AddBlog(blog);
                        logger.Info("Blog added - {name}", name);
                    }

                    if (userchoice == "2")
                    {
                        // Display all Blogs from the database
                        var query = db.Blogs.OrderBy(b => b.Name);

                        Console.WriteLine("All blogs in the database:");
                        foreach (var item in query)
                        {
                            Console.WriteLine(item.Name);
                        }
                    }
                    if (userchoice == "3")
                    {
                        Console.WriteLine("Please enter the id of the blog you wish to post to");
                        int bloggerId = int.Parse(Console.ReadLine());
                        Console.WriteLine("What is the title of your post?");
                        var title = Console.ReadLine();
                        Console.WriteLine("Please enter all content of the post");
                        var content = Console.ReadLine();
                        var post = new Post { Title = title, Content = content, BlogId = bloggerId };
                        db.AddPost(post);
                        db.SaveChanges();
                        // Blog blog23 = db.Blogs.FirstOrDefault(c => c.BlogId == bloggerId);
                        // blog23.Posts.Add(post);


                    }
                    if (userchoice == "4")
                    {
                        Console.WriteLine("Type '1' to display all posts from all blogs, or enter 2 to show posts from a specific blog");
                        int choice = int.Parse(Console.ReadLine());

                        if (choice == 1)
                        {
                            var query = db.Blogs.OrderBy(b => b.BlogId);
                            foreach (var item in query)
                            {
                                Console.WriteLine(item.Name);
                                foreach (Post p in item.Posts)
                                {
                                    Console.WriteLine(p.Title + "\n" + p.Content);
                                }
                            }

                        }
                        else if (choice == 2)
                        {
                            var dbb = new BloggingContext();
                            var query = db.Blogs.OrderBy(b => b.BlogId);
                            Console.WriteLine("select the blog whose post you would like to see");
                            foreach (var item in query)
                            {
                                Console.WriteLine(item.BlogId + " " + item.Name);
                            }
                            int id = int.Parse(Console.ReadLine());
                            Console.Clear();
                            Blog blog2 = db.Blogs.FirstOrDefault(c => c.BlogId == id);
                            Console.WriteLine(blog2.BlogId + " " + blog2.Name);
                            foreach (Post p in blog2.Posts)
                            {
                                Console.WriteLine(p.Title + "\n" + p.Content);
                            }
                        }
                    }

                }
                while (userchoice == "1" || userchoice == "2" || userchoice == "3" || userchoice == "4");

            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }

            logger.Info("Program ended");
        }
    }
}
