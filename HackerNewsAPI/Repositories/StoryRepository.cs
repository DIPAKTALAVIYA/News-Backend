﻿using HackerNewsAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace HackerNewsAPI.Repositories
{
    public class StoryRepository : IStoryRepository
    {
        private static HttpClient _client = new HttpClient();

        public StoryRepository()
        {
        }

        public async Task<List<StoryDetails>> GetStoryListAsync()
        {
            List<StoryDetails> stories = new List<StoryDetails>();

            var result = await _client.GetAsync("https://hacker-news.firebaseio.com/v0/newstories.json");

            if (result.IsSuccessStatusCode)
            {
                var storiesResponse = result.Content.ReadAsStringAsync().Result;
                var storyIds = JsonConvert.DeserializeObject<List<int>>(storiesResponse);

                var tasks = storyIds?.Select(GetStoryByIdAsync);
                if (tasks != null)
                    stories = (await Task.WhenAll(tasks)).ToList();
            }
            return stories;
        }

        public async Task<StoryDetails> GetStoryByIdAsync(int Id)
        {
            StoryDetails story = new StoryDetails();
            var result = await _client.GetAsync(string.Format("https://hacker-news.firebaseio.com/v0/item/{0}.json", Id));

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