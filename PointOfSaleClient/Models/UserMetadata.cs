namespace PointOfSaleClient.Models
{
    public class UserMetadata
    {
        public string username { get; set; }

        public string role { get; set; }

        public string accessToken { get; set; }
        
        public string refreshToken { get; set; }
    }
}
