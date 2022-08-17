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

        public List<ModifiersByItem> Items { get; set; }

        [Required]
        public double TotalPrice { get; set; }

        [Required]
        public DateTime DateTime { get; set; }
    }
}
