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

        public Task Register();

        public Task Logout();

        public Task<bool> RefreshToken();

        public Task<UserMetadata> GetUser();

        public Task ForgotPassword();

        public Task ResetPassword();
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
            var responseString = await response.Content.ReadAsStringAsync();
            var res = JsonSerializer.Deserialize<UserMetadata>(responseString);
            if (response.IsSuccessStatusCode)
            {
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
            if (userMetadata == null || string.IsNullOrEmpty(userMetadata.accessToken) || string.IsNullOrEmpty(userMetadata.refreshToken) || string.IsNullOrEmpty(userMetadata.username) || string.IsNullOrEmpty(userMetadata.role))
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

        public Task Register()
        {
            throw new NotImplementedException();
        }

        public async Task Logout()
        {
            var userMetadata = (UserMetadata)Cache.Get("user");
            if (userMetadata == null || string.IsNullOrEmpty(userMetadata.accessToken) || string.IsNullOrEmpty(userMetadata.refreshToken) || string.IsNullOrEmpty(userMetadata.username) || string.IsNullOrEmpty(userMetadata.role))
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
            if (userMetadata == null || string.IsNullOrEmpty(userMetadata.accessToken) || string.IsNullOrEmpty(userMetadata.refreshToken) || string.IsNullOrEmpty(userMetadata.username) || string.IsNullOrEmpty(userMetadata.role))
                return false;
            var refreshToken = new RefreshToken
            {
                refreshToken = userMetadata.refreshToken
            };
            var request = new HttpRequestMessage(HttpMethod.Post,
            "https://localhost:7134/api/auth/get-user");
            request.Headers.Add("Accept", "*/*");
            request.Headers.Add("Authorization", "Bearer " + userMetadata.accessToken);
            request.Content = new StringContent(JsonSerializer.Serialize(refreshToken), Encoding.UTF8, "application/json");

            var client = ClientFactory.CreateClient();

            var response = await client.SendAsync(request);
            var responseString = await response.Content.ReadAsStringAsync();
            var res = JsonSerializer.Deserialize<UserMetadata>(responseString);
            if (response.IsSuccessStatusCode)
            {
                Cache.Set("user", res);
                return true;
            }
            else
            {
                return false;
            }
        }

        public Task<UserMetadata> GetUser()
        {
            throw new NotImplementedException();
        }

        public Task ForgotPassword()
        {
            throw new NotImplementedException();
        }

        public Task ResetPassword()
        {
            throw new NotImplementedException();
        }
    }
}
