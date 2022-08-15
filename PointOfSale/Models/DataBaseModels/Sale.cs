using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace PointOfSale.Models.DataBaseModels
{
    public class Sale
    {
        [Required]
        [DataType(DataType.Text), StringLength(36)]
        public string Id { get; set; }

        public User Seller { get; set; }

        public List<Item> Items { get; set; }

        [Required]
        [Precision(2)]
        public decimal TotalPrice { get; set; }

        public List<Modifier> Modifiers { get; set; }
    }
}
