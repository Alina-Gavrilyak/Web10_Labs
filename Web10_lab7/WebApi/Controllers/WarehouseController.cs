using Contracts.Models;
using Contracts.Repositories;
using DataAccessContracts.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WebApi.Helpers;

namespace WebApi.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WarehouseController : ControllerBase {
        private readonly IWarehouseRepository repository;

        public WarehouseController(IWarehouseRepository repository) {
            this.repository = repository;
        }

        [HttpGet]
        public IEnumerable<WarehouseDTO> GetAllWarehouses() {
            return repository.GetAll();
        }

        [HttpGet("{id}")]
        public ActionResult<WarehouseDTO> GetWarehouse(int id) {
            WarehouseDTO item = repository.Get(id);
            if (item == null) {
                return NotFound();
            }
            return item;
        }

        [HttpPost]
        [Authorize(Roles = RolesHelper.AdminRole)]
        public int PostWarehouse(WarehouseInputDTO item) {
            return repository.Add(item);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = RolesHelper.AdminRole)]
        public void PutWarehouse(int id, WarehouseInputDTO warehouse) {
            repository.Update(id, warehouse);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = RolesHelper.AdminRole)]
        public void DeleteWarehouse(int id) {
            repository.Remove(id);
        }
    }
}
