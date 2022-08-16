using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace PointOfSale.Models.SalesModels
{
    public class SaleDTO
    {
        [Required]
        public string SellerID { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public List<ModifiersByItemsDTO> Items { get; set; }
    }
}
