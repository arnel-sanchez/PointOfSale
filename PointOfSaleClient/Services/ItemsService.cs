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

        public Task Add(ItemDTO item);

        public Task Update(string id, ItemDTO item);

        public Task Delete(string id);

        public Task AssignDissasignModifier(string itemId, string modifierId);
    }

    public class ItemService : IItemService
    {
        private IHttpClientFactory ClientFactory { get; set; }

        public ItemService(IHttpClientFactory _clientFactory)
        {
            ClientFactory = _clientFactory;
        }

        public async Task Add(ItemDTO item)
        {
            var request = new HttpRequestMessage(HttpMethod.Post,
            "https://localhost:7134/api/items/add");
            request.Headers.Add("Accept", "*/*");
            request.Content = new StringContent(JsonSerializer.Serialize(item), Encoding.UTF8, "application/json");

            var client = ClientFactory.CreateClient();

            var response = await client.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error");
            }
        }

        public async Task Delete(string id)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete,
            "https://localhost:7134/api/items/delete/" + id);
            request.Headers.Add("Accept", "*/*");

            var client = ClientFactory.CreateClient();

            var response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
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
            var res = JsonSerializer.Deserialize<List<Item>>(responseString);
            if (response.IsSuccessStatusCode)
            {
                return res;
            }
            else
            {
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
            var res = JsonSerializer.Deserialize<Item>(responseString);
            res.modifiersId = new List<Modifier>();
            var conversion1 = responseString.Split("\"modifiers\":[");
            var conversion2 = conversion1[1].Split("]");
            var conversion3 = conversion2[0].Split("},{");
            foreach (var modifier in conversion3)
            {
                if (string.IsNullOrEmpty(modifier))
                    break;
                var modifierToParse = modifier;
                if (modifier[0]!='{')
                    modifierToParse = "{" + modifierToParse;
                if (modifier[modifier.Length - 1] != '}')
                    modifierToParse = modifierToParse + "}";
                res.modifiersId.Add(JsonSerializer.Deserialize<Modifier>(modifierToParse));
            }
            if (response.IsSuccessStatusCode)
            {
                return res;
            }
            else
            {
                throw new Exception("Error");
            }
        }

        public async Task Update(string id, ItemDTO item)
        {
            var request = new HttpRequestMessage(HttpMethod.Put,
            "https://localhost:7134/api/items/update/" + id );
            request.Headers.Add("Accept", "*/*");
            request.Content = new StringContent(JsonSerializer.Serialize(item), Encoding.UTF8, "application/json");

            var client = ClientFactory.CreateClient();

            var response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error");
            }
        }

        public async Task AssignDissasignModifier(string itemId, string modifierId)
        {
            var request = new HttpRequestMessage(HttpMethod.Put,
            $"https://localhost:7134/api/items/assign/{itemId}/{modifierId}");
            request.Headers.Add("Accept", "*/*");

            var client = ClientFactory.CreateClient();

            var response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error");
            }
        }
    }
}
