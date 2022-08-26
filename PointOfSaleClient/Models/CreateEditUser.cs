namespace PointOfSaleClient.Models
{
    public class CreateEditUser
    {
        public string id { get; set; }

        public string name { get; set; }

        public string lastName { get; set; }

        public string email { get; set; }

        public string phoneNumber { get; set; }

        public string role { get; set; }

        public string userName { get; set; }

        public string password { get; set; }

        public string confirmPassword { get; set; }
    }
}
