﻿using AutoMapper;
using Contracts.Models;
using DataAccessContracts.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Mapper {
    public class WarehouseMappingProfile : Profile {
        public WarehouseMappingProfile() {
            CreateMap<Warehouse, WarehouseBaseDTO>();

            CreateMap<Warehouse, WarehouseDTO>()
                .IncludeBase<Warehouse, WarehouseBaseDTO>();

            CreateMap<WarehouseInputDTO, Warehouse>();
        }
    }
}
