using HackerNewsAPI.Models;
using HackerNewsAPI.Repositories;
using Moq;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Json;


namespace HackerNewsTest.Repositories
{
    public class StoryRepositoryTests
    {
        [Fact]
        public async Task GetStoryListAsync_ReturnsListOfStories()
        {
            // Arrange
            var storyIds = new List<int> { 37846601, 37846588, 37846599 };
            var stories = new List<StoryDetails>
            {
                new StoryDetails { Id=37846601,Title="Unity CEO is out, but that still may not be enough for developers",
                    By="serverlessmom",Url="https://www.theverge.com/2023/10/10/23911338/unity-ceo-steps-down-developers-react" },
                new StoryDetails { Id=37846588,Title="Wingcopter delivers groceries by drone to remote districts in Germany",
                    By="thunderbong",Url="https://newatlas.com/drones/wingcopter-frankfurt-uas-drolex-drone-cargo-ebike/" },
                new StoryDetails {Id=37846599,Title="Osiris-Rex: NASA reveals first look at 'beautiful' asteroid sample",
                    By="iamben",Url="https://www.bbc.co.uk/news/science-environment-67078632"},
            };

            var httpClientMock = new Mock<HttpClient>();
            httpClientMock
                .Setup(client => client.GetAsync("https://hacker-news.firebaseio.com/v0/newstories.json"))
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(JsonConvert.SerializeObject(storyIds))
                });

            var repository = new StoryRepository(httpClientMock.Object);

            // Act
            var result = await repository.GetStoryListAsync();

            // Assert
            Assert.Equal(stories, result);
        }

        [Fact]
        public async Task GetStoryByIdAsync_ReturnsStoryDetails()
        {
            // Arrange
            int storyId = 37846601;
            var story = new StoryDetails
            {
                Id = storyId,
                Title = "Unity CEO is out, but that still may not be enough for developers",
                By = "serverlessmom",
                Url = "https://www.theverge.com/2023/10/10/23911338/unity-ceo-steps-down-developers-react"

            };

            var httpClientMock = new Mock<HttpClient>();
            httpClientMock
                .Setup(client => client.GetAsync($"https://hacker-news.firebaseio.com/v0/item/{storyId}.json"))
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(JsonConvert.SerializeObject(story))
                });

            var repository = new StoryRepository(httpClientMock.Object);

            // Act
            var result = await repository.GetStoryByIdAsync(storyId);

            // Assert
            Assert.Equal(story, result);
        }
    }
}
