using System.ComponentModel.DataAnnotations;

namespace PointOfSale.Models.SalesModels
{
    public class GetSalesDTO
    {
        [Required]
        public DateTime DateTime { get; set; }

        [DataType(DataType.Text), StringLength(36)]
        public string SellerId { get; set; }
    }
}
