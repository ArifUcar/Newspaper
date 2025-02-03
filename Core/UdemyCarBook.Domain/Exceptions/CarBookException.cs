namespace UdemyCarBook.Domain.Exceptions
{
    public class AuFrameWorkException : Exception
    {
        public string ErrorCode { get; }
        public string ErrorType { get; }

        public AuFrameWorkException(string message) : base(message)
        {
            ErrorType = "GeneralError";
        }

        public AuFrameWorkException(string message, string errorCode) : base(message)
        {
            ErrorCode = errorCode;
            ErrorType = "BusinessError";
        }

        public AuFrameWorkException(string message, string errorCode, string errorType) : base(message)
        {
            ErrorCode = errorCode;
            ErrorType = errorType;
        }
    }
} 