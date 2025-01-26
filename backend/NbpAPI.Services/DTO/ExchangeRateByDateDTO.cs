using NbpAPI.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NbpAPI.Services.DTO
{
    public class ExchangeRateByDateDTO
    {
        public string Table { get; set; }
        public string Currency { get; set; }
        public string Code { get; set; }
        public List<RateByDateDTO> Rates { get; set; }
    }
}
