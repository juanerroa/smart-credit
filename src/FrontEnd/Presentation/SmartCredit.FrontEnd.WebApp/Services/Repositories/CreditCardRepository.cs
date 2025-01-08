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
    public class CreditCardRepository : ICreditCardRepository
    {
        private readonly HttpClient _httpClient;
        public CreditCardRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<CreditCardViewModel> AddUserAndCreditCardAsync(CreditCardViewModel creditCard)
        {
            var endPoint = $"{ConfigurationHelper.GetApiUrl()}/api/CreditCard/AddUserAndCreditCard";

            var requestBody = new
            {
                User = new
                {
                    FullName = creditCard.FullName,
                    Address = creditCard.Address,
                    City = creditCard.City,
                    State = creditCard.State,
                    Country = creditCard.Country,
                    Email = creditCard.Email
                },
                Card = new
                {
                    CardNumber = creditCard.CardNumber,
                    HolderName = creditCard.HolderName,
                    CreditLimit = creditCard.CreditLimit,
                    Balance = creditCard.Balance,
                    AvailableBalance = creditCard.AvailableBalance,
                    ClosingDay = creditCard.ClosingDay,
                    Type = creditCard.Type
                }
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
                var dataResponse = JsonSerializer.Deserialize<ApiResponse<CreditCardViewModel>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (dataResponse.Success)
                    return dataResponse.Data;
            }

            return null;
        }

        public Task<PaymentViewModel> CalculatePayments(int creditCardId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<CreditCardViewModel>> GetAll()
        {
            var endPoint = $"{ConfigurationHelper.GetApiUrl()}/api/CreditCard/GetAll";
            var response = await _httpClient.GetAsync(endPoint);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var dataResponse = JsonSerializer.Deserialize<ApiResponse<List<CreditCardViewModel>>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true // Ignora las diferencias entre PascalCase y camelCase
                });

                if (dataResponse.Success)
                    return dataResponse.Data;
            }

            return new List<CreditCardViewModel>();
        }

        public async Task<CreditCardViewModel> GetById(Guid? creditCardId)
        {
            var endPoint = $"{ConfigurationHelper.GetApiUrl()}/api/CreditCard/GetById/{creditCardId}";
            var response = await _httpClient.GetAsync(endPoint);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var dataResponse = JsonSerializer.Deserialize<ApiResponse<CreditCardViewModel>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true // Ignora las diferencias entre PascalCase y camelCase
                });

                if (dataResponse.Success)
                    return dataResponse.Data;
            }

            return null;
        }

        public async Task<CreditCardStatementViewModel> GetCreditCardStatement(Guid creditCardId, int year, int month)
        {
            var endPoint = $"{ConfigurationHelper.GetApiUrl()}/api/CreditCard/GetCreditCardStatement";

            var requestBody = new
            {
                CreditCardId = creditCardId,
                Year = year,
                Month = month
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
                var dataResponse = JsonSerializer.Deserialize<ApiResponse<CreditCardStatementViewModel>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (dataResponse.Success)
                    return dataResponse.Data;
            }

            return new CreditCardStatementViewModel();
        }
    }
}