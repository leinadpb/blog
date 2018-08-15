using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Models;

namespace Blog.Services
{
    public interface IPostService
    {
        Task Add(Post post);
        Task Update(Post newPost);
        Task Delete(Post post);
        Task<Post> GetPostById(int? postId);
        Task<IEnumerable<Post>> GetAll();
        IEnumerable<Post> GetPostByNumberOfComments(int numberOfComments, bool greaterThan, bool ascendingOrder);
        Task<bool> Exists(int postId);
        Task AddComment(Post post, Comment comment);
        Task<IEnumerable<Comment>> GetComments(int postId);
    }
}
