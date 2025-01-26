using TechTalk.SpecFlow;
using NbpAPI.Services.Services;
using NbpAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using AutoMapper;
using NbpAPI.Data.Interface;
using NbpAPI.Services.DTO;
using NbpAPI.Services.MapperProfile;
using Moq;
using NbpAPI.Data.Models;
using System.Runtime.Serialization;
namespace NbpAPI.Ttest
{
    [Binding]
    public class Tests : IDisposable
    {
        private HttpClient _httpClient;
        private DBContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IRateRepository _rateRepository;
        private readonly Mock<IRateRepository> _rateRepositoryMock;
        private List<RateDTO> _availableCurrencies;
        private List<RateByDateDTO> _currencyHistory;
        public Tests()
        {
            _httpClient = new HttpClient();
            _rateRepository = ConfigureMockRateRepository();
            _mapper = ConfigureMapper();
        }

        private IRateRepository ConfigureMockRateRepository()
        {
            var mockRepository = new Mock<IRateRepository>();
            mockRepository.Setup(repo => repo.GetAll()).Returns(new List<RateByDate>());
            return mockRepository.Object;
        }
        private IMapper ConfigureMapper()
        {
            var mappingConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MapperProfile());
            });
            return mappingConfig.CreateMapper();
        }

        [BeforeScenario]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<DBContext>().UseInMemoryDatabase("TestDatabase").Options;
            _dbContext = new DBContext(options);
            _dbContext.Database.EnsureCreated();
        }

        [Given(@"I have a valid table identifier ""(.*)""")]
        public void GivenIHaveAValidTableIdentifier(string table)
        {
            ScenarioContext.Current["Table"] = table;
        }

        [When(@"I fetch available currencies")]
        public async Task WhenIFetchAvailableCurrencies()
        {
            var table = ScenarioContext.Current["Table"].ToString();
            var service = new CurrencyService(_httpClient, _mapper, _rateRepository);
            _availableCurrencies = await service.GetAvailableCurrencies(table);
        }

        [Then(@"API should return list of currencies")]
        public void APIShouldReturnListOfCurrencies()
        {
            Assert.NotNull(_availableCurrencies);
            Assert.IsNotEmpty(_availableCurrencies);
        }

        [Given(@"I have a valid table identifier ""(.*)"" and currency code ""(.*)""")]
        public void GivenIHaveAValidTableAndCurrency(string table, string currency)
        {
            ScenarioContext.Current["Table"] = table;
            ScenarioContext.Current["Currency"] = currency;
        }

        [Given(@"a date range from ""(.*)"" to ""(.*)""")]
        public void GivenADateRangeFromTo(string startDate, string endDate)
        {
            ScenarioContext.Current["StartDate"] = DateTime.Parse(startDate);
            ScenarioContext.Current["EndDate"] = DateTime.Parse(endDate);
        }

        [When(@"I fetch historical currency rates")]
        public async Task WhenIFetchHistoricalCurrencyRates()
        {
            var table = ScenarioContext.Current["Table"].ToString();
            var currency = ScenarioContext.Current["Currency"].ToString();
            var startDate = (DateTime)ScenarioContext.Current["StartDate"];
            var endDate = (DateTime)ScenarioContext.Current["EndDate"];
            
            var service = new CurrencyService(_httpClient, _mapper, _rateRepositoryMock.Object); // Twoja klasa serwisowa
            _currencyHistory = await service.FetchCurrencyHistoryAsync(table, currency, startDate, endDate);
        }

        [Then(@"the API should return the currency rates for each day in the range")]
        public void ThenTheAPIShouldReturnTheCurrencyRatesForEachDayInTheRange()
        {
            Assert.NotNull(_currencyHistory);
            Assert.IsNotEmpty(_currencyHistory);

            var startDate = (DateTime)ScenarioContext.Current["StartDate"];
            var endDate = (DateTime)ScenarioContext.Current["EndDate"];

            var daysInRange = (endDate - startDate).Days + 1;
            Assert.True(_currencyHistory.Count <= daysInRange); // Sprawdzamy, czy wszystkie dni s¹ w zakresie
        }

        [When($"I connect to the database")]
        public void WhenIConnectToDatabase()
        {
            var connection = _dbContext.Database.GetDbConnection();
            connection.Open(); // Próba nawi¹zania po³¹czenia
        }

        [AfterScenario]
        public void Dispose()
        {
            _dbContext?.Dispose();
        }

        
    }
   
        

}