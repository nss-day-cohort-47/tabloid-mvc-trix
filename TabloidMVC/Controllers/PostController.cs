using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using System.Collections.Generic;
using System.Security.Claims;
using TabloidMVC.Models;
using TabloidMVC.Models.ViewModels;
using TabloidMVC.Repositories;

namespace TabloidMVC.Controllers
{
    [Authorize]
    public class PostController : Controller
    {
        private readonly IPostRepository _postRepository;
        private readonly ICategoryRepository _categoryRepository;

        public PostController(IPostRepository postRepository, ICategoryRepository categoryRepository)
        {
            _postRepository = postRepository;
            _categoryRepository = categoryRepository;
        }

        public IActionResult Index()
        {
            var posts = _postRepository.GetAllPublishedPosts();
            return View(posts);
        }

        // Starter for the 'My Post' right now it is not working.
        public IActionResult MyPosts()
        {
            int userId = GetCurrentUserProfileId();
            var posts = _postRepository.GetAllPostsByUser(userId);
            return View(posts);
        }

        public IActionResult Details(int id)
        {
            var post = _postRepository.GetPublishedPostById(id);
            if (post == null)
            {
                int userId = GetCurrentUserProfileId();
                post = _postRepository.GetUserPostById(id, userId);
                if (post == null)
                {
                    return NotFound();
                }
            }
            return View(post);
        }

        public IActionResult Create()
        {
            var vm = new PostCreateViewModel();
            vm.CategoryOptions = _categoryRepository.GetAll();
            return View(vm);
        }

        [HttpPost]
        public IActionResult Create(PostCreateViewModel vm)
        {
            try
            {
                vm.Post.CreateDateTime = DateAndTime.Now;
                vm.Post.IsApproved = true;
                vm.Post.UserProfileId = GetCurrentUserProfileId();

                _postRepository.Add(vm.Post);

                return RedirectToAction("Details", new { id = vm.Post.Id });
            }
            catch
            {
                vm.CategoryOptions = _categoryRepository.GetAll();
                return View(vm);
            }
        }

        // Edit and Delete will probably need to refactor, especially for a soft delete. 
        public IActionResult Edit(int id)
        {
            int userId = GetCurrentUserProfileId();

            Post post = _postRepository.GetPublishedPostById(id);

            // If the post has null value or it is not the user's post, they shall not pass
            if (post == null || userId != post.UserProfileId)
            {
                return NotFound();
            }
            // However, if one of those is true then return the post for editing.
            return View(post);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Post post)
        {
            // Since we only want the authenticated, authorized logged in user to edit the post
            // then] we can just go directly to edit the post and save it.
            post.UserProfileId = GetCurrentUserProfileId();
            _postRepository.Edit(post);
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            Post post = _postRepository.GetPublishedPostById(id);
            return View(post);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Post post)
        {
            _postRepository.Delete(id);
            return RedirectToAction("Index");
        }

        private int GetCurrentUserProfileId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }
    }
}