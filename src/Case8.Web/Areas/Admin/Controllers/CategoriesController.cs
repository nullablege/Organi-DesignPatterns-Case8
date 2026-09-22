using Case8.Domain;
using Case8.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Case8.Web.Areas.Admin.Controllers;

[Area("Admin")]
public class CategoriesController(StoreDbContext db) : Controller
{
    public async Task<IActionResult> Index() => View(await db.Categories.AsNoTracking().OrderBy(x => x.Name).ToListAsync());
    public IActionResult Create() => View(new Category());
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Category category)
    {
        if (!ModelState.IsValid) return View(category);
        db.Categories.Add(category); await db.SaveChangesAsync(); return RedirectToAction("Index");
    }
    public async Task<IActionResult> Edit(int id) => await db.Categories.FindAsync(id) is { } category ? View(category) : NotFound();
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Category category)
    {
        if (!ModelState.IsValid) return View(category);
        db.Categories.Update(category); await db.SaveChangesAsync(); return RedirectToAction("Index");
    }
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var category = await db.Categories.FindAsync(id); if (category is not null) { db.Categories.Remove(category); await db.SaveChangesAsync(); }
        return RedirectToAction("Index");
    }
}
