namespace Van_Quyet_Moblie_BackEnd.Middleware
{
    public class CustomException : Exception
    {
        public int ErrorCode { get; set; }
        public string ErrorMessage { get; set; }

        public CustomException(int errorCode, string errorMessage)
        {
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
        }
    }

}
