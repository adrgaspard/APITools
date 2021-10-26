using APIBase.Core.DAO.Models;
using APIBase.Core.Ioc;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace APIBase.ASPTools.Client
{
    public class RestfulWebClient<TEntity> where TEntity : class, IGuidResolvable
    {
        protected HttpClient HttpClient { get; init; }

        public string RoutesBaseAddress { get; protected init; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="automaticallySetHttpClientWithIoc"></param>
        /// <param name="routesBaseAddress"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public RestfulWebClient(bool automaticallySetHttpClientWithIoc, string routesBaseAddress)
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
        /// 
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="routesBaseAddress"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public RestfulWebClient(HttpClient httpClient, string routesBaseAddress) : this(false, routesBaseAddress)
        {
            if (httpClient is null)
            {
                throw new ArgumentNullException(nameof(httpClient));
            }
            HttpClient = httpClient;
        }

        public async Task<HttpStatusCode> DeleteOne(Guid id)
        {
            HttpResponseMessage response = await HttpClient.DeleteAsync($"{RoutesBaseAddress}/{id}");
            return response.StatusCode;
        }

        public async Task<IEnumerable<TEntity>> GetAll()
        {
            List<TEntity> entities = null;
            HttpResponseMessage response = await HttpClient.GetAsync($"{RoutesBaseAddress}");
            if (response.IsSuccessStatusCode)
            {
                entities = await response.Content.ReadFromJsonAsync<List<TEntity>>();
            }
            return entities;
        }

        public async Task<TEntity> GetOne(Guid id)
        {
            TEntity entity = null;
            HttpResponseMessage response = await HttpClient.GetAsync($"{RoutesBaseAddress}/{id}");
            if (response.IsSuccessStatusCode)
            {
                entity = await response.Content.ReadFromJsonAsync<TEntity>();
            }
            return entity;
        }

        public async Task<TEntity> PatchOne(TEntity entity, JsonPatchDocument<TEntity> patchDocument)
        {
            HttpResponseMessage response = await HttpClient.PatchAsync($"{RoutesBaseAddress}/{entity.Id}", JsonContent.Create(patchDocument));
            response.EnsureSuccessStatusCode();
            entity = await response.Content.ReadFromJsonAsync<TEntity>();
            return entity;
        }

        public async Task<Uri> PostOne(TEntity entity)
        {
            HttpResponseMessage response = await HttpClient.PostAsJsonAsync($"{RoutesBaseAddress}", entity);
            response.EnsureSuccessStatusCode();
            return response.Headers.Location;
        }

        public async Task<TEntity> PutOne(TEntity entity)
        {
            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{RoutesBaseAddress}/{entity.Id}", entity);
            response.EnsureSuccessStatusCode();
            entity = await response.Content.ReadFromJsonAsync<TEntity>();
            return entity;
        }
    }
}
