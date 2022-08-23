using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace PointOfSale.Models.DataBaseModels
{
    public class Item
    {
        [Required]
        [DataType(DataType.Text), StringLength(36)]
        public string Id { get; set; }

        [Required]
        [DataType(DataType.Text), StringLength(20)]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Text), StringLength(255)]
        public string Description { get; set; }

        [Required]
        public double Price { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        [DataType(DataType.Text), StringLength(20)]
        public string Category { get; set; }

        [Required]
        public string Image { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        public string QRCode { get; set; }

        [Required]
        public bool IsDeleted { get; set; }

        public List<Modifier> Modifiers { get; set; }
    }
}
