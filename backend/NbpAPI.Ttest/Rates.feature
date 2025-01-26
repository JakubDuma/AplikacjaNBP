Feature: Fetching currency data

	Scenario: Fetching available currencies
	Given I have a valid table identifier "A"
	When I fetch available currencies
	Then API should return list of currencies

	Scenario: Fetching currency history
    Given I have a valid table identifier "A" and currency code "USD"
    And a date range from "2023-01-01" to "2023-01-15"
    When I fetch historical currency rates
    Then the API should return the currency rates for each day in the range

	Scenario: Checking database connection
    Given the database is running
    When I connect to the database
    Then the connection should be successful