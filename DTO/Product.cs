using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AppServer.DTO
{
    public class Product
    {
        public int ProductId { get; set; }

        public int? SellerId { get; set; }

        public string ProductName { get; set; } = null!;

        public string? Description { get; set; }

        public decimal Price { get; set; }

        public string? ImageUrl { get; set; }

        public DateTime? DateAdded { get; set; }

        public bool IsAvailable { get; set; }

    }
}
