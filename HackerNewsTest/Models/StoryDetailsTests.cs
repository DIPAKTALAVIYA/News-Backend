
using HackerNewsAPI.Models;
using System;

namespace HackerNewsTest.Models
{
    public class StoryDetailsTests
    {
        [Fact]
        public void StoryDetails_Initialization()
        {
            // Arrange
            long id = 37846601;
            string title = "Unity's CEO is out, but that still may not be enough for developers";
            string url = "https://www.theverge.com/2023/10/10/23911338/unity-ceo-steps-down-developers-react";
            string by = "serverlessmom";

            // Act
            var storyDetails = new StoryDetails
            {
                Id = id,
                Title = title,
                Url = url,
                By = by
            };

            // Assert
            Assert.Equal(id, storyDetails.Id);
            Assert.Equal(title, storyDetails.Title);
            Assert.Equal(url, storyDetails.Url);
            Assert.Equal(by, storyDetails.By);
        }

        [Fact]
        public void StoryDetails_Equality()
        {
            // Arrange
            var storyDetails1 = new StoryDetails
            {
                Id = 37846601,
                Title = "Unity CEO is out, but that still may not be enough for developers",
                Url = "https://www.theverge.com/2023/10/10/23911338/unity-ceo-steps-down-developers-react",
                By = "serverlessmom"
            };

            var storyDetails2 = new StoryDetails
            {
                Id = 37846602,
                Title = "Unity CEO is out, but that still may not be enough for developers",
                Url = "https://www.theverge.com/2023/10/10/23911338/unity-ceo-steps-down-developers-react",
                By = "serverlessmom"
            };

            // Assert
            Assert.NotEqual(storyDetails1, storyDetails2);
        }
    }
}
