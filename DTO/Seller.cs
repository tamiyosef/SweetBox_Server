using System.ComponentModel.DataAnnotations;

namespace AppServer.DTO
{
    public class Seller
    {
        public int SellerId { get; set; }

        public string BusinessName { get; set; } = null!;

        public string BusinessAddress { get; set; } = null!;

        public string? BusinessPhone { get; set; }

        public string? Description { get; set; }

        public string? ProfilePicture { get; set; }
    }
}
