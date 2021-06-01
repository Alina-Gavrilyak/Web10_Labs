using AutoMapper;
using AutoMapper.QueryableExtensions;
using Contracts.Models;
using Contracts.Repositories;
using DataAccessContracts.Entities;
using DataAccessServices;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Services.Repositories {
    public class ShopRepository : IShopRepository {
        private readonly IMapper mapper;
        private readonly TurnoverDbContext db;

        public ShopRepository(TurnoverDbContext db, IMapper mapper) {
            this.db = db;
            this.mapper = mapper;
        }

        public IEnumerable<ShopDTO> GetAll() {
            return GetQueryableEntities();
        }

        public ShopDTO Get(int id) {
            return GetQueryableEntities()
                .FirstOrDefault(x => x.Id == id);
        }

        private IQueryable<ShopDTO> GetQueryableEntities() {
            return db.Shops
                .Include(x => x.Products)
                .Include(x => x.RequestDeliveries)
                .ProjectTo<ShopDTO>(mapper.ConfigurationProvider);
        }

        public int Add(ShopInputDTO inputEntity) {
            Shop newEntity = mapper.Map<Shop>(inputEntity);
            CreateReferences(newEntity, inputEntity);
            db.Shops.Add(newEntity);
            db.SaveChanges();
            return newEntity.Id;
        }

        public void Update(int id, ShopInputDTO inputEntity) {
            Shop entityToUpdate = db.Shops.Find(id);
            mapper.Map(inputEntity, entityToUpdate);
            CreateReferences(entityToUpdate, inputEntity);
            db.SaveChanges();
        }

        private void CreateReferences(Shop entity, ShopInputDTO inputEntity) {
            entity.Products = db.Products.Where(x => inputEntity.ProductIds.Contains(x.Id)).ToList();
        }

        public void Remove(int id) {
            IQueryable<Shop> entityToRemove = db.Shops.Where(x => x.Id == id);
            db.Shops.RemoveRange(entityToRemove);
        }
    }
}
