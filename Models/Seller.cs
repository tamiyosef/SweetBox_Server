using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AppServer.Models;

public partial class Seller
{
    [Key]
    public int SellerId { get; set; }

    [StringLength(100)]
    public string BusinessName { get; set; } = null!;

    [StringLength(255)]
    public string BusinessAddress { get; set; } = null!;

    [StringLength(20)]
    public string? BusinessPhone { get; set; }

    [StringLength(255)]
    public string? Description { get; set; }

    [StringLength(255)]
    public string? ProfilePicture { get; set; }

    [InverseProperty("Seller")]
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    [ForeignKey("SellerId")]
    [InverseProperty("Seller")]
    public virtual User SellerNavigation { get; set; } = null!;
}
