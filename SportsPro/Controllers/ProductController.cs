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


        // GET: Product
        public async Task<IActionResult> Index()
        {
            var products = await _context.Products.ToListAsync();
            return View(products);
            // return View(await _context.Products.ToListAsync());
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

        // GET: Product/Create
        [HttpGet]
        [Route("/Product/Create")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Product/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductID,ProductCode,Name,YearlyPrice,ReleaseDate")] Product product)
        {
            if (ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine("Validation Error: " + error.ErrorMessage);
                }
                _context.Add(product);
                await _context.SaveChangesAsync();

                // Debugging: Confirm this line executes
                Console.WriteLine("Before setting TempData");
                TempData["SuccessMessage"] = "Product added successfully.";
                Console.WriteLine("TempData Set: " + TempData["SuccessMessage"]);

                return RedirectToAction(nameof(List)); // ✅ Redirect ensures TempData persists
            }

            Console.WriteLine("ModelState Invalid: TempData was not set");
            return View(product); // ❌ If we return View(), TempData will be cleared
        }
        // GET: Product/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int? ProductID)
        {
            if (ProductID == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(ProductID);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Product/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductID,ProductCode,Name,YearlyPrice,ReleaseDate")] Product product)
        {
            if (id != product.ProductID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Product updated successfully.";
                    Console.WriteLine("TempData Message Set: " + TempData["SuccessMessage"]); // Debugging
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductID))
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
