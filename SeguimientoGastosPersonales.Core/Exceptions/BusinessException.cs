using System;
using System.Collections.Generic;
using System.Text;

using System.Net;

namespace SeguimientoGastosPersonales.Core.Exceptions
{
    public class BusinessException : Exception
    {
        public HttpStatusCode StatusCode { get; }
        public object? Details { get; }

        public BusinessException(
            string message,
            HttpStatusCode statusCode = HttpStatusCode.BadRequest,
            object? details = null)
            : base(message)
        {
            StatusCode = statusCode;
            Details = details;
        }
    }
}
