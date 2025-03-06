using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SportsPro.Models;
using SportsPro.ViewModels;

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
            var viewModel = new IncidentFormViewModel
            {
                Customers = _context.Customers.ToList(),
                Products = _context.Products.ToList(),
                Technicians = _context.Technicians.ToList(),
                FormMode = id == null || id == 0 ? "Add" : "Edit"
            };

            if (id == null || id == 0)
            {
                viewModel.Incident = new Incident { DateOpened = DateTime.Now };
            }
            else
            {
                var incident = _context.Incidents.Find(id);
                if (incident == null)
                {
                    return NotFound();
                }
                viewModel.Incident = incident;
            }

            return View(viewModel);
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

        public async Task<IActionResult> List(string filter = "All")
        {
            var incidents = await _context.Incidents
                .Include(i => i.Customer)
                .Include(i => i.Product)
                .Include(i => i.Technician)
                .ToListAsync();

            var viewModel = new IncidentManagerViewModel
            {
                Incidents = incidents,
                Filter = filter
            };

            return View(viewModel);
        }

        private bool IncidentExists(int id)
        {
            return _context.Incidents.Any(e => e.IncidentID == id);
        }
    }
}
