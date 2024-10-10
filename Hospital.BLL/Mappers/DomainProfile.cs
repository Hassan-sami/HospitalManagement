using AutoMapper;
using Hospital.BLL.ModelVM;
using Hospital.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.BLL.Mappers
{
    public class DomainProfile : Profile
    {
        public DomainProfile() 
        {
            CreateMap<RegisterViewModel, Patient>();
        }
    }
}
