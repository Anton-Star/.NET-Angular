namespace FactWeb.Mvc.Models
{
    public class ServiceResponse
    {
        public bool HasError { get; set; }
        public string Message { get; set; }
    }

    public class ServiceResponse<T>
    {
        public bool HasError { get; set; }
        public string Message { get; set; }
        public T Item { get; set; }
    }
}