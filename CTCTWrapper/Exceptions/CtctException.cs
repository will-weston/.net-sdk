using CTCT.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace CTCT.Exceptions
{
    /// <summary>
    /// General exception.
    /// </summary>
    public class CtctException : Exception
    {
        public HttpStatusCode StatusCode { get; set; }

        public CUrlRequestError[] Errors { get; set; }

        /// <summary>
        /// Class constructor.
        /// </summary>
        public CtctException() 
        { 
        }

        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <param name="message">Error message.</param>
        public CtctException(string message, CUrlRequestError[] errors = null, HttpStatusCode code = HttpStatusCode.InternalServerError) 
            : base(message) 
        {
            Errors = errors;
            StatusCode = code;
        }
    }
}
