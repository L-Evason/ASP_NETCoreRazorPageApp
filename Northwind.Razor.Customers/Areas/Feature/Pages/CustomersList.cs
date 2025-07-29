using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Northwind.EntityModels;

namespace Northwind.Razor.Customers;

public class CustomersList : PageModel
{
    private NorthwindContext _db;
    public CustomersList(NorthwindContext db)
    {
        _db = db;
    }
    public Customer[] Customers { get; set; } = null!;
    public void OnGet()
    {
        ViewData["Title"] = "Northwind B2B - Customers";
        Customers = _db.Customers
            .OrderBy(c => c.Country)
            .ToArray();
    }
}
