using Microsoft.Extensions.Caching.Memory;
using Microsoft.JSInterop;
using PointOfSaleClient.Models;
using System.Text;
using System.Text.Json;

namespace PointOfSaleClient.Services
{
    public interface IUserService
    {
        public Task Login(Login item);

        public Task<bool> UserIsLogged();

        public Task Add(CreateRegisterUserDTO user);

        public Task Update(string id, CreateRegisterUserDTO user);

        public Task Logout();

        public Task<bool> RefreshToken();

        public Task<User> GetUser();

        public Task ForgotPassword();

        public Task ResetPassword();

        public Task<List<User>> GetUsers();

        public Task Delete(string id);
    }

    public class UserService : IUserService
    {
        private IHttpClientFactory ClientFactory { get; set; }
        
        private IMemoryCache Cache { get; set; }
        
        public UserService(IHttpClientFactory _clientFactory, IMemoryCache _cache)
        {
            ClientFactory = _clientFactory;
            Cache = _cache;
        }
        
        public async Task Login(Login item)
        {
            var request = new HttpRequestMessage(HttpMethod.Post,
            "https://localhost:7134/api/auth/login");
            request.Headers.Add("Accept", "*/*");
            request.Content = new StringContent(JsonSerializer.Serialize(item), Encoding.UTF8, "application/json");

            var client = ClientFactory.CreateClient();

            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                var res = JsonSerializer.Deserialize<UserMetadata>(responseString);
                Cache.Set("user", res);
            }
            else
            {
                throw new Exception("Error");
            }
        }

        public async Task<bool> UserIsLogged()
        {
            var userMetadata = (UserMetadata)Cache.Get("user");
            if (userMetadata == null || string.IsNullOrEmpty(userMetadata.accessToken) || string.IsNullOrEmpty(userMetadata.refreshToken))
                return false;
            var request = new HttpRequestMessage(HttpMethod.Get,
            "https://localhost:7134/api/auth/get-user");
            request.Headers.Add("Accept", "*/*");
            request.Headers.Add("Authorization", "Bearer " + userMetadata.accessToken);

            var client = ClientFactory.CreateClient();
            
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task Logout()
        {
            var userMetadata = (UserMetadata)Cache.Get("user");
            if (userMetadata == null || string.IsNullOrEmpty(userMetadata.accessToken) || string.IsNullOrEmpty(userMetadata.refreshToken))
                return;
            var request = new HttpRequestMessage(HttpMethod.Post,
            "https://localhost:7134/api/auth/logout");
            request.Headers.Add("Accept", "*/*");
            request.Headers.Add("Authorization", "Bearer " + userMetadata.accessToken);

            var client = ClientFactory.CreateClient();

            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                Cache.Remove("user");
            }
        }

        public async Task<bool> RefreshToken()
        {
            var userMetadata = (UserMetadata)Cache.Get("user");
            if (userMetadata == null || string.IsNullOrEmpty(userMetadata.accessToken) || string.IsNullOrEmpty(userMetadata.refreshToken))
                return false;
            var refreshToken = new RefreshToken
            {
                refreshToken = userMetadata.refreshToken,
                accessToken = userMetadata.accessToken
            };
            var request = new HttpRequestMessage(HttpMethod.Post,
            "https://localhost:7134/api/auth/get-user");
            request.Headers.Add("Accept", "*/*");
            request.Content = new StringContent(JsonSerializer.Serialize(refreshToken), Encoding.UTF8, "application/json");

            var client = ClientFactory.CreateClient();

            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                var res = JsonSerializer.Deserialize<UserMetadata>(responseString);
                Cache.Set("user", res);
                return true;
            }
            else
            {
                return false;
            }
        }

        public Task<User> GetUser()
        {
            throw new NotImplementedException();
        }

        public async Task<List<User>> GetUsers()
        {
            var userMetadata = (UserMetadata)Cache.Get("user");
            if (userMetadata == null || string.IsNullOrEmpty(userMetadata.accessToken) || string.IsNullOrEmpty(userMetadata.refreshToken))
                return new List<User>();
            var request = new HttpRequestMessage(HttpMethod.Get,
            "https://localhost:7134/api/auth/get-all");
            request.Headers.Add("Accept", "*/*");
            request.Headers.Add("Authorization", "Bearer " + userMetadata.accessToken);
            
            var client = ClientFactory.CreateClient();

            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<User>>(responseString);
            }
            else
            {
                throw new Exception("Error");
            }
        } 

        public Task ForgotPassword()
        {
            throw new NotImplementedException();
        }

        public Task ResetPassword()
        {
            throw new NotImplementedException();
        }

        public async Task Add(CreateRegisterUserDTO user)
        {
            var userMetadata = (UserMetadata)Cache.Get("user");
            if (userMetadata == null || string.IsNullOrEmpty(userMetadata.accessToken) || string.IsNullOrEmpty(userMetadata.refreshToken))
                return;
            var request = new HttpRequestMessage(HttpMethod.Post,
            "https://localhost:7134/api/auth/register");
            request.Headers.Add("Accept", "*/*");
            request.Headers.Add("Authorization", "Bearer " + userMetadata.accessToken);
            request.Content = new StringContent(JsonSerializer.Serialize(user), Encoding.UTF8, "application/json");

            var client = ClientFactory.CreateClient();

            var response = await client.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error");
            }
        }
        
        public async Task Update(string id, CreateRegisterUserDTO user)
        {
            var userMetadata = (UserMetadata)Cache.Get("user");
            if (userMetadata == null || string.IsNullOrEmpty(userMetadata.accessToken) || string.IsNullOrEmpty(userMetadata.refreshToken))
                return;
            var request = new HttpRequestMessage(HttpMethod.Put,
            $"https://localhost:7134/api/auth/update/{id}");
            request.Headers.Add("Accept", "*/*");
            request.Headers.Add("Authorization", "Bearer " + userMetadata.accessToken);
            var json = JsonSerializer.Serialize(user);
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
            var userMetadata = (UserMetadata)Cache.Get("user");
            if (userMetadata == null || string.IsNullOrEmpty(userMetadata.accessToken) || string.IsNullOrEmpty(userMetadata.refreshToken))
                return;
            var request = new HttpRequestMessage(HttpMethod.Delete,
            $"https://localhost:7134/api/auth/delete/{id}");
            request.Headers.Add("Accept", "*/*");
            request.Headers.Add("Authorization", "Bearer " + userMetadata.accessToken);

            var client = ClientFactory.CreateClient();

            var response = await client.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error");
            }
        }
    }
}
