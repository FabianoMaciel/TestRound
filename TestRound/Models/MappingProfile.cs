using AutoMapper;
using DataAcess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestRound.Models
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CompanyModel, Company>();
            CreateMap<Company, CompanyModel>();
        }
    }
}
