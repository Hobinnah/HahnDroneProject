using System;
using System.Net;


namespace HahnDroneAPI.CustomExceptions
{
    public class MessageException : Exception
    {

        public HttpStatusCode HttpStatusCode { get; private set; }
        public MessageException(string message, HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest, Exception innerException = null) : base(message, innerException)
        {
            HttpStatusCode = httpStatusCode;
        }

    }
}
