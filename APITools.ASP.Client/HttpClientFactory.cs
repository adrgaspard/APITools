using APITools.ASP.Base;
using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace APITools.ASP.Client
{
    /// <summary>
    /// The default implementation of IHttpClientFactory.
    /// </summary>
    public class HttpClientFactory : IHttpClientFactory
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="baseAPIAddress">The base address of the API targetted by the client</param>
        /// <exception cref="ArgumentNullException">Occurs when <paramref name="baseAPIAddress"/> is null</exception>
        public HttpClientFactory(string baseAPIAddress)
        {
            if (baseAPIAddress is null)
            {
                throw new ArgumentNullException(nameof(baseAPIAddress));
            }
            BaseAPIAddress = baseAPIAddress;
        }

        /// <summary>
        /// Gets or sets the base address of the API targetted by the client.
        /// </summary>
        public string BaseAPIAddress { get; protected set; }

        /// <inheritdoc cref="IHttpClientFactory.BuildHttpClient"/>
        public HttpClient BuildHttpClient()
        {
            HttpClient client = new();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(HTTPConstants.JsonApplicationTypeHeader));
            client.BaseAddress = new(BaseAPIAddress);
            return client;
        }
    }
}