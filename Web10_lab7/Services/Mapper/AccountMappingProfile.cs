using AutoMapper;
using DataAccessContracts.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiModels.AccountApiModels;

namespace Services.Mapper {
    public class AccountMappingProfile : Profile {
        public AccountMappingProfile() {
            CreateMap<RegisterModel, TurnoverUser>();
        }
    }
}
