using Case8.Infrastructure;
using Case8.Web.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Case8.Web.Areas.Admin.Controllers;

[Area("Admin")]
public class ActivityController(StoreDbContext db) : Controller
{
    public async Task<IActionResult> Index()
    {
        var notifications = await db.AdminNotifications.AsNoTracking().OrderByDescending(x => x.CreatedAt).Take(50).ToListAsync();
        var auditLogs = await db.AuditLogs.AsNoTracking().OrderByDescending(x => x.CreatedAt).Take(50).ToListAsync();
        return View(new ActivityViewModel(notifications, auditLogs));
    }
}
