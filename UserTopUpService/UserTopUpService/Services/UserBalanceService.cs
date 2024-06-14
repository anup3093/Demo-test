using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using UserTopUpService.DataBase;
using UserTopUpService.Repositories;

namespace UserTopUpService.Services
{
    public class UserBalanceService : IUserBalanceService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IDbConnectionFactory _connectionFactory;
        public UserBalanceService(IDbConnectionFactory connectionFactory, IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _connectionFactory = connectionFactory;
        }

        public async Task<int> GetUserBalanceAsync(int userId)
        {
            var httpClient = _httpClientFactory.CreateClient();
            // httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "token");
            var response = await httpClient.GetAsync($"https://localhost:7235/Balance/{userId}");
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Failed to retrieve user balance.");
            }

            var content = await response.Content.ReadAsStringAsync();
            var balance = int.Parse(content);
            return balance;
        }
        public async Task CreditUserBalanceAsync(int userId, int amount)
        {
            var httpClient = _httpClientFactory.CreateClient();
            //httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "token");
            var response = await httpClient.PostAsJsonAsync($"https://localhost:7235/Balance/{userId}/credit?amount={0}", amount);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Failed to credit user balance.");
            }
        }
        public async Task DebitUserBalanceAsync(int userId, int amount)
        {
            var httpClient = _httpClientFactory.CreateClient();
            //httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "token");
            var response = await httpClient.PostAsJsonAsync($"https://localhost:7235/Balance/{userId}/debit?amount={0}", amount);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Failed to credit user balance.");
            }
        }

    }



}
