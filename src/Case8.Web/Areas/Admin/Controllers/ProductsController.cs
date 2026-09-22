using Case8.Domain;
using Case8.Infrastructure;
using Case8.Web.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Case8.Web.Areas.Admin.Controllers;

[Area("Admin")]
public class ProductsController(StoreDbContext db) : Controller
{
    public async Task<IActionResult> Index() => View(await db.Products.Include(x => x.Category).AsNoTracking().OrderBy(x => x.Name).ToListAsync());
    public async Task<IActionResult> Create() { await Categories(); return View(new ProductFormViewModel { IsActive = true }); }
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProductFormViewModel form)
    {
        if (!ModelState.IsValid) { await Categories(); return View(form); }
        db.Products.Add(new Product
        {
            Name = form.Name,
            Description = form.Description,
            Price = form.Price,
            Stock = form.Stock,
            IsActive = form.IsActive,
            ImageUrl = form.ImageUrl,
            CategoryId = form.CategoryId
        });
        await db.SaveChangesAsync(); return RedirectToAction("Index");
    }
    public async Task<IActionResult> Edit(int id)
    {
        var product = await db.Products.FindAsync(id);
        if (product is null) return NotFound();
        await Categories();
        return View(new ProductFormViewModel
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Stock = product.Stock,
            IsActive = product.IsActive,
            ImageUrl = product.ImageUrl,
            CategoryId = product.CategoryId
        });
    }
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(ProductFormViewModel form)
    {
        if (!ModelState.IsValid) { await Categories(); return View(form); }
        var current = await db.Products.FindAsync(form.Id);
        if (current is null) return NotFound();
        current.Name = form.Name;
        current.Description = form.Description;
        current.Price = form.Price;
        current.Stock = form.Stock;
        current.IsActive = form.IsActive;
        current.ImageUrl = form.ImageUrl;
        current.CategoryId = form.CategoryId;
        await db.SaveChangesAsync(); return RedirectToAction("Index");
    }
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var product = await db.Products.FindAsync(id); if (product is not null) { db.Products.Remove(product); await db.SaveChangesAsync(); }
        return RedirectToAction("Index");
    }
    private async Task Categories() => ViewBag.Categories = new SelectList(await db.Categories.OrderBy(x => x.Name).ToListAsync(), "Id", "Name");
}
