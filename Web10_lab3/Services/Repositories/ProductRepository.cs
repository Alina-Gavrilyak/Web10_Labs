using AutoMapper;
using AutoMapper.QueryableExtensions;
using Contracts.Models;
using Contracts.Repositories;
using DataAccessContracts.Entities;
using DataAccessServices;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Services.Repositories {
    public class ProductRepository : IProductRepository {

        private readonly TurnoverDbContext db;
        private readonly IMapper mapper;

        public ProductRepository(TurnoverDbContext db, IMapper mapper) {
            this.db = db;
            this.mapper = mapper;
        }

        public IEnumerable<ProductDTO> GetAll() {
            return GetQueryableProducts();
        }

        public ProductDTO Get(int id) {
            return GetQueryableProducts()
                .FirstOrDefault(x => x.Id == id);
        }

        private IQueryable<ProductDTO> GetQueryableProducts() {
            return db.Products
                .Include(x => x.Shops)
                .Include(x => x.Warehouses)
                .Include(x => x.RequestDeliveries)
                .ProjectTo<ProductDTO>(mapper.ConfigurationProvider);
        }

        public int Add(ProductInputDTO inputEntity) {
            Product newEntity = mapper.Map<Product>(inputEntity);
            CreateReferences(newEntity, inputEntity);
            db.Products.Add(newEntity);
            db.SaveChanges();
            return newEntity.Id;
        }

        public void Update(int id, ProductInputDTO inputEntity) {
            Product entityToUpdate = db.Products
                .Include(x => x.Shops)
                .Include(x => x.Warehouses)
                .First(x => x.Id == id);
            mapper.Map(inputEntity, entityToUpdate);
            CreateReferences(entityToUpdate, inputEntity);
            db.SaveChanges();
        }

        private void CreateReferences(Product entity, ProductInputDTO inputEntity) {
            entity.Shops = db.Shops.Where(x => inputEntity.ShopIds.Contains(x.Id)).ToList();
            entity.Warehouses = db.Warehouses.Where(x => inputEntity.WarehouseIds.Contains(x.Id)).ToList();
        }

        public void Remove(int id) {
            IQueryable<Product> entityToRemove = db.Products.Where(x => x.Id == id);
            db.Products.RemoveRange(entityToRemove);
            db.SaveChanges();
        }
    }
}
