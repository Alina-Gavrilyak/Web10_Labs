using DataAccessContracts.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.Repositories;
using Contracts.Models;

namespace WebApi.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class ShopController : ControllerBase {
        private readonly IShopRepository repository;

        public ShopController(IShopRepository repository) {
            this.repository = repository;
        }

        [HttpGet]
        public IEnumerable<ShopDTO> GetAllShops() {
            return repository.GetAll();
        }

        [HttpGet("{id}")]
        public ActionResult<ShopDTO> GetShop(int id) {
            ShopDTO item = repository.Get(id);
            if (item == null) {
                return NotFound();
            }
            return item;
        }

        [HttpPost]
        public int PostShop(ShopInputDTO item) {
            return repository.Add(item);
        }

        [HttpPut("{id}")]
        public void PutShop(int id, ShopInputDTO shop) {
            repository.Update(id, shop);
        }

        [HttpDelete("{id}")]
        public void DeleteShop(int id) {
            repository.Remove(id);
        }
    }
}
