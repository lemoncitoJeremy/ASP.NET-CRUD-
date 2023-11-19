using myWebApp.Models.Domain;

namespace myWebApp.Repositories
{
    public interface IBlogPostRepository
    {
        Task<IEnumerable<BlogPost>> GetAllAsync();
        
        Task<BlogPost> AddAsync(BlogPost blogpost);
        Task<BlogPost?> GetAsync(Guid id);
        Task<BlogPost?> UpdateAsync(BlogPost blogpost);
        Task<BlogPost?> DeleteAsync(Guid id);
    }
}
