using Microsoft.EntityFrameworkCore;
using myWebApp.Data;
using myWebApp.Models.Domain;

namespace myWebApp.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly webAppDbContext webAppDbContext;

        public TagRepository(webAppDbContext webAppDbContext)
        {
            this.webAppDbContext = webAppDbContext;
        }


        public async Task<Tag> AddAsync(Tag tag)
        {
            await webAppDbContext.Tags.AddAsync(tag);
            await webAppDbContext.SaveChangesAsync();
            return tag;
        }


        public async Task<Tag?> DeleteAsync(Guid id)
        {
            var existingTag = await webAppDbContext.Tags.FindAsync(id);
            if (existingTag != null) 
            {
                webAppDbContext.Tags.Remove(existingTag);
                await webAppDbContext.SaveChangesAsync();
                return existingTag;
            }
            return null;
        }


        public async Task<IEnumerable<Tag>> GetAllAsync()
        {
            return await webAppDbContext.Tags.ToListAsync();
        }


                        //Getting single id
        public async Task<Tag?> GetAsync(Guid id)
        {
           return  await webAppDbContext.Tags.FirstOrDefaultAsync(x => x.Id == id);
        }



        public async Task<Tag?> UpdateAsync(Tag tag)
        {
            var existingTag = await webAppDbContext.Tags.FindAsync(tag.Id);

            if (existingTag != null) 
            {
                existingTag.Name = tag.Name;
                existingTag.DisplayName = tag.DisplayName;
                await webAppDbContext.SaveChangesAsync();
                return existingTag;

            }
            return null;
        }

    }
}
