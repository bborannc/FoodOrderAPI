using System;
using System.Collections.Generic;
using System.Text;

public class OrderItemDto
{
    public int MenuItemId { get; set; }
    public string MenuItemName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalItemPrice => Quantity * UnitPrice;
}
