namespace PointOfSaleClient.Models
{
    public class Login
    {
        public Login()
        {
            username = "";
            password = "";
            rememberMe = false;
        }
        
        public string username { get; set; }

        public string password { get; set; }

        public bool rememberMe { get; set; }
    }
}
