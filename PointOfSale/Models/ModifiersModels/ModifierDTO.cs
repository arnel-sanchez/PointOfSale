using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace PointOfSale.Models.ModifiersModels
{
    public class ModifierDTO
    {
        public ModifierDTO(string name, string description, double price, bool add)
        {
            Name = name;
            Description = description;
            Price = price;
            Add = add;
        }

        [Required]
        [DataType(DataType.Text), StringLength(20)]
        public readonly string Name;

        [Required]
        [DataType(DataType.Text), StringLength(255)]
        public readonly string Description;

        [Required]
        [Precision(2)]
        public readonly double Price;

        [Required]
        public readonly bool Add;
    }
}
