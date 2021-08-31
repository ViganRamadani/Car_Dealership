using System;
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
using Microsoft.AspNetCore.Authorization;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Drawing;
using System.IO;
using Syncfusion.Pdf.Grid;
using System.Data;
using ClosedXML.Excel;

namespace Car_Dealership.Controllers
{
    [Authorize(Roles = "Admin")]
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

        [AllowAnonymous]
        public async Task<IActionResult> Index(string autoName, string? sortByPrice)
        {
            var allAutos = await _context.Autos.ToListAsync();
            var autos = new List<Auto>();
            if (!String.IsNullOrEmpty(sortByPrice))
            {
                if (sortByPrice.Equals("lowest"))
                {
                    allAutos = allAutos.OrderBy(x => x.Price).ToList();

                } else if (sortByPrice.Equals("highest"))
                {
                    allAutos = allAutos.OrderByDescending(x => x.Price).ToList();
                }

            }
            if (autoName != null)
            {
                foreach (var a in allAutos)
                {
                    if (a.Brand.ToLower().Contains(autoName.ToLower()))
                    {
                        autos.Add(a);
                    }
                }
                return View(autos);

            }
            else
            {
                return View(allAutos);
            }


        }


        // GET: Autos/Details/5
        [AllowAnonymous]
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
        [HttpPost]
        public IActionResult Export()
        {
            DataTable dt = new DataTable("Grid");
            dt.Columns.AddRange(new DataColumn[] { new DataColumn("Price"),
                                        new DataColumn("Brand"),
                                        new DataColumn("Auto_Production_Year"),
                                        new DataColumn("Engine") ,
                                        new DataColumn("Body_Type") ,
                                        new DataColumn("Start_Production") ,
                                        new DataColumn("End_Production") ,
                                        new DataColumn("Sets") ,
                                        new DataColumn("Doors") ,
                                        new DataColumn("Acceleration") });

            var customers = from customer in this._context.Autos.Take(10)
                            select customer;

            foreach (var a in _context.Autos)
            {
                dt.Rows.Add(a.Price, a.Brand, a.Auto_Production_Year, a.Engine ,a.Body_Type, a.Start_Production,
                            a.End_Production, a.Sets, a.Doors, a.Acceleration);
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Grid.xlsx");
                }
            }
        }


        [AllowAnonymous]
        public IActionResult AddToFavorite(int id)
        {
            var user = _userManager.GetUserAsync(HttpContext.User);
            Favorite favorites = new Favorite();
            favorites.UserId = user.Result.Id;
            favorites.Auto_Id = id;
            _context.Favorites.Add(favorites);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        [AllowAnonymous]
        public IActionResult DisplayFavorites()
        {
            var user = _userManager.GetUserAsync(HttpContext.User);
    
            var results = _context.Favorites.Where(car => car.UserId.Equals(user.Result.Id)).ToList();
            var cars = new List<Auto>();
            
            foreach (var result in results)
            {
                Auto temp = _context.Autos.Where(car => car.Id.Equals(result.Auto_Id)).FirstOrDefault();
                cars.Add(temp);
            }
            return View(cars);
        }
        [AllowAnonymous]
        public IActionResult DeleteFavorite(int id)
        {
            var user = _userManager.GetUserAsync(HttpContext.User);

            var results = _context.Favorites.Where(car => car.Auto_Id.Equals(id) && car.UserId.Equals(user.Result.Id)).First();
            _context.Favorites.Remove(results);
            _context.SaveChanges();
            return RedirectToAction(nameof(DisplayFavorites));
            /* var cars = new List<Auto>();*/

            /*foreach (var result in results)
            {
                Favorite temp = _context.Favorites.Where(car => car.Auto_Id.Equals(result.Auto_Id)).FirstOrDefault();
                _context.Favorites.Remove(temp);
                _context.SaveChanges();
                return RedirectToAction(nameof(DisplayFavorites));
            }*/
            /*return View();*/
        }
        // GET: Autos/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateAutoViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = UploadedFile(model);

                Auto autos = new Auto
                {
                    Brand = model.Brand,
                    Engine = model.Engine,
                    EngineSize = model.EngineSize,
                    Auto_Production_Year = model.Auto_Production_Year,
                    Start_Production = model.Start_Production,
                    End_Production = model.End_Production,
                    Doors = model.Doors,
                    Fuel_Consumption = model.Fuel_Consumption,
                    Sets = model.Sets,
                    Photo = uniqueFileName,
                    Price = model.Price,
                    Fuel_Type = model.Fuel_Type,
                    Body_Type = model.Body_Type,
                    Acceleration = model.Acceleration,
                    Max_Speed = model.Max_Speed,
                    Power = model.Power,
                    Torque = model.Torque,
                    IsDogan = model.IsDogan


                };
                _context.Add(autos);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();

        }
        private string UploadedFile(CreateAutoViewModel model)
        {
            string uniqueFileName = null;

            if (model.Photo != null)
            {
                string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "img");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.Photo.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,Brand,Engine,Body_Type,Start_Production,End_Production,Photo,Sets,Doors,Fuel_Consumption,Fuel_Type,Acceleration,Max_Speed,Power,Torque,Price,IsDogan")] Auto auto)
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
