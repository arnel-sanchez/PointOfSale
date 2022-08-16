using System.ComponentModel.DataAnnotations;

namespace PointOfSale.Models.SalesModels
{
    public class ModifiersByItemsDTO
    {
        [Required]
        public string ItemId { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public List<string> ModifiersId { get; set; }
    }
}
