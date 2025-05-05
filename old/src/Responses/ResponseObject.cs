using Microsoft.AspNetCore.Http;

namespace Responses
{
    public class ResponseObject<T>
    {
        public int Status { get; set; }
        public string Message { get; set; } = string.Empty;
        public DateTime ResponseDate { get; set; }
        public T? Data { get; set; }

        public ResponseObject() { }

        public ResponseObject(int status, string message, DateTime responseDate, T data)
        {
            Status = status;
            Message = message;
            ResponseDate = responseDate;
            Data = data;
        }
        public ResponseObject(int status, string message, DateTime responseDate)
        {
            Status = status;
            Message = message;
            ResponseDate = responseDate;
        }

        public static ResponseObject<T> ResponseSuccess(string message, T data)
        {
            return new ResponseObject<T>(StatusCodes.Status200OK, message, DateTime.Now, data);
        }
    }
}
