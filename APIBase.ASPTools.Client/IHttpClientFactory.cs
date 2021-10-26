using System.Net.Http;

namespace APIBase.ASPTools.Client
{
    /// <summary>
    /// Represents all classes that instanciate a HttpClient (it's recommended to have only one HttpClient).
    /// </summary>
    public interface IHttpClientFactory
    {
        /// <summary>
        /// Creates a new HttpClient.
        /// </summary>
        /// <returns>The new HttpClient</returns>
        HttpClient BuildHttpClient();
    }
}
