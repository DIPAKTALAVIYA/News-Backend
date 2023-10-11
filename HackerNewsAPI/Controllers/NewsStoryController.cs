using HackerNewsAPI.Common;
using HackerNewsAPI.Models;
using HackerNewsAPI.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace HackerNewsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NewsStoryController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMemoryCache _cache;
        public NewsStoryController(IMediator mediator, IMemoryCache cache)
        {
            _mediator = mediator;
            _cache = cache;
        }

        [HttpGet]
        public async Task<List<StoryDetails>> GetStoryListAsync()
        {
            if (!_cache.TryGetValue(CacheKeys.NewStories, out List<StoryDetails> storyDetails))
            {
                storyDetails = await _mediator.Send(new GetStoryListQuery()); // API Call

                var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddMinutes(5),
                    SlidingExpiration = TimeSpan.FromMinutes(2),
                    Size = 1024,
                };
                _cache.Set(CacheKeys.NewStories, storyDetails, cacheEntryOptions);
            }

            return storyDetails;
        }
    }
}
