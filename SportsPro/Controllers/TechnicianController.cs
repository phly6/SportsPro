using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportsPro.Models;

namespace SportsPro.Controllers
{
    public class TechnicianController : Controller
    {
        public IActionResult GetTechnician()
        {
            var technicians = _context.Technicians.ToList();
            return View(technicians);
        }

        [HttpPost]
        public IActionResult SelectTechnician(int? technicianId)
        {
            if (technicianId == null || technicianId == 0)
            {
                TempData["ErrorMessage"] = "Please select a technician.";
                return RedirectToAction("GetTechnician");
            }
            HttpContext.Session.SetInt32("TechnicianID", technicianId.Value);
            return RedirectToAction("IncidentsByTechnician", "Incident");
        }
        private readonly SportsProContext _context;

        public TechnicianController(SportsProContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> List()
        {
            var technicians = await _context.Technicians.ToListAsync();
            return View(technicians);
        }

        public async Task<IActionResult> TechnicianForm(int? id)
        {
            if (id == null || id == 0) // Create Mode
            {
                return View(new Technician()); // Return an empty Technician object
            }

            var technician = await _context.Technicians.FindAsync(id);
            if (technician == null)
            {
                return NotFound();
            }

            return View(technician); // Edit Mode
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(Technician technician)
        {
            if (!ModelState.IsValid)
            {
                return View("TechnicianForm", technician);
            }

            if (technician.TechnicianID == 0) // New Technician
            {
                _context.Technicians.Add(technician);
            }
            else // Existing Technician
            {
                _context.Technicians.Update(technician);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(List));
        }

        // GET: Technician/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var technician = await _context.Technicians
                .FirstOrDefaultAsync(m => m.TechnicianID == id);
            if (technician == null)
            {
                return NotFound();
            }

            return View(technician);
        }

        // POST: Technician/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var technician = await _context.Technicians.FindAsync(id);
            if (technician != null)
            {
                _context.Technicians.Remove(technician);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(List));
        }

        private bool TechnicianExists(int id)
        {
            return _context.Technicians.Any(e => e.TechnicianID == id);
        }
    }
}
