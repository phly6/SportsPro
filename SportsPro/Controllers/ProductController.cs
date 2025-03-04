using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SportsPro.Models;

namespace SportsPro.Controllers
{
    public class ProductController : Controller
    {
        private readonly SportsProContext _context;

        public ProductController(SportsProContext context)
        {
            _context = context;
        }

        public IActionResult ProductForm(int? id)
        {
            if (id == null || id == 0) // Create Mode
            {
                return View(new Product { ReleaseDate = DateTime.Now }); // Prefill date
            }

            var product = _context.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Save(Product product)
        {
            if (!ModelState.IsValid)
            {
                return View("ProductForm", product);
            }

            if (product.ProductID == 0) // New Product
            {
                _context.Products.Add(product);
                TempData["SuccessMessage"] = "Product added successfully.";
            }
            else // Existing Product
            {
                _context.Products.Update(product);
                TempData["SuccessMessage"] = "Product updated successfully.";
            }

            _context.SaveChanges();
            return RedirectToAction("List");
        }

        // GET: Product/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.ProductID == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Product/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.ProductID == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Product deleted successfully.";
                Console.WriteLine("TempData Message Set: " + TempData["SuccessMessage"]); // Debugging
            }
            return RedirectToAction(nameof(List));
        }

        //list 
        [Route("/Product/all-products/")]
        public async Task<IActionResult> List()
        {
            var products = await _context.Products.ToListAsync();
            return View(products);
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductID == id);
        }
    }
}
