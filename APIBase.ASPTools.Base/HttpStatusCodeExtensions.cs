using System.Net;

namespace APIBase.ASPTools.Base
{
    /// <summary>
    /// Represents a set of extension methods for the HttpStatusCode enumeration.
    /// </summary>
    /// <see cref="HttpStatusCode"/>
    public static class HttpStatusCodeExtensions
    {
        /// <summary>
        /// Gets the int value of a http status code.
        /// </summary>
        /// <param name="source">The http status code</param>
        /// <returns>The corresponding int value</returns>
        public static int ToInt(this HttpStatusCode source)
        {
            return (int)source;
        }

        /// <summary>
        /// Checks if a http status code is in the information category.
        /// </summary>
        /// <param name="source">The http status code to be checked</param>
        /// <returns>A value that indicates whether the http status code is in the information category</returns>
        public static bool IsInformation(this HttpStatusCode source)
        {
            int code = source.ToInt();
            return code >= 100 && code <= 199;
        }

        /// <summary>
        /// Checks if a http status code is in the success category.
        /// </summary>
        /// <param name="source">The http status code to be checked</param>
        /// <returns>A value that indicates whether the http status code is in the success category</returns>
        public static bool IsSuccess(this HttpStatusCode source)
        {
            int code = source.ToInt();
            return code >= 200 && code <= 299;
        }

        /// <summary>
        /// Checks if a http status code is in the redirection category.
        /// </summary>
        /// <param name="source">The http status code to be checked</param>
        /// <returns>A value that indicates whether the http status code is in the redicrection category</returns>
        public static bool IsRedirection(this HttpStatusCode source)
        {
            int code = source.ToInt();
            return code >= 300 && code <= 399;
        }

        /// <summary>
        /// Checks if a http status code is in the client error category.
        /// </summary>
        /// <param name="source">The http status code to be checked</param>
        /// <returns>A value that indicates whether the http status code is in the client error category</returns>
        public static bool IsClientError(this HttpStatusCode source)
        {
            int code = source.ToInt();
            return code >= 400 && code <= 499;
        }

        /// <summary>
        /// Checks if a http status code is in the server error category.
        /// </summary>
        /// <param name="source">The http status code to be checked</param>
        /// <returns>A value that indicates whether the http status code is in the server error category</returns>
        public static bool IsServerError(this HttpStatusCode source)
        {
            int code = source.ToInt();
            return code >= 500 && code <= 599;
        }

        /// <summary>
        /// Checks if a http status code is a error code.
        /// </summary>
        /// <param name="source">The http status code to be checked</param>
        /// <returns>A value that indicates whether the http status code is in the client error or the server error category</returns>
        public static bool IsError(this HttpStatusCode source)
        {
            int code = source.ToInt();
            return code >= 400 && code <= 599;
        }
    }
}