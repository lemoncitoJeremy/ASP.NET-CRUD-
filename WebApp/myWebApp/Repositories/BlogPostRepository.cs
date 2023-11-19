using Microsoft.EntityFrameworkCore;
using myWebApp.Data;
using myWebApp.Models.Domain;

namespace myWebApp.Repositories
{
    public class BlogPostRepository : IBlogPostRepository
    {
        private readonly webAppDbContext webAppDbContext;

        public BlogPostRepository(webAppDbContext webAppDbContext)
        {
            this.webAppDbContext = webAppDbContext;
        }


        public async Task<BlogPost> AddAsync(BlogPost blogpost)
        {
            await webAppDbContext.AddAsync(blogpost);
            await webAppDbContext.SaveChangesAsync();
            return blogpost;
        }

        public async Task<BlogPost?> DeleteAsync(Guid id)
        {
            var existingBlog = await webAppDbContext.BlogPosts.FindAsync(id);
            if (existingBlog != null) 
            {
                webAppDbContext.BlogPosts.Remove(existingBlog);
                await webAppDbContext.SaveChangesAsync();
                return existingBlog;
            }
            return null;
        }

        //List
        public async Task<IEnumerable<BlogPost>> GetAllAsync()
        {
            return await webAppDbContext.BlogPosts.Include(x => x.Tags).ToListAsync();
        }

        public async Task<BlogPost?> GetAsync(Guid id)
        {
            return await webAppDbContext.BlogPosts.Include(x=> x.Tags).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<BlogPost?> UpdateAsync(BlogPost blogpost)
        {
            var existingBlog = await webAppDbContext.BlogPosts.Include(x => x.Tags).FirstOrDefaultAsync(x => x.Id == blogpost.Id);
                if(existingBlog != null) 
                {
                    existingBlog.Id = blogpost.Id;
                    existingBlog.Heading = blogpost.Heading;
                    existingBlog.PageTitle = blogpost.PageTitle;
                    existingBlog.Content = blogpost.Content;
                    existingBlog.ShortDescription = blogpost.ShortDescription;
                    existingBlog.Author = blogpost.Author;
                    existingBlog.FeaturedImageUrl = blogpost.FeaturedImageUrl;
                    existingBlog.UrlHandle = blogpost.UrlHandle;
                    existingBlog.Visible = blogpost.Visible;
                    existingBlog.PublishedDate = blogpost.PublishedDate;    
                    existingBlog.Tags = blogpost.Tags;

                    await webAppDbContext.SaveChangesAsync();
                    return existingBlog;
                }
            return null; 
        }
    }
}
