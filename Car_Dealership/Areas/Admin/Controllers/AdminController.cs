using Car_Dealership.Areas.Admin.ViewModels;
using Car_Dealership.Data;
using Car_Dealership.Models;
using Car_Dealership.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Car_Dealership.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminController:Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private RoleManager<IdentityRole> _roleManager;

        public AdminController(UserManager<ApplicationUser> userManager, ApplicationDbContext context, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _context = context;
            _roleManager = roleManager;
        }

        /*public RoleManager<IdentityRole> RoleManager { get; }*/
        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityRole identityRole = new IdentityRole{Name = model.RoleName};
                IdentityResult result = await _roleManager.CreateAsync(identityRole);
            
            /*if (result.Succeeded)
            {
               *//* return RedirectToAction("index","home");*//*

            }*/
            foreach(IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }
    }
}
