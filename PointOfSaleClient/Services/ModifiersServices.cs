using PointOfSaleClient.Models;
using System.Text;
using System.Text.Json;

namespace PointOfSaleClient.Services
{
    public interface IModifierService
    {
        public Task<List<Modifier>> GetAll();

        public Task<Modifier> GetItem(string id);

        public Task Add(ModifierDTO modifier);

        public Task Update(string id, ModifierDTO modifier);

        public Task Delete(string id);
    }

    public class ModifierService : IModifierService
    {
        private IHttpClientFactory ClientFactory { get; set; }
        
        public ModifierService(IHttpClientFactory _clientFactory)
        {
            ClientFactory = _clientFactory;
        }

        public async Task<List<Modifier>> GetAll()
        {
            var request = new HttpRequestMessage(HttpMethod.Get,
            "https://localhost:7134/api/modifiers/get-all");
            request.Headers.Add("Accept", "*/*");

            var client = ClientFactory.CreateClient();

            var response = await client.SendAsync(request);
            var responseString = await response.Content.ReadAsStringAsync();
            var res = JsonSerializer.Deserialize<List<Modifier>>(responseString);
            if (response.IsSuccessStatusCode)
            {
                return res;
            }
            else
            {
                throw new Exception("Error");
            }
        }

        public Task<Modifier> GetItem(string id)
        {
            throw new NotImplementedException();
        }

        public async Task Add(ModifierDTO modifier)
        {
            var request = new HttpRequestMessage(HttpMethod.Post,
            "https://localhost:7134/api/modifiers/add");
            request.Headers.Add("Accept", "*/*");
            var json = "{\"name\": \""+ modifier.name +"\"," +
                       "\"description\": \"" + modifier.description + "\"," +
                       "\"price\": " + modifier.price.ToString().Replace(",", ".") + "," +
                       "\"add\": " + modifier.add.ToString().ToLower() +
                    "}";
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            var client = ClientFactory.CreateClient();

            var response = await client.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error");
            }
        }

        public async Task Update(string id, ModifierDTO modifier)
        {
            var request = new HttpRequestMessage(HttpMethod.Put,
            "https://localhost:7134/api/modifiers/update/" + id);
            request.Headers.Add("Accept", "*/*");
            var json = "{\"name\": \"" + modifier.name + "\"," +
                      "\"description\": \"" + modifier.description + "\"," +
                      "\"price\": " + modifier.price.ToString().Replace(",", ".") + "," +
                      "\"add\": " + modifier.add.ToString().ToLower() +
                   "}";
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

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
            "https://localhost:7134/api/modifiers/delete/" + id);
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
