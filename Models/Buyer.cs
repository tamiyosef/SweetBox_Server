using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AppServer.Models;

public partial class Buyer
{
    [Key]
    public int BuyerId { get; set; }

    [StringLength(255)]
    public string? ShippingAddress { get; set; }

    [StringLength(20)]
    public string? PhoneNumber { get; set; }

    [ForeignKey("BuyerId")]
    [InverseProperty("Buyer")]
    public virtual User BuyerNavigation { get; set; } = null!;

    [InverseProperty("Buyer")]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
