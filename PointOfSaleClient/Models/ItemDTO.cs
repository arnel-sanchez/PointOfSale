namespace PointOfSaleClient.Models
{
    public class ItemDTO
    {
        public string name { get; set; }

        public string description { get; set; }

        public double price { get; set; }

        public int quantity { get; set; }

        public string category { get; set; }

        public string image { get; set; }

        public string code { get; set; }

        public string qrCode { get; set; }

        public List<string> modifiersId { get; set; }
    }
}
