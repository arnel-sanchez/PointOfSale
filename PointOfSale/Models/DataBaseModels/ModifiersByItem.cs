using System.ComponentModel.DataAnnotations;

namespace PointOfSale.Models.DataBaseModels
{
    public class ModifiersByItem
    {
        [Required]
        [DataType(DataType.Text), StringLength(36)]
        public string Id { get; set; }

        [Required]
        public Item Item { get; set; }

        [Range(0, int.MaxValue)]
        public List<Modifier> Modifiers { get; set; }
    }
}
