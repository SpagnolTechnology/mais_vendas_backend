using System.Net;

namespace Crosscutting.CustomException
{
    public class CustomBusinessException : Exception
    {
        public HttpStatusCode? Status { get; private set; }

        public CustomBusinessException()
        {

        }

        public CustomBusinessException(string message, HttpStatusCode? status = HttpStatusCode.BadRequest) : base(message) => Status = status;
        public CustomBusinessException(string message, Exception innerException, HttpStatusCode? status = HttpStatusCode.BadRequest) : base(message, innerException) => Status = status;
    }
}
