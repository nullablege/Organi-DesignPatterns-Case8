using System.ComponentModel.DataAnnotations.Schema;
using Case8.Domain.Orders.States;

namespace Case8.Domain;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public ICollection<Product> Products { get; set; } = new List<Product>();
}

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public bool IsActive { get; set; }
    public string? ImageUrl { get; set; }
    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
}

public class Cart
{
    public int Id { get; set; }
    public string SessionKey { get; set; } = string.Empty;
    public ICollection<CartItem> Items { get; set; } = new List<CartItem>();
}

public class CartItem
{
    public int Id { get; set; }
    public int CartId { get; set; }
    public Cart Cart { get; set; } = null!;
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
    public int Quantity { get; set; }
}

public class Order
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Status { get; set; } = "Pending";
    public decimal Total { get; set; }
    public string ShippingMethod { get; set; } = string.Empty;
    public decimal ShippingCost { get; set; }
    public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
    [NotMapped]
    public IOrderState State { get; private set; } = new PendingOrderState();

    public void RestoreState()
    {
        State = Status switch
        {
            "Pending" => new PendingOrderState(),
            "Preparing" => new PreparingOrderState(),
            "Shipped" => new ShippedOrderState(),
            "Delivered" => new DeliveredOrderState(),
            "Cancelled" => new CancelledOrderState(),
            _ => throw new InvalidOperationException("Unknown order state.")
        };
    }

    public void Prepare() => ChangeState(State.Prepare());
    public void Ship() => ChangeState(State.Ship());
    public void Deliver() => ChangeState(State.Deliver());
    public void Cancel() => ChangeState(State.Cancel());

    private void ChangeState(IOrderState state)
    {
        State = state;
        Status = state.Name;
    }
}

public class OrderItem
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public Order Order { get; set; } = null!;
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
}

public class StockMovement
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int QuantityChange { get; set; }
    public string Reason { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public class AdminNotification
{
    public int Id { get; set; }
    public string Message { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public class AuditLog
{
    public int Id { get; set; }
    public string Action { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
