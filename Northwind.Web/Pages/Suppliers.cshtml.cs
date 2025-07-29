using Microsoft.AspNetCore.Mvc; // To use [BindProperty], IActionResult
using Microsoft.AspNetCore.Mvc.RazorPages;
using Northwind.EntityModels;

namespace Northwind.Web.Pages
{
    public class SuppliersModel : PageModel
    {
        private readonly NorthwindContext _db;
        public SuppliersModel(NorthwindContext db)
        {
            _db = db;
        }
        public IEnumerable<Supplier>? Suppliers { get; set; }
        public void OnGet()
        {
            ViewData["Title"] = "Northwind B2B - Suppliers";
            Suppliers = _db.Suppliers
                .OrderBy(c => c.Country)
                .ThenBy(c => c.CompanyName);
        }
        [BindProperty]
        public Supplier? Supplier { get; set; }
        public IActionResult OnPost()
        {
            if (Supplier is not null && ModelState.IsValid)
            {
                _db.Suppliers.Add(Supplier);
                _db.SaveChanges();
                return RedirectToPage("/suppliers");
            }
            else
            {
                return Page();
            }
        }
    }
}
