using Kaizen.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Kaizen.Services
{
    public class ApiService
    {
        private readonly HttpClient httpClient;
        //private readonly string apiEndpoint = $"https://10.0.2.2:5001/api";
        //private readonly string apiEndpoint = $"https://localhost:5001/api";
        //private readonly string apiEndpoint = $"http://192.168.1.49:5000/api";
        private readonly string apiEndpoint = $"https://192.168.1.49:5001/api";


        public ApiService()
        {
            var httpHandler = new HttpClientHandler
            {
                UseDefaultCredentials = false,
                ServerCertificateCustomValidationCallback = (o, cert, chain, errors) => true
            };
            this.httpClient = new HttpClient(httpHandler);
        }

        public async Task<bool> Login(string user, string password) 
        {
            try
            {
                var authentication = new
                {
                    email = user,
                    password
                };
                var json = JsonConvert.SerializeObject(authentication);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await this.httpClient.PostAsync($"{this.apiEndpoint}/accounts/login", content);
                var contentResponse = response.Content.ReadAsStringAsync().Result;
                var token = JsonConvert.DeserializeObject<AuthenticationResponse>(contentResponse).Token;
                await SecureStorage.SetAsync("token", token);

                return response.IsSuccessStatusCode;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        //public async Task<User> GetUserByEmail(string email) 
        //{
        //    var token = await SecureStorage.GetAsync("token");
        //    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        //    var response = await httpClient.GetStringAsync($"{apiEndpoint}/users/{email}");
        //    return JsonConvert.DeserializeObject<User>(response);
        //}

        public async Task<bool> SynchronizeTodayCalls(IEnumerable<CallLogModel> callLogs) 
        {
            try
            {
                //var email = await SecureStorage.GetAsync("user");
                //var user = await GetUserByEmail(email);

                var token = await SecureStorage.GetAsync("token");
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var json = JsonConvert.SerializeObject(callLogs);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await this.httpClient.PostAsync($"{this.apiEndpoint}/calls/synchronize", content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                return false;                
            }

        }
    }
}
