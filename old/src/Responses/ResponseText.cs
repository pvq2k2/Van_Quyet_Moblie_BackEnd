using Microsoft.AspNetCore.Http;

namespace Responses
{
    public class ResponseText
    {
        public int Status { get; set; }
        public string Message { get; set; } = string.Empty;
        public DateTime ResponseDate { get; set; }

        public ResponseText() { }

        public ResponseText(int status, string message, DateTime responseDate)
        {
            Status = status;
            Message = message;
            ResponseDate = responseDate;
        }

        public static ResponseText ResponseSuccess(string message)
        {
            return new ResponseText(StatusCodes.Status200OK, message, DateTime.Now);
        }
    }
}
