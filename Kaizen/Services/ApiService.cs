using Kaizen.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Kaizen.Services
{
    public class ApiService
    {
        private readonly HttpClient httpClient;
        //private readonly string apiEndpoint = $"http://10.0.2.2:5000/api";
        private readonly string apiEndpoint = $"http://192.168.1.49:5000/api";



        public ApiService()
        {
            //this.httpClient = new HttpClient();

            var httpHandler = new HttpClientHandler
            {
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
    }
}
