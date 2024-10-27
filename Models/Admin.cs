using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AppServer.Models;

public partial class Admin
{
    [Key]
    public int UserId { get; set; }

    public int AdminLevel { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("Admin")]
    public virtual User User { get; set; } = null!;
}
