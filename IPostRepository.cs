using System.Collections.Generic;
using Mnmlblg.Models;

public interface IPostRepository
{
    IEnumerable<Post> AllPosts { get; }

    IEnumerable<Post> GetPostsForPaginatedPage(int pageNumber);

    IEnumerable<Post> GetPostsForHomePage();

    IEnumerable<Post> GetPostsByCategory(string categoryName);

    IEnumerable<Post> GetPostsForPaginatedCategoryPage(int pageNumber, string category);

    Post GetPostByTitle(string title);

    int TotalPosts { get; }
    
    int GetPostsCountForCategory(string category);
}