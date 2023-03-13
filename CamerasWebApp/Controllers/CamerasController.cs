using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CamerasWebApp.Data;
using CamerasWebApp.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using InfiNickyCodes;

namespace CamerasWebApp.Controllers
{
    public class CamerasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CamerasController(ApplicationDbContext context)
        {
            _context = context;
            string csv_file_path = "wwwroot/csv/cameras-defb.csv";
            CSVDataHandler dataHandler = new(csv_file_path);
            foreach (var cam in dataHandler.CreateCamerasFromCSV())
            {
                Models.Camera camModel = new Models.Camera();
                camModel.Id = cam.Number;
                camModel.Number = cam.Number;
                camModel.Name = cam.Name;
                camModel.Longitude = cam.Longitude;
                camModel.Latitude = cam.Latitude;
                _context.Camera.Add(camModel);

            }
        }

        // GET: Cameras
        public async Task<IActionResult> Index()
        {
              return View(await _context.Camera.ToListAsync());
        }

        // GET: Cameras/SearchForm
        public async Task<IActionResult> SearchForm()
        {
              return View();
        }

        // POST: Cameras/SearchResults
        public async Task<IActionResult> SearchResults(string partialName)
        {
            ViewResult results;
            List<Models.Camera> cameras = await _context.Camera.Where(cam => cam.Name.Contains(partialName)).ToListAsync();
            if (cameras.Count > 0)
            {
                results = View("Index", cameras);
            }
            else
            {
                results = View("SearchForm");
            }
            return results;
        }

        // GET: Cameras/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Camera == null)
            {
                return NotFound();
            }

            var camera = await _context.Camera
                .FirstOrDefaultAsync(m => m.Id == id);
            if (camera == null)
            {
                return NotFound();
            }

            return View(camera);
        }

        // GET: Cameras/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Cameras/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Number,Name,Latitude,Longitude")] Models.Camera camera)
        {
            if (ModelState.IsValid)
            {
                _context.Add(camera);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(camera);
        }

        // GET: Cameras/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Camera == null)
            {
                return NotFound();
            }

            var camera = await _context.Camera.FindAsync(id);
            if (camera == null)
            {
                return NotFound();
            }
            return View(camera);
        }

        // POST: Cameras/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Number,Name,Latitude,Longitude")] Models.Camera camera)
        {
            if (id != camera.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(camera);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CameraExists(camera.Id))
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
            return View(camera);
        }

        // GET: Cameras/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Camera == null)
            {
                return NotFound();
            }

            var camera = await _context.Camera
                .FirstOrDefaultAsync(m => m.Id == id);
            if (camera == null)
            {
                return NotFound();
            }

            return View(camera);
        }

        // POST: Cameras/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Camera == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Camera'  is null.");
            }
            var camera = await _context.Camera.FindAsync(id);
            if (camera != null)
            {
                _context.Camera.Remove(camera);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CameraExists(int id)
        {
          return _context.Camera.Any(e => e.Id == id);
        }
    }
}
