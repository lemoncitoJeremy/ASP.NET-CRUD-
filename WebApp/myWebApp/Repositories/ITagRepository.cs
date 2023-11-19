using myWebApp.Models.Domain;

namespace myWebApp.Repositories
{
    public interface ITagRepository
    {
        Task<IEnumerable<Tag>> GetAllAsync();

        //single tag Task<Tag>
        Task<Tag?> GetAsync(Guid id);

        Task<Tag> AddAsync(Tag tag);

        // ? means it can return null
        Task<Tag?> UpdateAsync(Tag tag);

        Task<Tag?> DeleteAsync(Guid id);
    }
}
