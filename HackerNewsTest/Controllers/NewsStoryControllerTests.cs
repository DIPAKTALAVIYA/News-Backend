using AutoFixture;
using HackerNewsAPI.Common;
using HackerNewsAPI.Controllers;
using HackerNewsAPI.Models;
using HackerNewsAPI.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Moq;

namespace HackerNewsTest.Controllers
{
    public class NewsStoryControllerTests
    {
        [Fact]
        public async Task GetStoryListAsync_ReturnsCachedData()
        {
            // Arrange
            var cache = new MemoryCache(new MemoryCacheOptions());
            var mediatorMock = new Mock<IMediator>();
            var controller = new NewsStoryController(mediatorMock.Object, cache);

            // Create a list of test data for caching
            var testData = new List<StoryDetails>
            {
                new StoryDetails { Id=37846601,Title="Unity's CEO is out, but that still may not be enough for developers",
                    By="serverlessmom",Url="https://www.theverge.com/2023/10/10/23911338/unity-ceo-steps-down-developers-react" },
                new StoryDetails { Id=37846588,Title="Wingcopter delivers groceries by drone to remote districts in Germany",
                    By="thunderbong",Url="https://newatlas.com/drones/wingcopter-frankfurt-uas-drolex-drone-cargo-ebike/" },
                new StoryDetails {Id=37846599,Title="Osiris-Rex: NASA reveals first look at 'beautiful' asteroid sample",
                    By="iamben",Url="https://www.bbc.co.uk/news/science-environment-67078632"},
            };

            // Set the cache to for the test data
            cache.Set(CacheKeys.NewStories, testData);

            // Act
            var result = await controller.GetStoryListAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(testData, result);
            mediatorMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task GetStoryListAsync_ReturnsDataFromMediator()
        {
            // Arrange
            var cache = new MemoryCache(new MemoryCacheOptions());
            var mediatorMock = new Mock<IMediator>();
            var controller = new NewsStoryController(mediatorMock.Object, cache);

            // Mock the Mediator to return test data
            var testData = new List<StoryDetails>
            {
                new StoryDetails { Id=37846601,Title="Unity's CEO is out, but that still may not be enough for developers",
                    By="serverlessmom",Url="https://www.theverge.com/2023/10/10/23911338/unity-ceo-steps-down-developers-react" },
                new StoryDetails { Id=37846588,Title="Wingcopter delivers groceries by drone to remote districts in Germany",
                    By="thunderbong",Url="https://newatlas.com/drones/wingcopter-frankfurt-uas-drolex-drone-cargo-ebike/" },
                new StoryDetails {Id=37846599,Title="Osiris-Rex: NASA reveals first look at 'beautiful' asteroid sample",
                    By="iamben",Url="https://www.bbc.co.uk/news/science-environment-67078632"}
            };
            mediatorMock.Setup(m => m.Send(It.IsAny<GetStoryListQuery>(), default))
                .ReturnsAsync(testData);

            // Act
            var result = await controller.GetStoryListAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(testData, result);
            mediatorMock.Verify(m => m.Send(It.IsAny<GetStoryListQuery>(), default), Times.Once);
        }
    }
}