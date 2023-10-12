using HackerNewsAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace HackerNewsAPI.Repositories
{
    public class StoryRepository : IStoryRepository
    {
        /// <summary>
        /// Gets the HTTP client used for making web requests.
        /// </summary>
        public HttpClient Client { get; }


        /// <summary>
        /// Initializes a new instance of the <see cref="StoryRepository"/> class.
        /// </summary>
        /// <param name="client">The HTTP client to use for making requests.</param>
        public StoryRepository(HttpClient client)
        {
            Client = client;
        }


        /// <summary>
        /// Asynchronously retrieves a list of Hacker News stories.
        /// </summary>
        /// <returns>A list of <see cref="StoryDetails"/> objects representing the stories.</returns>
        public async Task<List<StoryDetails>> GetStoryListAsync()
        {
            List<StoryDetails> stories = new List<StoryDetails>();

            var result = await Client.GetAsync("https://hacker-news.firebaseio.com/v0/newstories.json");

            if (result.IsSuccessStatusCode)
            {
                var storiesResponse = result.Content.ReadAsStringAsync().Result;
                var storyIds = JsonConvert.DeserializeObject<List<int>>(storiesResponse);

                storyIds = storyIds.Take(200).ToList();

                var tasks = storyIds?.Select(GetStoryByIdAsync);
                if (tasks != null)
                    stories = (await Task.WhenAll(tasks)).ToList();
            }
            return stories;
        }

        /// <summary>
        /// Asynchronously retrieves a Hacker News story by its ID.
        /// </summary>
        /// <param name="Id">The ID of the story to retrieve.</param>
        /// <returns>A <see cref="StoryDetails"/> object representing the story.</returns>
        public async Task<StoryDetails> GetStoryByIdAsync(int Id)
        {
            StoryDetails story = new StoryDetails();
            var result = await Client.GetAsync(string.Format("https://hacker-news.firebaseio.com/v0/item/{0}.json", Id));

            if (result.IsSuccessStatusCode)
            {
                var storyResponse = result.Content.ReadAsStringAsync().Result;
                if (storyResponse != null)
                    story = JsonConvert.DeserializeObject<StoryDetails>(storyResponse);
            }

            return story;
        }
    }
}
