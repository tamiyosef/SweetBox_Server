using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AppServer.Models;

[Index("Email", Name = "UQ__Users__A9D10534E17FDD2E", IsUnique = true)]
public partial class User
{
    [Key]
    public int UserId { get; set; }

    [StringLength(100)]
    public string FullName { get; set; } = null!;

    [StringLength(100)]
    public string Email { get; set; } = null!;

    [StringLength(100)]
    public string Password { get; set; } = null!;

    [StringLength(10)]
    public string UserType { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime? DateCreated { get; set; }

    [InverseProperty("User")]
    public virtual Admin? Admin { get; set; }

    [InverseProperty("BuyerNavigation")]
    public virtual Buyer? Buyer { get; set; }

    [InverseProperty("SellerNavigation")]
    public virtual Seller? Seller { get; set; }
}
