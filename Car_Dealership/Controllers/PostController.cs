using Car_Dealership.Data;
using Car_Dealership.Models;
using Car_Dealership.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Car_Dealership.Controllers
{
  
        public class PostController : Controller
        {
            private readonly ILogger<HomeController> _logger;
            private readonly ApplicationDbContext _context;
            private readonly UserManager<ApplicationUser> _userManager;
            private readonly IHostingEnvironment _hostingEnvironment;
            private readonly SignInManager<ApplicationUser> _signInManager;
            public PostController(ILogger<HomeController> logger, ApplicationDbContext context, UserManager<ApplicationUser> userManager, IHostingEnvironment hostingEnvironment, SignInManager<ApplicationUser> signInManager)
            {
                _logger = logger;
                _context = context;
                _userManager = userManager;
                _hostingEnvironment = hostingEnvironment;
                _signInManager = signInManager;
            }
            [HttpGet]
            public IActionResult CreatePost()
            {
                return View();
            }

            [Authorize]
            [HttpPost]
            public async Task<IActionResult> CreatePost(CreatePostViewModel model)
            {
                if (ModelState.IsValid)
                {
                    string uniqueFileName = null;


                    if (model.Photo != null)
                    {
                        string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "postphotos");

                        uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                        FileStream fileStream = new FileStream(filePath, FileMode.Create);
                        model.Photo.CopyTo(fileStream);
                        fileStream.Close();

                    }
                    var user = await _userManager.GetUserAsync(User);
                    Post post = new Post
                    {
                        Title = model.Title,
                        Photo = uniqueFileName,
                        UserId = user.Id,
                        CreatedOn = DateTime.Now,
                        Likes = 0
                    };

                    var defaultLike = new UserPostLike
                    {
                        UserId = user.Id,
                        PostId = post.Id
                    };
                    _context.Posts.Add(post);
                    _context.SaveChanges();
                    return RedirectToAction("Index", "Home");
                }

                return View();
            }
            [HttpGet]
            public async Task<IActionResult> PostDetails(int id)
            {
                if (ModelState.IsValid)
                {
                    var post = _context.Posts.Find(id);

               
                    var user = await _userManager.FindByIdAsync(post.UserId);
                    var model = new ListPostsViewModel()
                    {
                        Id = post.Id,
                        UserId = user.Id,
                        Username = user.User_Username,
                        UserProfilePicture = user.ProfilePicture,
                        Title = post.Title,
                       
                        Photo = post.Photo,
                        CreatedOn = post.CreatedOn,
                        Likes = post.Likes
                       

                    };
                    if (_signInManager.IsSignedIn(User))
                    {
                        ApplicationUser applicationUser = await _userManager.GetUserAsync(User);
                        if ((_context.UserPostLikes.Where(x => x.PostId == model.Id && user.Id.Equals(x.UserId)).SingleOrDefault()) != null)
                        {
                            model.isLiked = true;
                        }
                    }
                    else
                    {
                        model.isLiked = false;
                    }

                    return View(model);
                }
                else
                {
                    return RedirectToAction("Index", "Home", new { area = "" });
                }
            }
            
            [HttpPost]
            [Authorize]
            public bool Add(int postId)
            {
                if (ModelState.IsValid)
                {
                    var post = _context.Posts.Find(postId);
                    var user = _userManager.GetUserId(User);

                    var userLiked = _context.UserPostLikes.Where(x => x.PostId == postId && x.UserId.Equals(user)).SingleOrDefault();
                    if (userLiked != null)
                    {
                        _context.UserPostLikes.Remove(userLiked);
                        post.Likes -= 1;
                        _context.Posts.Update(post);
                        _context.SaveChanges();
                        return false;
                    }

                    UserPostLike userLike = new UserPostLike()
                    {
                        UserId = user,
                        PostId = post.Id
                    };
                    post.Likes += 1;


                    _context.Posts.Update(post);
                    _context.UserPostLikes.Add(userLike);
                    _context.SaveChanges();
                    return true;
                }
                return true;
            }

          
            //Delete the post and redirect user/admin to Index/Home
            public async Task<IActionResult> DeletePost(int id)
            {
                var post = _context.Posts.Find(id);
                var user = await _userManager.GetUserAsync(User);
                if (user.Id.Equals(post.UserId) || User.IsInRole("Admin") && post != null)
                {
                    //Remove the likes from the database
                    var postLikes = _context.UserPostLikes.Where(x => x.PostId == post.Id).ToList();
                    foreach (var item in postLikes)
                    {
                        _context.UserPostLikes.Remove(item);
                    }
                    //Remove the photo inside the postphotos folder.
                    var photoPath = post.Photo;
                    string _imageToBeDeleted = Path.Combine(_hostingEnvironment.WebRootPath, "postphotos\\", photoPath);
                    if ((System.IO.File.Exists(_imageToBeDeleted)))
                    {
                        System.IO.File.Delete(_imageToBeDeleted);
                    }
                    
                    //Finally remove the post and redirect user to index, home.
                    _context.Posts.Remove(post);
                    _context.SaveChanges();

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    //Refresh page.
                    return RedirectToAction("PostDetails", "Post", new { id = post.Id });
                }
            }
        }
    }

