using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AppServer.Models;

public partial class Product
{
    [Key]
    public int ProductId { get; set; }

    public int? SellerId { get; set; }

    [StringLength(100)]
    public string ProductName { get; set; } = null!;

    [StringLength(255)]
    public string? Description { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal Price { get; set; }

    [StringLength(255)]
    public string? ImageUrl { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DateAdded { get; set; }

    public bool IsAvailable { get; set; }

    [InverseProperty("Product")]
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    [ForeignKey("SellerId")]
    [InverseProperty("Products")]
    public virtual Seller? Seller { get; set; }
}
