using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Blog.Models;
using Blog.Services;
namespace Blog.Controllers
{
    public class PostsController : Controller
    {

        private readonly IPostService _postService;

        public PostsController(IPostService postService)
        {
            this._postService = postService;
        }

        // GET: Posts
        public async Task<IActionResult> Index(bool newComment)
        {
            if (newComment)
            {
                ViewBag.NewComment = "New comment added.";
            }
            return View(await _postService.GetAll() );
        }

        // GET: Posts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _postService.GetPostById(id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // GET: Posts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PostId,Content,CreatedAt")] Post post)
        {
            if (ModelState.IsValid)
            {
                await _postService.Add(post);
                return RedirectToAction(nameof(Index));
            }
            return View(post);
        }

        // GET: Posts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _postService.GetPostById(id);
            if (post == null)
            {
                return NotFound();
            }
            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PostId,Content,CreatedAt")] Post post)
        {
            if (id != post.PostId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _postService.Update(post);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(post.PostId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(post);
        }

        // GET: Posts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _postService.GetPostById(id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var post = await _postService.GetPostById(id);
            await _postService.Delete(post);

            return RedirectToAction(nameof(Index));
        }

        private bool PostExists(int id)
        {
            return _postService.Exists(id).Result;
        }

        [HttpGet]
        public IActionResult AddComment(int? postid)
        {
            ViewBag.PostId = postid;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddComment([Bind("CommentId,Content,CreateAt,PostId")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                Post post = await _postService.GetPostById(comment.PostId);
                if(post != null)
                {
                    await _postService.AddComment(post, comment);
                    return RedirectToAction("Index", new { newComment = true });
                }
            }
            return View();
        }

        [HttpGet]
        public IActionResult Comments(int postId)
        {
            return View(_postService.GetComments(postId).Result);
        }
    }
}
