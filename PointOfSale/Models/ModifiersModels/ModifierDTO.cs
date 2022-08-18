using System.ComponentModel.DataAnnotations;

namespace PointOfSale.Models.ModifiersModels
{
    public class ModifierDTO
    {
        [Required]
        [DataType(DataType.Text), StringLength(20)]
        public string Name;

        [Required]
        [DataType(DataType.Text), StringLength(255)]
        public string Description;

        [Required]
        public double Price;

        [Required]
        public bool Add;
    }
}
