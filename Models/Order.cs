using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AppServer.Models;

public partial class Order
{
    [Key]
    public int OrderId { get; set; }

    public int? BuyerId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? OrderDate { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal TotalAmount { get; set; }

    [StringLength(50)]
    public string OrderStatus { get; set; } = null!;

    [StringLength(255)]
    public string ShippingAddress { get; set; } = null!;

    [ForeignKey("BuyerId")]
    [InverseProperty("Orders")]
    public virtual Buyer? Buyer { get; set; }

    [InverseProperty("Order")]
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
