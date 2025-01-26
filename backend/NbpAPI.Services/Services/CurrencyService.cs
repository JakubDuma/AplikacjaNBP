using NbpAPI.Data;
using NbpAPI.Data.Models;
using NbpAPI.Services.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using AutoMapper;
using NbpAPI.Data.Repository;
using NbpAPI.Data.Interface;
using Microsoft.EntityFrameworkCore;

namespace NbpAPI.Services.Services
{
    public class CurrencyService
    {
        private readonly HttpClient _httpClient;
        private readonly IMapper _mapper;
        private readonly IRateRepository _rateRepository;

        public CurrencyService(HttpClient httpClient, IMapper mapper, IRateRepository rateRepository)
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://api.nbp.pl"),
                Timeout = TimeSpan.FromSeconds(60) // Set timeout to 60 seconds
            };
            _mapper = mapper;
            _rateRepository = rateRepository;
        }

        public async Task<List<RateDTO>> GetAvailableCurrencies(string table)
        {
            var currencies = new List<RateDTO>();
            var url = $"http://api.nbp.pl/api/exchangerates/tables/{table}?format=json";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true // Ignoruje wielkość liter
            };

            var content = await response.Content.ReadAsStringAsync();
            var dataList = JsonSerializer.Deserialize<List<ExchangeRateDTO>>(content, options);
            var data = dataList?.First();


            if (data.Rates is not null)
            {
                 currencies.AddRange(data.Rates);
            }
            return currencies;
        }

        public async Task<List<RateByDateDTO>> FetchCurrencyHistoryAsync(string tableCode, string currencyCode, DateTime startDate, DateTime endDate)
        {
            var allRates = new List<RateByDateDTO>();
            var maxDays = 91;
            var currentStart = startDate;

            while (currentStart <= endDate)
            {
                var currentEnd = currentStart.AddDays(maxDays - 1);
                if (currentEnd > endDate) currentEnd = endDate;

                // Tworzymy URL do API NBP
                var url = $"http://api.nbp.pl/api/exchangerates/rates/{tableCode}/{currencyCode}/{currentStart:yyyy-MM-dd}/{currentEnd:yyyy-MM-dd}?format=json";

                var response = await _httpClient.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Failed to fetch data for {currencyCode} from {currentStart} to {currentEnd}");
                }

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var content = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<ExchangeRateByDateDTO>(content, options);

                if (data?.Rates is not null)
                {
                    var existingRates = _rateRepository.GetAll().Where(c => c.CurrencyCode == currencyCode).ToList();
                    foreach (var rate in data.Rates)
                    {
                        // Sprawdzenie czy wpis już istnieje w bazie danych
                        if (!existingRates.Any(c => c.CurrencyCode == currencyCode && c.EffectiveDate == rate.EffectiveDate))
                        {
                            // Jeśli wpis nie istnieje, zapisujemy go w bazie
                            var currency = new RateByDateDTO
                            {
                                CurrencyCode = currencyCode,
                                No = rate.No,
                                Mid = rate.Mid,
                                EffectiveDate = rate.EffectiveDate
                            };

                            _rateRepository.Create(_mapper.Map<RateByDate>(currency));
                        }
                    }
                        allRates.AddRange(data.Rates);
                }

                currentStart = currentEnd.AddDays(1); // Przesuwamy start na kolejny dzień
            }

            return allRates;
        }
        
        public List<RateByDateDTO> GetAll()
        {
            var rates = _rateRepository.GetAll();
            return _mapper.Map<List<RateByDateDTO>>(rates);
        }
        public async Task<List<RateByDate>> GetRatesForDateRangeAsync(string currencyCode, DateTime startDate, DateTime endDate)
        {
            // Sprawdzenie, czy dane istnieją w bazie
            var rates = await _rateRepository.GetRatesForDateRangeAsync(currencyCode, startDate, endDate);

            // Jeśli brak danych, można obsłużyć odpowiedni scenariusz
            if (rates == null || !rates.Any())
            {
                throw new Exception($"No data found for {currencyCode} between {startDate:yyyy-MM-dd} and {endDate:yyyy-MM-dd}");
            }

            return rates;
        }
    }
}

