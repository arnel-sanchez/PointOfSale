using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace PointOfSale.Models.ItemsModels
{
    public class ItemsDTO
    {
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

        public List<string> ModifiersId { get; set; }
    }
}
