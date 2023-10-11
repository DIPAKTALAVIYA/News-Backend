using HackerNewsAPI.Models;
using HackerNewsAPI.Queries;
using HackerNewsAPI.Repositories;
using MediatR;

namespace HackerNewsAPI.Handlers
{
    public class GetStoryListHandler : IRequestHandler<GetStoryListQuery, List<StoryDetails>>
    {
        private readonly IStoryRepository _storyRepository;

        public GetStoryListHandler(IStoryRepository storyRepository)
        {
            _storyRepository = storyRepository;
        }
        public async Task<List<StoryDetails>> Handle(GetStoryListQuery request, CancellationToken cancellationToken)
        {
            return await _storyRepository.GetStoryListAsync();
        }
    }
}
