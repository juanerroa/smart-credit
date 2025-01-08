using SmartCredit.FrontEnd.WebApp.Helpers;
using SmartCredit.FrontEnd.WebApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SmartCredit.FrontEnd.WebApp.Contracts.Repositories
{
    public class TransactionsRepository : ITransactionsRepository
    {
        private readonly HttpClient _httpClient;
        public TransactionsRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<TransactionsViewModel> AddPayment(Guid creditCardId, decimal amout, DateTime date)
        {
            var endPoint = $"{ConfigurationHelper.GetApiUrl()}/api/Transactions/AddPayment";

            var requestBody = new
            {
                CreditCardId = creditCardId,
                Date = date,
                Amount = amout
            };

            var jsonContent = new StringContent(
                JsonSerializer.Serialize(requestBody),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.PostAsync(endPoint, jsonContent);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var dataResponse = JsonSerializer.Deserialize<ApiResponse<TransactionsViewModel>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (dataResponse.Success)
                    return dataResponse.Data;
            }

            return null;
        }

        public async Task<TransactionsViewModel> AddPurchase(Guid creditCardId, string description, decimal amout, DateTime date)
        {
            var endPoint = $"{ConfigurationHelper.GetApiUrl()}/api/Transactions/AddPurchase";

            var requestBody = new
            {
                CreditCardId = creditCardId,
                Description = description,
                Date = date,
                Amount = amout
            };

            var jsonContent = new StringContent(
                JsonSerializer.Serialize(requestBody),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.PostAsync(endPoint, jsonContent);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var dataResponse = JsonSerializer.Deserialize<ApiResponse<TransactionsViewModel>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (dataResponse.Success)
                    return dataResponse.Data;
            }

            return null;
        }

        public async Task<List<TransactionsViewModel>> GetByPeriod(Guid creditCardId, int year, int month, int? type)
        {
            var endPoint = $"{ConfigurationHelper.GetApiUrl()}/api/Transactions/GetByPeriod";

            var requestBody = new
            {
                CreditCardId = creditCardId,
                Year = year,
                Month = month,
                Type = type
            };

            var jsonContent = new StringContent(
                JsonSerializer.Serialize(requestBody),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.PostAsync(endPoint, jsonContent);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var dataResponse = JsonSerializer.Deserialize<ApiResponse<List<TransactionsViewModel>>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (dataResponse.Success)
                    return dataResponse.Data;
            }

            return new List<TransactionsViewModel>();
        }

    }
}