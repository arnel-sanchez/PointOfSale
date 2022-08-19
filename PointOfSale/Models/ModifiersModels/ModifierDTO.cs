using System.ComponentModel.DataAnnotations;

namespace PointOfSale.Models.ModifiersModels
{
    public class ModifierDTO
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
        public bool Add { get; set; }
    }
}
