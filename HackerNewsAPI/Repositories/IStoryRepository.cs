using HackerNewsAPI.Models;

namespace HackerNewsAPI.Repositories
{
    public interface IStoryRepository
    {
        public Task<List<StoryDetails>> GetStoryListAsync();
        public Task<StoryDetails> GetStoryByIdAsync(int Id);
    }
}
