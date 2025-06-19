namespace Application.Common.ExceptionHandling.Exceptions
{
    public class FriendlyException : Exception
    {
        public int StatusCode { get; }
        public object? Details { get; }

        public FriendlyException(string message, int statusCode = 500, object? details = null)
            : base(message)
        {
            StatusCode = statusCode;
            Details = details;
        }
    }
}
