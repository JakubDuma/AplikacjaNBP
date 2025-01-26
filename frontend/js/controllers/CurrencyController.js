app.controller('CurrencyController', function ($scope, CurrencyService) {
  // Zmienna przechowująca dostępne waluty
  $scope.currencies = [];
  $scope.selectedCurrency = '';
  $scope.tables = ["A", "B", "C"];
  $scope.selectedTable = ''; // Domyślnie wybieramy tabelę A
  $scope.startDate = '';
  $scope.endDate = '';
  $scope.chart = null;

  // Funkcja do pobrania dostępnych walut
  function fetchCurrencies() {
    CurrencyService.getAvailableCurrencies($scope.selectedTable).then(function (data) {
      $scope.currencies = data;
    });
  }
  function fetchCurrencyHistory() {
    CurrencyService.postCurrencyHistory($scope.selectedTable, $scope.selectedCurrency, $scope.startDate, $scope.endDate)
  }
  // Wywołaj fetchCurrencies przy zmianie tabeli
  $scope.$watch('selectedTable', function () {
    fetchCurrencies();
  });

  $scope.$watchGroup(['startDate', 'endDate'], function () {
    fetchCurrencyHistory();
  });

  $scope.onTableChange = function() {
    $scope.startDate = null;
    $scope.endDate = null;
    $scope.getAvailableCurrencies(); // Pobierz dostępne waluty dla nowej tabeli
};

// Funkcja resetująca daty przy zmianie waluty
$scope.onCurrencyChange = function() {
    $scope.startDate = null;
    $scope.endDate = null;
};

  // Początkowe załadowanie walut
  fetchCurrencies();

  // Funkcja do pobrania historii kursu waluty
  $scope.fetchCurrencyHistory = function () {
    if (!$scope.selectedCurrency || !$scope.startDate || !$scope.endDate) {
      alert('Please select a currency and a date range');
      return;
    }

    CurrencyService.postCurrencyHistory($scope.selectedTable, $scope.selectedCurrency, $scope.startDate, $scope.endDate);
    CurrencyService.getCurrencyHistory($scope.selectedCurrency, $scope.startDate, $scope.endDate).then(function (data) {
      // Generowanie wykresu z danych kursów
      generateChart(data);
    });
  };

  // Funkcja do generowania wykresu
  function generateChart(data) {
    var ctx = document.getElementById('currencyChart').getContext('2d');
    
    // Jeśli wykres już istnieje, usuń go przed narysowaniem nowego
    if ($scope.chart) {
      $scope.chart.destroy();
    }

    // Przygotowanie danych do wykresu
    var labels = data.map(item => item.effectiveDate);
    var chartData = data.map(item => item.mid);

    $scope.chart = new Chart(ctx, {
      type: 'line',
      data: {
        labels: labels,
        datasets: [{
          label: 'Currency Rate',
          data: chartData,
          borderColor: 'rgba(75, 192, 192, 1)',
          backgroundColor: 'rgba(75, 192, 192, 0.2)',
          fill: true
        }]
      },
      options: {
        responsive: true,
        scales: {
          y: {
            beginAtZero: true
          }
        }
      }
    });
  }
});