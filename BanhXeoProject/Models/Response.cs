namespace BanhXeoProject.Models
{
    public class ResponseModel
    {
        public bool Status { get; set; }
    }

    public class SuccessResponseModel<T> : ResponseModel
    {
        public string? Message { get; set; }
        public T? Data { get; set; }
    }

    public class ErrorResponseModel : ResponseModel
    {
        public string? ErrorMessage { get; set; }
    }

    public class CombineResponseModel<T> : ResponseModel
    {
        public string? ErrorMessage { get; set; }
        public T? Data { get; set; }
    }
    public class ErrorModel
    {
        public string Message { get; set; } = string.Empty;
    }
}
