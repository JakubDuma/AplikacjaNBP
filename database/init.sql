-- Tworzenie bazy danych
CREATE DATABASE NBPAPI;
GO

-- Ustawienie bazy danych do użycia
USE NBPAPI;
GO

--ALTER LOGIN sa WITH DEFAULT_DATABASE = master;
--GO

CREATE TABLE RatesByDate (
    Id INT PRIMARY KEY IDENTITY(1,1),
    CurrencyCode NVARCHAR(100) NOT NULL,
    No NVARCHAR(200) NOT NULL,
    Mid DECIMAL(18, 4) NOT NULL,
    EffectiveDate DATETIME NOT NULL
);
GO
