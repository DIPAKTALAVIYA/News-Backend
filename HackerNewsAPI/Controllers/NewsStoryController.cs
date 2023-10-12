using HackerNewsAPI.Common;
using HackerNewsAPI.Models;
using HackerNewsAPI.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace HackerNewsAPI.Controllers
{
    /// <summary>
    /// Controller for managing news stories.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class NewsStoryController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMemoryCache _cache;


        /// <summary>
        /// Initializes a new instance of the <see cref="NewsStoryController"/> class.
        /// </summary>
        /// <param name="mediator">The mediator for handling requests.</param>
        /// <param name="cache">The memory cache for storing news stories.</param>
        public NewsStoryController(IMediator mediator, IMemoryCache cache)
        {
            _mediator = mediator;
            _cache = cache;
        }


        /// <summary>
        /// Retrieves a list of news stories.
        /// If the stories are not cached, it fetches them from the API and caches them for future use.
        /// </summary>
        /// <returns>A list of <see cref="StoryDetails"/> representing the news stories.</returns>
        [HttpGet]
        public async Task<List<StoryDetails>> GetStoryListAsync()
        {
            try
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
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
