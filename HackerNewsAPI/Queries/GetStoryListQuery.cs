using HackerNewsAPI.Models;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace HackerNewsAPI.Queries
{
    public class GetStoryListQuery : IRequest<List<StoryDetails>>
    {

    }
}
