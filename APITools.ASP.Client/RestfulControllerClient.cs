using APITools.ASP.Base;
using APITools.Core.DAO.Models;
using APITools.Core.Ioc;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace APITools.ASP.Client
{
    /// <summary>
    /// Represents the base class for a web client with the methods provided by a REST client-side architecture.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity manipulated by the client</typeparam>
    public class RestfulControllerClient<TEntity> where TEntity : class, IGuidResolvable
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="automaticallySetHttpClientWithIoc">Whether the http client is to be automatically searched with the service locator or not</param>
        /// <param name="routesBaseAddress">The base address for the routes (in general cases, it's the name of the corresponding server controller)</param>
        /// <exception cref="ArgumentNullException">Occurs when <paramref name="routesBaseAddress"/> is null</exception>
        /// <exception cref="InvalidOperationException">Occurs when the http client is not registered in the service locator</exception>
        public RestfulControllerClient(bool automaticallySetHttpClientWithIoc, string routesBaseAddress)
        {
            if (routesBaseAddress is null)
            {
                throw new ArgumentNullException(nameof(routesBaseAddress));
            }
            if (automaticallySetHttpClientWithIoc)
            {
                HttpClient = ServiceLocator.Current.GetInstance<HttpClient>();
                if (HttpClient is null)
                {
                    throw new InvalidOperationException($"The {nameof(HttpClient)} can't be null in the ioc container");
                }
            }
            RoutesBaseAddress = routesBaseAddress;
        }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="httpClient">The provided http client</param>
        /// <param name="routesBaseAddress">The base address for the routes (in general cases, it's the name of the corresponding server controller)</param>
        /// <exception cref="ArgumentNullException">>Occurs when <paramref name="httpClient"/> or <paramref name="routesBaseAddress"/> is null</exception>
        public RestfulControllerClient(HttpClient httpClient, string routesBaseAddress) : this(false, routesBaseAddress)
        {
            if (httpClient is null)
            {
                throw new ArgumentNullException(nameof(httpClient));
            }
            HttpClient = httpClient;
        }

        /// <summary>
        /// Gets or sets the base address for the routes (in general cases, it's the name of the corresponding server controller).
        /// </summary>
        public string RoutesBaseAddress { get; protected init; }

        /// <summary>
        /// Gets or sets the http client used to communicate.
        /// </summary>
        protected HttpClient HttpClient { get; init; }

        /// <summary>
        /// Deletes one item.
        /// </summary>
        /// <param name="id">The id of the item to be deleted</param>
        /// <returns>The result code of the request</returns>
        public async Task<HttpStatusCode> DeleteOne(Guid id)
        {
            HttpResponseMessage response = await HttpClient.DeleteAsync($"{RoutesBaseAddress}/{id}");
            return response.StatusCode;
        }

        /// <summary>
        /// Gets all items.
        /// </summary>
        /// <returns>The result code of the request and a list of all items found</returns>
        public async Task<(HttpStatusCode, IEnumerable<TEntity>)> GetAll()
        {
            List<TEntity> entities = null;
            HttpResponseMessage response = await HttpClient.GetAsync($"{RoutesBaseAddress}");
            if (response.IsSuccessStatusCode)
            {
                entities = await response.Content.ReadFromJsonAsync<List<TEntity>>();
            }
            return (response.StatusCode, entities);
        }

        /// <summary>
        /// Gets one item.
        /// </summary>
        /// <param name="id">The id of the wanted item</param>
        /// <returns>The result code of the request and the wanted item</returns>
        public async Task<(HttpStatusCode, TEntity)> GetOne(Guid id)
        {
            TEntity entity = null;
            HttpResponseMessage response = await HttpClient.GetAsync($"{RoutesBaseAddress}/{id}");
            if (response.IsSuccessStatusCode)
            {
                entity = await response.Content.ReadFromJsonAsync<TEntity>();
            }
            return (response.StatusCode, entity);
        }

        /// <summary>
        /// Partially modifies an existing item.
        /// </summary>
        /// <param name="id">The id of the item to be edited</param>
        /// <param name="patchDocument">The patch document to apply to the item</param>
        /// <returns>The result code of the request and the modified item</returns>
        public async Task<(HttpStatusCode, TEntity)> PatchOne(Guid id, JsonPatchDocument<TEntity> patchDocument)
        {
            HttpResponseMessage response = await HttpClient.PatchAsync($"{RoutesBaseAddress}/{id}", JsonContent.Create(patchDocument));
            if (response.StatusCode.IsSuccess())
            {
                return (response.StatusCode, await response.Content.ReadFromJsonAsync<TEntity>());
            }
            return (response.StatusCode, null);
        }

        /// <summary>
        /// Creates a new item.
        /// </summary>
        /// <param name="entity">The new item</param>
        /// <returns>The result code of the request and its assigned id</returns>
        public async Task<(HttpStatusCode, Guid?)> PostOne(TEntity entity)
        {
            HttpResponseMessage response = await HttpClient.PostAsJsonAsync($"{RoutesBaseAddress}", entity);
            Guid resultGuid = Guid.Empty;
            if (response.StatusCode.IsSuccess() && Guid.TryParse(response.Headers.Location?.AbsoluteUri.Split('/')[^1], out Guid guid))
            {
                resultGuid = guid;
            }
            return (response.StatusCode, resultGuid);
        }

        /// <summary>
        /// Modifies an existing item.
        /// </summary>
        /// <param name="entity">The item to be edited</param>
        /// <returns>The result code of the request and the modified item</returns>
        public async Task<(HttpStatusCode, TEntity)> PutOne(TEntity entity)
        {
            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{RoutesBaseAddress}/{entity.Id}", entity);
            if (response.StatusCode.IsSuccess())
            {
                return (response.StatusCode, await response.Content.ReadFromJsonAsync<TEntity>());
            }
            return (response.StatusCode, null);
        }
    }
}