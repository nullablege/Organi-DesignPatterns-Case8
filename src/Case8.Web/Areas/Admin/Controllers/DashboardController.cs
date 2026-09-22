using Case8.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Case8.Web.Areas.Admin.Controllers;

[Area("Admin")]
public class DashboardController(StoreDbContext db) : Controller
{
    public async Task<IActionResult> Index() => View(await db.Products.AsNoTracking().OrderBy(x => x.Name).ToListAsync());
}
