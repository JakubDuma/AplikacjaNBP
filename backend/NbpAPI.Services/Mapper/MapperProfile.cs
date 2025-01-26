using AutoMapper;
using NbpAPI.Data.Models;
using NbpAPI.Services.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NbpAPI.Services.MapperProfile
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<RateByDate, RateByDateDTO>().ReverseMap();
            CreateMap<Rate, RateDTO>().ReverseMap();
        }
    }
}
