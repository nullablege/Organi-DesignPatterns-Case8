using Case8.Domain;

namespace Case8.Web.Areas.Admin.Models;

public record ActivityViewModel(IReadOnlyList<AdminNotification> Notifications, IReadOnlyList<AuditLog> AuditLogs);
