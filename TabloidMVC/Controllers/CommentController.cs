using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TabloidMVC.Models;
using TabloidMVC.Models.ViewModels;
using TabloidMVC.Repositories;

namespace TabloidMVC.Controllers
{
    public class CommentController : Controller
    {
        private readonly ICommentRepository _commentRepo;
        private readonly IPostRepository _postRepository;

        public CommentController(
            ICommentRepository commentRepository,
            IPostRepository postRepository)
        {
            _commentRepo = commentRepository;
            _postRepository = postRepository;
        }

        // GET: HomeController1
        public ActionResult Index(int postid)
        {

            Post post = _postRepository.GetPublishedPostById(postid);
            List<Comment> comments = _commentRepo.GetCommentsById(postid);

            CommentViewModel cvm = new CommentViewModel()
            {
                Post = post,
                Comments = comments
        };

            return View(cvm);
        }

        // GET: HomeController1/Details/5
        public ActionResult Details()
        {
            return View();
        }

        // GET: HomeController1/Create
        public ActionResult Create(int postid)
        {
            return View();
        }

        // POST: HomeController1/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Comment comment)
        {
            try
            {
                comment.UserProfileId = GetCurrentUserProfileId();
                _commentRepo.AddComment(comment);
                return RedirectToAction(("Index"));
            }
            catch (Exception ex)
            {
                return View(comment);
            }
        }

        // GET: HomeController1/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: HomeController1/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: HomeController1/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: HomeController1/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        private int GetCurrentUserProfileId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }
    }
}
