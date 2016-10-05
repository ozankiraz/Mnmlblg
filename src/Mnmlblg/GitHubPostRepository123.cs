using System.Collections.Generic;
using Mnmlblg.Models;

namespace Mnmlblg
{
    public class GitHubPostRepository123 : IPostRepository
    {
        public IEnumerable<Post> AllPosts { get; }
        public IEnumerable<Post> GetPostsForPaginatedPage(int pageNumber)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Post> GetPostsForHomePage()
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Post> GetPostsByCategory(string categoryName)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Post> GetPostsForPaginatedCategoryPage(int pageNumber, string category)
        {
            throw new System.NotImplementedException();
        }

        public Post GetPostByTitle(string title)
        {
            throw new System.NotImplementedException();
        }

        public int TotalPosts { get; }
        public int GetPostsCountForCategory(string category)
        {
            throw new System.NotImplementedException();
        }
    }
}