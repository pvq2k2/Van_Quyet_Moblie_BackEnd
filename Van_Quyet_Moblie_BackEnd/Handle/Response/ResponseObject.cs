
namespace Van_Quyet_Moblie_BackEnd.Handle.Response
{
    public class ResponseObject<T>
    {
        public int Status { get; set; }
        public string Message { get; set; } = string.Empty;
        public DateTime ResponseDate { get; set; }
        public T? Data { get; set; }

        public ResponseObject() {}

        public ResponseObject(int status, string message, DateTime responseDate, T data)
        {
            Status = status;
            Message = message;
            ResponseDate = responseDate;
            Data = data;
        }

        public ResponseObject<T> ResponseSuccess(string message, T data)
        {
            return new ResponseObject<T>(StatusCodes.Status200OK, message, DateTime.Now, data);
        }

        public ResponseObject<T> ResponseError(int statusCode, string message, T data)
        {
            return new ResponseObject<T>(statusCode, message, DateTime.Now, data);
        }
    }
}
