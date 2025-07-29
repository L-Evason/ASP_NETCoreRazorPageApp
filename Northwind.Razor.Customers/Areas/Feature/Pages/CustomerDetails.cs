using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Northwind.EntityModels;

namespace Northwind.Razor.CustomerDetails;

public class CustomersDetail : PageModel
{
    private NorthwindContext _db;
    public CustomersDetail(NorthwindContext db)
    {
        _db = db;
    }
    [BindProperty(SupportsGet = true)]
    public string? Id { get; set; } = string.Empty;
    public Customer? Customer { get; set; }
    public IActionResult OnGet()
    {
        if (string.IsNullOrWhiteSpace(Id))
        {
            return NotFound();
        }
        Customer = _db.Customers
            .Include(c => c.Orders)
            .ThenInclude(o => o.OrderDetails)
            .ThenInclude(d => d.Product)
            .FirstOrDefault(c => c.CustomerId == Id);
        if (Customer == null)
        {
            return NotFound();
        }
        return Page();
    }
}