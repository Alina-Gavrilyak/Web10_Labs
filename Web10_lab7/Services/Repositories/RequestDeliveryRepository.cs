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
    public class RequestDeliveryRepository : IRequestDeliveryRepository {
        private readonly IMapper mapper;
        private readonly TurnoverDbContext db;

        public RequestDeliveryRepository(TurnoverDbContext db, IMapper mapper) {
            this.db = db;
            this.mapper = mapper;
        }

        public IEnumerable<RequestDeliveryDTO> GetAll() {
            return GetQueryableEntities();
        }

        public RequestDeliveryDTO Get(int id) {
            return GetQueryableEntities()
                .FirstOrDefault(x => x.Id == id);
        }

        private IQueryable<RequestDeliveryDTO> GetQueryableEntities() {
            return db.RequestDeliverys
                .Include(x => x.Products)
                .Include(x => x.Warehouse)
                .Include(x => x.Shop)
                .ProjectTo<RequestDeliveryDTO>(mapper.ConfigurationProvider);
        }

        public int Add(RequestDeliveryInputDTO inputEntity) {
            RequestDelivery newEntity = mapper.Map<RequestDelivery>(inputEntity);
            CreateReferences(newEntity, inputEntity);
            db.RequestDeliverys.Add(newEntity);
            db.SaveChanges();
            return newEntity.Id;
        }

        public void Update(int id, RequestDeliveryInputDTO inputEntity) {
            RequestDelivery entityToUpdate = db.RequestDeliverys.Find(id);
            mapper.Map(inputEntity, entityToUpdate);
            CreateReferences(entityToUpdate, inputEntity);
            db.SaveChanges();
        }

        private void CreateReferences(RequestDelivery entity, RequestDeliveryInputDTO inputEntity) {
            entity.Products = db.Products.Where(x => inputEntity.ProductIds.Contains(x.Id)).ToList();
        }

        public void Remove(int id) {
            IQueryable<RequestDelivery> entityToRemove = db.RequestDeliverys.Where(x => x.Id == id);
            db.RequestDeliverys.RemoveRange(entityToRemove);
        }
    }
}
