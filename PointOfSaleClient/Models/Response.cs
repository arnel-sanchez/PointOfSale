namespace PointOfSaleClient.Models
{
    public class Response<T>
    {
        public string message { get; set; }

        public int code { get; set; }
        
        public T data { get; set; }
    }
}
