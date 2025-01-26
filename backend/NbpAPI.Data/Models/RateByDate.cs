using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NbpAPI.Data.Models
{
    public class RateByDate
    {
        public int Id { get; set; }
        public string CurrencyCode {  get; set; }
        public string No { get; set; }
        public DateTime EffectiveDate {  get; set; }
        public decimal Mid { get; set; }
    }
}
