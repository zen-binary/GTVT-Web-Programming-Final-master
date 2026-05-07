namespace Project_Final.ExceptionHandle
{
    public class BussinessException : Exception
    {
        public BussinessErrorCode ErrorCode {  get; }
        public string? Message { get; }

        public BussinessException(BussinessErrorCode errorCode, string message) : base(message) 
        {
            ErrorCode = errorCode;
            Message = message;
        }
    }
}
