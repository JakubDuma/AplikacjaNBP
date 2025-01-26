app.service('CurrencyService', function ($http) {
  const baseUrl = 'http://localhost:8080/currencies/';

  // Funkcja do pobierania dostępnych walut
  this.getAvailableCurrencies = function (selectedTable) {
    return $http.get(baseUrl + 'GetExchangeRates' + `?table=${selectedTable}`)
      .then(function (response) {
        return response.data;
      }, function (error) {
        console.error('Error fetching available currencies:', error);
        return [];
      });
  };

  // Funkcja do pobierania historii kursu waluty
  this.postCurrencyHistory = function (tableCode, currencyCode, startDate, endDate) {
    const url = `${baseUrl}DateHistory?tableCode=${tableCode}&currencyCode=${currencyCode}&startDate=${startDate.toISOString().split('T')[0]}&endDate=${endDate.toISOString().split('T')[0]}`;
    return $http.post(url)
      .then(function () {
      }, function (error) {
        console.error('Error fetching currency history:', error);
        return [];
      });
  };

  this.getCurrencyHistory = function (currencyCode, startDate, endDate) {
    const url = `${baseUrl}RatesDateRange?currency=${currencyCode}&startDate=${startDate.toISOString().split('T')[0]}&endDate=${endDate.toISOString().split('T')[0]}`;
    return $http.get(url)
      .then(function (response) {
        return response.data;
      }, function (error) {
        console.error('Error fetching currency history:', error);
        return [];
      });
  };

});