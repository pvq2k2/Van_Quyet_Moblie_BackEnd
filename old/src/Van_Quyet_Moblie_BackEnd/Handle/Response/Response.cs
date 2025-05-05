namespace Van_Quyet_Moblie_BackEnd.Handle.Response
{
    public class Response
    {
        public int Status { get; set; }
        public string Message { get; set; } = string.Empty;
        public DateTime ResponseDate { get; set; }

        public Response() { }

        public Response(int status, string message, DateTime responseDate)
        {
            Status = status;
            Message = message;
            ResponseDate = responseDate;
        }

        public Response ResponseSuccess(string message)
        {
            return new Response(StatusCodes.Status200OK, message, DateTime.Now);
        }
    }
}
