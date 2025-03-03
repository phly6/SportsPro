using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SportsPro.Models;

namespace SportsPro._Controllers
{
    public class IncidentController : Controller
    {
        private readonly SportsProContext _context;

        public IncidentController(SportsProContext context)
        {
            _context = context;
        }

        public IActionResult IncidentForm(int? id)
        {
            ViewBag.Customers = _context.Customers
                .Select(c => new SelectListItem
                {
                    Value = c.CustomerID.ToString(),
                    Text = c.FirstName + " " + c.LastName
                }).ToList();

            ViewBag.Products = _context.Products
                .Select(p => new SelectListItem
                {
                    Value = p.ProductID.ToString(),
                    Text = p.Name
                }).ToList();

            ViewBag.Technicians = _context.Technicians
                .Select(t => new SelectListItem
                {
                    Value = t.TechnicianID.ToString(),
                    Text = t.Name
                }).ToList();

            if (id == null || id == 0) // Create Mode
            {
                return View(new Incident { DateOpened = DateTime.Now });
            }

            var incident = _context.Incidents.Find(id);
            if (incident == null)
            {
                return NotFound();
            }

            return View(incident);
        }

        // GET: Incident
        public async Task<IActionResult> Index()
        {
            var sportsProContext = _context.Incidents
                .Include(i => i.Customer)
                .Include(i => i.Product)
                .Include(i => i.Technician);
            return View(await sportsProContext.ToListAsync());
        }

        // GET: Incident/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var incident = await _context.Incidents
                .Include(i => i.Customer)
                .Include(i => i.Product)
                .Include(i => i.Technician)
                .FirstOrDefaultAsync(m => m.IncidentID == id);
            if (incident == null)
            {
                return NotFound();
            }

            return View(incident);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Save(Incident incident)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Customers = _context.Customers
                    .Select(c => new SelectListItem
                    {
                        Value = c.CustomerID.ToString(),
                        Text = c.FirstName + " " + c.LastName
                    }).ToList();

                ViewBag.Products = _context.Products
                    .Select(p => new SelectListItem
                    {
                        Value = p.ProductID.ToString(),
                        Text = p.Name
                    }).ToList();

                ViewBag.Technicians = _context.Technicians
                    .Select(t => new SelectListItem
                    {
                        Value = t.TechnicianID.ToString(),
                        Text = t.Name
                    }).ToList();

                return View("IncidentForm", incident);
            }

            if (incident.IncidentID == 0) // New Incident
            {
                _context.Incidents.Add(incident);
            }
            else // Existing Incident
            {
                _context.Update(incident);
            }

            _context.SaveChanges();
            return RedirectToAction(nameof(List));
        }

        // GET: Incident/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var incident = await _context.Incidents.FindAsync(id);
            if (incident == null)
            {
                return NotFound();
            }

            ViewData["CustomerID"] = new SelectList(_context.Customers, "CustomerID", "CustomerID", incident.CustomerID);
            ViewData["ProductID"] = new SelectList(_context.Products, "ProductID", "ProductID", incident.ProductID);
            ViewData["TechnicianID"] = new SelectList(_context.Technicians, "TechnicianID", "TechnicianID", incident.TechnicianID);
            return View(incident);
        }

        // POST: Incident/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IncidentID,Title,Description,DateOpened,DateClosed,CustomerID,ProductID,TechnicianID")] Incident incident)
        {
            if (id != incident.IncidentID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(incident);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IncidentExists(incident.IncidentID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(List));
            }

            ViewData["CustomerID"] = new SelectList(_context.Customers, "CustomerID", "CustomerID", incident.CustomerID);
            ViewData["ProductID"] = new SelectList(_context.Products, "ProductID", "ProductID", incident.ProductID);
            ViewData["TechnicianID"] = new SelectList(_context.Technicians, "TechnicianID", "TechnicianID", incident.TechnicianID);
            return View(incident);
        }

        // GET: Incident/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var incident = await _context.Incidents
                .Include(i => i.Customer)
                .Include(i => i.Product)
                .Include(i => i.Technician)
                .FirstOrDefaultAsync(m => m.IncidentID == id);
            if (incident == null)
            {
                return NotFound();
            }

            return View(incident);
        }

        // POST: Incident/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var incident = await _context.Incidents.FindAsync(id);
            if (incident != null)
            {
                _context.Incidents.Remove(incident);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(List));
        }

        public async Task<IActionResult> List()
        {
            var incidents = await _context.Incidents.ToListAsync();
            return View(incidents);
        }

        private bool IncidentExists(int id)
        {
            return _context.Incidents.Any(e => e.IncidentID == id);
        }
    }
}
