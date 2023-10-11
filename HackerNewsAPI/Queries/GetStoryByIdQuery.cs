using HackerNewsAPI.Models;
using MediatR;

namespace HackerNewsAPI.Queries
{
    public class GetStoryByIdQuery : IRequest<StoryDetails>
    {
        public int Id { get; set; }
    }
}
