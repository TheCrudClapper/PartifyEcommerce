namespace CSOS.UI.Helpers
{
    public class JsonResponseModel<T> : JsonResponseModel
    {
        public T? Data { get; set; }
    }

    public class JsonResponseModel
    {
        public bool Success { get; set; }
        public string Message { get; set; } = null!;
    }
}
