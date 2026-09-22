using Case8.Domain;
using Case8.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Case8.Web.Areas.Admin.Controllers;

[Area("Admin")]
public class OrdersController(StoreDbContext db) : Controller
{
    public async Task<IActionResult> Index() => View(await db.Orders.Include(x => x.Items).AsNoTracking().OrderByDescending(x => x.CreatedAt).ToListAsync());

    public async Task<IActionResult> Details(int id)
    {
        var order = await db.Orders.Include(x => x.Items).SingleOrDefaultAsync(x => x.Id == id);
        if (order is null) return NotFound();
        return View(order);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Transition(int id, string command)
    {
        var order = await db.Orders.FindAsync(id);
        if (order is null) return NotFound();
        order.RestoreState();
        try
        {
            switch (command)
            {
                case "prepare": order.Prepare(); break;
                case "ship": order.Ship(); break;
                case "deliver": order.Deliver(); break;
                case "cancel": order.Cancel(); break;
                default: return BadRequest();
            }
            await db.SaveChangesAsync();
            TempData["Message"] = $"Order is now {order.Status}.";
        }
        catch (InvalidOperationException exception)
        {
            TempData["Error"] = exception.Message;
        }
        return RedirectToAction("Details", new { id });
    }
}
