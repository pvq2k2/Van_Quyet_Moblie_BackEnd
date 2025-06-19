namespace Application.Common.ExceptionHandling.Exceptions
{
    public class ValidationException : Exception
    {
        public IDictionary<string, string[]> Errors { get; }

        public int StatusCode { get; }

        public ValidationException(IDictionary<string, string[]> errors, string? message = "Dữ liệu không hợp lệ", int statusCode = 400)
            : base(message)
        {
            Errors = errors;
            StatusCode = statusCode;
        }
    }
}
