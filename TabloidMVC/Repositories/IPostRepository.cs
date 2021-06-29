
using System.Collections.Generic;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public interface IPostRepository
    {
        void Add(Post post);
        void Edit(Post post);
        void Delete(int id);
        List<Post> GetAllPublishedPosts();
        List<Post> GetAllPostsByUser(int id);
        Post GetPublishedPostById(int id);
        Post GetUserPostById(int id, int userProfileId);
    }
}