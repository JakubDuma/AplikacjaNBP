using Microsoft.AspNetCore.Mvc;
using NbpAPI.Data.Models;
using NbpAPI.Services.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NbpAPI.Controllers
{
    [Route("currencies")]
    [ApiController]
    public class CurrenciesController : ControllerBase
    {
        private readonly CurrencyService _currencyService;
        public CurrenciesController(CurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        // GET: api/<Currencies>
        [HttpGet]
        [Route("GetExchangeRates")]
        public async Task<IActionResult> Get(string table)
        {
            var data = await _currencyService.GetAvailableCurrencies(table);

            if (data == null || data.Count == 0)
            {
               return NotFound("No exchange rates found.");
            }
               
            return Ok(data);

        }
        [HttpGet]
        [Route("RatesDateRange")]
        public async Task<IActionResult> GetRatesForDateRange(string currency, DateTime startDate, DateTime endDate)
        {
            var rates = await _currencyService.GetRatesForDateRangeAsync(currency, startDate, endDate);
            return Ok(rates);
        }

        // POST api/<Currencies>
        [HttpPost]
        [Route("DateHistory")]
        public async Task<IActionResult> Post(string tableCode, string currencyCode, DateTime startDate, DateTime endDate)
        {
            var data = await _currencyService.FetchCurrencyHistoryAsync(tableCode, currencyCode, startDate, endDate);
            return Ok(data);
        }

    }
}
