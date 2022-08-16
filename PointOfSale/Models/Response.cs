namespace PointOfSale.Models
{
    public class Response<T>
    {
        public string Message { get; set; }

        public int Code { get; set; }

        public T Data { get; set; }
    }
}
