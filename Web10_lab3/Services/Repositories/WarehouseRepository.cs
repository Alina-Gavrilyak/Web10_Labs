using AutoMapper;
using AutoMapper.QueryableExtensions;
using Contracts.Models;
using Contracts.Repositories;
using DataAccessContracts.Entities;
using DataAccessServices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Repositories {
    public class WarehouseRepository : IWarehouseRepository {
        private readonly IMapper mapper;
        private readonly TurnoverDbContext db;

        public WarehouseRepository(TurnoverDbContext db, IMapper mapper) {
            this.db = db;
            this.mapper = mapper;
        }

        public IEnumerable<WarehouseDTO> GetAll() {
            return GetQueryableEntities();
        }

        public WarehouseDTO Get(int id) {
            return GetQueryableEntities()
                .FirstOrDefault(x => x.Id == id);
        }

        private IQueryable<WarehouseDTO> GetQueryableEntities() {
            return db.Warehouses
                .Include(x => x.Products)
                .Include(x => x.RequestDeliveries)
                .ProjectTo<WarehouseDTO>(mapper.ConfigurationProvider);
        }

        public int Add(WarehouseInputDTO inputEntity) {
            Warehouse newEntity = mapper.Map<Warehouse>(inputEntity);
            CreateReferences(newEntity, inputEntity);
            db.Warehouses.Add(newEntity);
            db.SaveChanges();
            return newEntity.Id;
        }

        public void Update(int id, WarehouseInputDTO inputEntity) {
            Warehouse entityToUpdate = db.Warehouses.Find(id);
            mapper.Map(inputEntity, entityToUpdate);
            CreateReferences(entityToUpdate, inputEntity);
            db.SaveChanges();
        }

        private void CreateReferences(Warehouse entity, WarehouseInputDTO inputEntity) {
            entity.Products = db.Products.Where(x => inputEntity.ProductIds.Contains(x.Id)).ToList();
        }

        public void Remove(int id) {
            IQueryable<Warehouse> entityToRemove = db.Warehouses.Where(x => x.Id == id);
            db.Warehouses.RemoveRange(entityToRemove);
        }
    }
}
