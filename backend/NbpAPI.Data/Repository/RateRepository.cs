using Microsoft.EntityFrameworkCore;
using NbpAPI.Data.Interface;
using NbpAPI.Data.Models;

namespace NbpAPI.Data.Repository
{
    public class RateRepository : IRateRepository
    {
        private readonly DBContext _context;
        public RateRepository(DBContext context)
        {
            _context = context;
        }
        public List<RateByDate> GetAll()
        {
            return  _context.RatesByDate.AsNoTracking().ToList();
        }
        public async Task<List<RateByDate>> GetRatesForDateRangeAsync(string currencyCode, DateTime startDate, DateTime endDate)
        {
            return await _context.RatesByDate
                .AsNoTracking()
                .Where(r => r.CurrencyCode == currencyCode && r.EffectiveDate >= startDate && r.EffectiveDate <= endDate)
                .OrderBy(r => r.EffectiveDate)
                .ToListAsync();
        }
        public RateByDate Create(RateByDate rate)
        {
            _context.RatesByDate.Add(rate);
            _context.SaveChanges();
            return rate;
        }
    }
}
