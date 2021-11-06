using System;
using Xunit;

using MangaDexSharp.Parameters;

namespace MangaDexSharp.Tests.Parameters
{
    public class IncludeParametersTests
    {
        [Fact]
        public void IncludesArtistValid()
        {
            var includes = new IncludeParameters()
            {
                IncludeArtist = true
            };

            string includesQuery = includes.ToQueryString();

            Assert.Equal("includes[]=artist", includesQuery);
        }
        
        [Fact]
        public void IncludesAuthorValid()
        {
            var includes = new IncludeParameters()
            {
                IncludeAuthor = true
            };

            string includesQuery = includes.ToQueryString();

            Assert.Equal("includes[]=author", includesQuery);
        }
        
        [Fact]
        public void IncludesCoverValid()
        {
            var includes = new IncludeParameters()
            {
                IncludeCover = true
            };

            string includesQuery = includes.ToQueryString();

            Assert.Equal("includes[]=cover_art", includesQuery);
        }
        
        [Fact]
        public void IncludesGroupLeaderValid()
        {
            var includes = new IncludeParameters()
            {
                IncludeLeader = true
            };

            string includesQuery = includes.ToQueryString();

            Assert.Equal("includes[]=leader", includesQuery);
        }
        
        [Fact]
        public void IncludesGroupMemberValid()
        {
            var includes = new IncludeParameters()
            {
                IncludeMembers = true
            };

            string includesQuery = includes.ToQueryString();

            Assert.Equal("includes[]=member", includesQuery);
        }
        
        [Fact]
        public void IncludesMangaValid()
        {
            var includes = new IncludeParameters()
            {
                IncludeManga = true
            };

            string includesQuery = includes.ToQueryString();

            Assert.Equal("includes[]=manga", includesQuery);
        }
        
        [Fact]
        public void IncludesScanlationGroupValid()
        {
            var includes = new IncludeParameters()
            {
                IncludeScanlationGroup = true
            };

            string includesQuery = includes.ToQueryString();

            Assert.Equal("includes[]=scanlation_group", includesQuery);
        }

        [Fact]
        public void IncludesUserValid()
        {
            var includes = new IncludeParameters()
            {
                IncludeUser = true
            };

            string includesQuery = includes.ToQueryString();

            Assert.Equal("includes[]=user", includesQuery);
        }
        
        [Fact]
        public void IncludesMultipleValid_Two()
        {
            var includes = new IncludeParameters()
            {
                IncludeLeader = true,
                IncludeMembers = true
            };

            string includesQuery = includes.ToQueryString();

            Assert.Equal("includes[]=leader&includes[]=member", includesQuery);
        }

        
        
        [Fact]
        public void IncludesMultipleValid_Three()
        {
            var includes = new IncludeParameters()
            {
                IncludeManga = true,
                IncludeArtist = true,
                IncludeAuthor = true
            };

            string includesQuery = includes.ToQueryString();

            Assert.Equal("includes[]=artist&includes[]=author&includes[]=manga", includesQuery);
        }
    }
}