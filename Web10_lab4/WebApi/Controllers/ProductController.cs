using DataAccessContracts.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Contracts.Repositories;
using Contracts.Models;
using Microsoft.AspNetCore.Authorization;
using WebApi.Helpers;

namespace WebApi.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase {
        private readonly IProductRepository repository;

        public ProductController(IProductRepository repository) {
            this.repository = repository;
        }

        [HttpGet]
        public IEnumerable<ProductDTO> GetAllProducts() {
            return repository.GetAll();
        }

        [HttpGet("{id}")]
        public ActionResult<ProductDTO> GetProduct(int id) {
            ProductDTO item = repository.Get(id);
            if (item == null) {
                return NotFound();
            }
            return item;
        }

        [HttpPost]
        [Authorize(Roles = RolesHelper.AdminRole)]
        public int PostProduct(ProductInputDTO item) {
            return repository.Add(item);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = RolesHelper.AdminRole)]
        public void PutProduct(int id, ProductInputDTO product) {
            repository.Update(id, product);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = RolesHelper.AdminRole)]
        public void DeleteProduct(int id) {
            repository.Remove(id);
        }
    }
}
