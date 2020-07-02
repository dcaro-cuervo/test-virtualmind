using System;
using Newtonsoft.Json;

namespace exchange.Helper
{
    public class ApiError
    {
        /// <summary>
        /// The integer of HTTP StatusCode;
        /// </summary>
        /// <value>The status code.</value>
        public int StatusCode { get; private set; }

        /// <summary>
        /// The string representation of the status code.
        /// </summary>
        /// <value>The status description.</value>
        public string StatusDescription { get; private set; }

        /// <summary>
        /// A Message that explains what type of error happened.
        /// </summary>
        /// <value>The message.</value>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Message { get; private set; }

        public ApiError(int statusCode, string statusDescription)
        {
            this.StatusCode = statusCode;
            this.StatusDescription = statusDescription;
        }

        public ApiError(int statusCode, string statusDescription, string message)
            : this(statusCode, statusDescription)
        {
            this.Message = message;
        }
    }
}
