using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Models;
using Microsoft.EntityFrameworkCore;

namespace Blog.Services
{
    public class PostService : IPostService
    {
        private readonly BlogContext _context;

        public PostService(BlogContext ctx)
        {
            this._context = ctx;
        }

        public Task Add(Post post)
        {
            _context.Posts.Add(post);
            _context.SaveChanges();
            return Task.CompletedTask;
        }

        public Task AddComment(Post post, Comment comment)
        {
            var p = _context.Posts.Include(pt => pt.Comments).Where(pt => pt.PostId == post.PostId).First();
            p.Comments.Add(comment);
            _context.SaveChanges();
            return Task.CompletedTask;
        }

        public Task Delete(Post post)
        {
            _context.Posts.Remove(post);
            _context.SaveChanges();
            return Task.CompletedTask;
        }

        public Task<bool> Exists(int postId)
        {
            return Task.Run(async() => {
                Post post = await _context.Posts.FindAsync(postId);
                if(post == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            });
        }

        public Task<IEnumerable<Post>> GetAll()
        {
            return Task.Run(() => {
                return _context.Posts.Include(p => p.Comments).AsEnumerable();
            });
        }

        public Task<IEnumerable<Comment>> GetComments(int postId)
        {
            return Task.Run(() => {
                var post = _context.Posts.Include(p => p.Comments).Where(p => p.PostId == postId).First();
                return post.Comments.AsEnumerable();
            });
        }

        public Task<Post> GetPostById(int? postId)
        {
            return Task.Run(() => {
                Post post = _context.Posts.Where(p => p.PostId == postId).FirstOrDefault();
                return post;
            });
        }

        public IEnumerable<Post> GetPostByNumberOfComments(int numberOfComments, bool greaterThan, bool ascendingOrder)
        {
            var result = new List<Post>();
            var posts = _context.Posts.Include(p => p.Comments);
            foreach (var item in posts)
            {
                if (item.Comments.Count >= numberOfComments)
                {
                    result.Add(item);
                }
            }
            return result.AsEnumerable();
        }

        public Task Update(Post newPost)
        {
            _context.Posts.Update(newPost);
            _context.SaveChanges();
            return Task.CompletedTask;
        }
    }
}
