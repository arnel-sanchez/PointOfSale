using Microsoft.AspNetCore.Mvc;
using PointOfSaleClient.Models;
using System.Text;
using System.Text.Json;

namespace PointOfSaleClient.Services
{
    public interface IItemService
    {
        public Task<List<Item>> GetAll();

        public Task<Item> GetItem(string id);

        public Task<string> Add(ItemDTO item);

        public Task<string> Update(string id, ItemDTO item);

        public Task<string> Delete(string id);
    }

    public class ItemService : IItemService
    {
        private IHttpClientFactory ClientFactory { get; set; }

        public ItemService(IHttpClientFactory _clientFactory)
        {
            ClientFactory = _clientFactory;
        }

        public async Task<string> Add(ItemDTO item)
        {
            var request = new HttpRequestMessage(HttpMethod.Post,
            "https://localhost:7134/api/items/add");
            request.Headers.Add("Accept", "*/*");
            request.Content = new StringContent(JsonSerializer.Serialize(item), Encoding.UTF8, "application/json");

            var client = ClientFactory.CreateClient();

            var response = await client.SendAsync(request);
            var responseString = await response.Content.ReadAsStringAsync();
            var res = JsonSerializer.Deserialize<Response<string>>(responseString);
            if (response.IsSuccessStatusCode)
            {
                return res.message;
            }
            else
            {
                if (!string.IsNullOrEmpty(res.message))
                    throw new Exception(res.message);
                throw new Exception("Error");
            }
        }

        public async Task<string> Delete(string id)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete,
            "https://localhost:7134/api/items/delete/" + id);
            request.Headers.Add("Accept", "*/*");

            var client = ClientFactory.CreateClient();

            var response = await client.SendAsync(request);
            var responseString = await response.Content.ReadAsStringAsync();
            var res = JsonSerializer.Deserialize<Response<string>>(responseString);
            if (response.IsSuccessStatusCode)
            {
                return res.message;
            }
            else
            {
                if (!string.IsNullOrEmpty(res.message))
                    throw new Exception(res.message);
                throw new Exception("Error");
            }
        }

        public async Task<List<Item>> GetAll()
        {
            var request = new HttpRequestMessage(HttpMethod.Get,
            "https://localhost:7134/api/items/get-all");
            request.Headers.Add("Accept", "*/*");

            var client = ClientFactory.CreateClient();

            var response = await client.SendAsync(request);
            var responseString = await response.Content.ReadAsStringAsync();
            var res = JsonSerializer.Deserialize<Response<List<Item>>>(responseString);
            if (response.IsSuccessStatusCode)
            {
                return res.data;
            }
            else
            {
                if (!string.IsNullOrEmpty(res.message))
                    throw new Exception(res.message);
                throw new Exception("Error");
            }
        }

        public async Task<Item> GetItem(string id)
        {
            var request = new HttpRequestMessage(HttpMethod.Get,
            "https://localhost:7134/api/items/get/" + id);
            request.Headers.Add("Accept", "*/*");

            var client = ClientFactory.CreateClient();

            var response = await client.SendAsync(request);
            var responseString = await response.Content.ReadAsStringAsync();
            var res = JsonSerializer.Deserialize<Response<Item>>(responseString);
            if (response.IsSuccessStatusCode)
            {
                return res.data;
            }
            else
            {
                if (!string.IsNullOrEmpty(res.message))
                    throw new Exception(res.message);
                throw new Exception(res.message);
            }
        }

        public async Task<string> Update(string id, ItemDTO item)
        {
            var request = new HttpRequestMessage(HttpMethod.Put,
            "https://localhost:7134/api/items/update/" + id );
            request.Headers.Add("Accept", "*/*");
            request.Content = new StringContent(JsonSerializer.Serialize(item), Encoding.UTF8, "application/json");

            var client = ClientFactory.CreateClient();

            var response = await client.SendAsync(request);
            var responseString = await response.Content.ReadAsStringAsync();
            var res = JsonSerializer.Deserialize<Response<string>>(responseString);

            if (response.IsSuccessStatusCode)
            {
                return res.message;
            }
            else
            {
                if(!string.IsNullOrEmpty(res.message))
                    throw new Exception(res.message);
                throw new Exception("Error");
            }
        }
    }
}
