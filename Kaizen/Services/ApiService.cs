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
            var authentication = new { email = user, password };
            var json = JsonConvert.SerializeObject(authentication);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await this.httpClient.PostAsync($"{this.apiEndpoint}/accounts/login", content);
            var contentResponse = response.Content.ReadAsStringAsync().Result;

            if (response.IsSuccessStatusCode)
            {
                var token = JsonConvert.DeserializeObject<AuthenticationResponse>(contentResponse).Token;
                await SecureStorage.SetAsync("token", token);
                return response.IsSuccessStatusCode;
            }
            else 
            {
                throw new Exception(contentResponse);
            }            
        }

        public async Task<bool> SignUp(UserCredentials userCredentials)
        {
            var json = JsonConvert.SerializeObject(userCredentials);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await this.httpClient.PostAsync($"{this.apiEndpoint}/accounts/signUp", content);
            var contentResponse = await response.Content.ReadAsStringAsync();
            


            if (response.IsSuccessStatusCode)
                return response.IsSuccessStatusCode;
            else 
                throw new Exception(ParseErrorsAPI(JsonConvert
                    .DeserializeObject<IEnumerable<SignUpResponse>>(contentResponse)));
        }

        public async Task<QueryResult<Campaign>> GetAgentValidCampaigns()
        {
            var token = await SecureStorage.GetAsync("token");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await httpClient.GetStringAsync($"{this.apiEndpoint}/campaigns/agents/valids");
            return JsonConvert.DeserializeObject<QueryResult<Campaign>>(response);
        }

        public async Task<bool> SynchronizeTodayCalls(IEnumerable<CallLogModel> callLogs) 
        {
            try
            {
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

        private string ParseErrorsAPI(IEnumerable<SignUpResponse> responses) 
        {
            string messageError = string.Empty;
            foreach (var response in responses)
            {
                messageError += response.Description + "\n";
            }
            return messageError;
        }
    }
}
