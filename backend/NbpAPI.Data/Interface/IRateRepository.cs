using NbpAPI.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NbpAPI.Data.Interface
{
    public interface IRateRepository
    {
        List<RateByDate> GetAll();
        Task<List<RateByDate>> GetRatesForDateRangeAsync(string currencyCode, DateTime startDate, DateTime endDate);
        RateByDate Create(RateByDate rate);
    }
}
