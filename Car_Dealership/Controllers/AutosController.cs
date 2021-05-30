﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Car_Dealership.Data;
using Car_Dealership.Models;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Car_Dealership.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace Car_Dealership.Controllers
{
    public class AutosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;

        public AutosController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _userManager = userManager;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: Autos
        public async Task<IActionResult> Index()
        {
            return View(await _context.Autos.ToListAsync());
        }

        // GET: Autos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var auto = await _context.Autos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (auto == null)
            {
                return NotFound();
            }

            return View(auto);
        }

        // GET: Autos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Autos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /* [HttpPost]
         [ValidateAntiForgeryToken]

         public async Task<IActionResult> CreatePost(CreateAutoViewModel model)
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
                 Auto autos = new Auto
                 {
                     Brand = model.Title,
                     Photo = uniqueFileName,
                 };


                 _context.Autos.Add(autos);
                 _context.SaveChanges();
                 return RedirectToAction("Index", "Home");
             }

             return View();
         }*/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Brand,Engine,Body_Type,Start_Production,End_Production,Photo,Sets,Doors,Fuel_Consumption,Fuel_Type,Acceleration,Max_Speed,Power,Torque")] Auto auto, CreateAutoViewModel model)
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
                Auto autos = new Auto
                {
                    Brand = model.Title,
                    Photo = uniqueFileName,
                };


                var autoList = _context.Autos.Add(autos);
                _context.Add(autoList);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(auto);
        }


        // GET: Autos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var auto = await _context.Autos.FindAsync(id);
            if (auto == null)
            {
                return NotFound();
            }
            return View(auto);
        }

        // POST: Autos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Brand,Engine,Body_Type,Start_Production,End_Production,Photo,Sets,Doors,Fuel_Consumption,Fuel_Type,Acceleration,Max_Speed,Power,Torque")] Auto auto)
        {
            if (id != auto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(auto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AutoExists(auto.Id))
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
            return View(auto);
        }

        // GET: Autos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var auto = await _context.Autos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (auto == null)
            {
                return NotFound();
            }

            return View(auto);
        }

        // POST: Autos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var auto = await _context.Autos.FindAsync(id);
            _context.Autos.Remove(auto);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AutoExists(int id)
        {
            return _context.Autos.Any(e => e.Id == id);
        }
    }
}
